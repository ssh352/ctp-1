using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TDFAPI;
using System.Threading;
using CommonClassLib;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace ChinaStockMarket
{
    public partial class MainWindow : Form
    {


        TDFOpenSetting openSettins = null;
        TDFSourceImp dataSource = null;
        byte[] sendData = null;
        Socket udpSendSocket = null;
        String sendTargetIp = null;
        String sendTargetPort = null;
        IPEndPoint ipEndPoint = null;
        public MainWindow()
        {
            InitializeComponent();
            //异常日志
            ChinaMarketValue.exceptionLog = new CommonClassLib.WriteLog("./log/exception.log", ChinaMarketValue.logLevel);  
            ChinaMarketValue.blkMarketInfo = new System.Collections.Concurrent.BlockingCollection<Object>();
            PrintHelper.rtb = this.txtOutPut;
            ChinaMarketValue.dicMarketInfo = new System.Collections.Concurrent.ConcurrentDictionary<string, MarketInfo>();
            ChinaMarketValue.dicTDFCode = new System.Collections.Concurrent.ConcurrentDictionary<string,TDFCode>();
            sendData = new byte[1024];
            udpSendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sendTargetIp = System.Configuration.ConfigurationManager.AppSettings["SendTargetIp"];
            sendTargetPort = System.Configuration.ConfigurationManager.AppSettings["SendTargetPort"];
            ipEndPoint = new IPEndPoint(IPAddress.Parse(sendTargetIp), Convert.ToInt32(sendTargetPort));
            btnStart.Enabled = true;
            btnClose.Enabled = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ChinaMarketValue.isStartMarket = true;
            if (ChinaMarketValue.threadMarket == null)
            {
                ChinaMarketValue.threadMarket = new Thread(UpdateMarkets);
                ChinaMarketValue.threadMarket.IsBackground = true;
                ChinaMarketValue.threadMarket.Start();
            }
                   

            if (openSettins == null)
            {
                openSettins = new TDFOpenSetting()
                {
                    Ip = System.Configuration.ConfigurationManager.AppSettings["IP"],                                    //服务器Ip
                    Port = System.Configuration.ConfigurationManager.AppSettings["Port"],                                //服务器端口
                    Username = System.Configuration.ConfigurationManager.AppSettings["Username"],                        //服务器用户名
                    Password = System.Configuration.ConfigurationManager.AppSettings["Password"],                        //服务器密码
                    Subscriptions = System.Configuration.ConfigurationManager.AppSettings["SubScriptions"],              //订阅列表，以 ; 分割的代码列表，例如:if1406.cf;if1403.cf；如果置为空，则全市场订阅
                    Markets = System.Configuration.ConfigurationManager.AppSettings["Markets"],                          //市场列表，以 ; 分割，例如: sh;sz;cf;shf;czc;dce
                    ReconnectCount = uint.Parse(System.Configuration.ConfigurationManager.AppSettings["ReconnectCount"]),//当连接断开时重连次数，断开重连在TDFDataSource.Connect成功之后才有效
                    ReconnectGap = uint.Parse(System.Configuration.ConfigurationManager.AppSettings["ReconnectGap"]),    //重连间隔秒数
                    ConnectionID = uint.Parse(System.Configuration.ConfigurationManager.AppSettings["ConnectionID"]),    //连接ID，标识某个Open调用，跟回调消息中TDFMSG结构nConnectionID字段相同
                    Date = uint.Parse(System.Configuration.ConfigurationManager.AppSettings["Date"]),                    //请求的日期，格式YYMMDD，为0则请求今天
                    Time = (uint)int.Parse(System.Configuration.ConfigurationManager.AppSettings["Time"]),               //请求的时间，格式HHMMSS，为0则请求实时行情，为(uint)-1从头请求
                    TypeFlags = (uint)int.Parse(System.Configuration.ConfigurationManager.AppSettings["TypeFlags"])      //unchecked((uint)DataTypeFlag.DATA_TYPE_ALL);   //为0请求所有品种，或者取值为DataTypeFlag中多种类别，比如DATA_TYPE_MARKET | DATA_TYPE_TRANSACTION
                };

            }

           
            if (dataSource == null)
            {
                dataSource = new TDFSourceImp(openSettins);
                dataSource.SetEnv(EnvironSetting.TDF_ENVIRON_OUT_LOG, 1);
                TDFERRNO nOpenRet = dataSource.Open();
                if (nOpenRet == TDFERRNO.TDF_ERR_SUCCESS)
                {
                    PrintHelper.PrintText("connect success!\n");
                }
                else
                {
                    //这里判断错误代码，进行对应的操作，对于 TDF_ERR_NETWORK_ERROR，用户可以选择重连
                    PrintHelper.PrintText(string.Format("open returned:{0}, program quit", nOpenRet));                    
                }
            }
            
            btnStart.Enabled = false;
            btnClose.Enabled = true;
        }

        /// <summary>
        /// 刷新行情集合
        /// </summary>
        private void UpdateMarkets()
        {
            while (ChinaMarketValue.isStartMarket)
            {
                try
                {
                    object obj = ChinaMarketValue.blkMarketInfo.Take();
                    if (obj is TDFMarketData)
                    {
                        TDFMarketData data = (TDFMarketData)obj;
                        MarketInfo market = null;
                        if (ChinaMarketValue.dicMarketInfo.ContainsKey(data.WindCode))
                        {
                            market = ChinaMarketValue.dicMarketInfo[data.WindCode];

                        }
                        else
                        {
                            market = new MarketInfo();
                            ChinaMarketValue.dicMarketInfo.TryAdd(data.WindCode, market);
                        }
                        ConvertTDFMarketData(data, market);
                        if (market.type == "2")
                        {
                            MarketInfo market2 = new MarketInfo();
                            market2.exchangeCode = market.exchangeCode;
                            market2.code = market.code;
                            market2.currNumber = market.currNumber;
                            market2.currPrice = market2.currPrice;
                            market2.time = market2.time;
                            SendUDPPacket(market2.MyToString());
                            market.type = "Z";
                        }
                        SendUDPPacket(market.MyToString());
                    }
                }
                catch (Exception ex)
                {
                    ChinaMarketValue.exceptionLog.log(ChinaMarketValue.logLevel, ex.ToString());
                }

            }                       
        }

        private void ConvertTDFMarketData(TDFMarketData tdfdata, MarketInfo data)
        {
            if (ChinaMarketValue.dicTDFCode.ContainsKey(tdfdata.WindCode))
            {
                string day = tdfdata.ActionDay.ToString().Substring(0, 4) + "-" + tdfdata.ActionDay.ToString().Substring(4, 2) + "-" + tdfdata.ActionDay.ToString().Substring(6, 2);
                string time = "";
                if (tdfdata.Time < 100000000)
                {
                    //10点以前
                    time = "0" + tdfdata.Time.ToString().Substring(0, 1) + ":" + tdfdata.Time.ToString().Substring(1, 2) + ":" + tdfdata.Time.ToString().Substring(3, 2);
                }
                else
                {
                    time = tdfdata.Time.ToString().Substring(0, 2) + ":" + tdfdata.Time.ToString().Substring(2, 2) + ":" + tdfdata.Time.ToString().Substring(4, 2);
                }

                data.time = day + " " + time;
                if (!string.IsNullOrEmpty(data.time))
                {
                    //换一天的时候把成交量清掉
                    if (day.CompareTo(data.time.Substring(0,10)) > 0)
                    {
                        data.filledNum = "0";
                    }
                }
                
                data.exchangeCode = ChinaMarketValue.dicTDFCode[tdfdata.WindCode].Market;
                data.code = tdfdata.WindCode;
                data.salePrice = FormatFracPrx(tdfdata.AskPrice[0]);
                data.saleNumber = (tdfdata.AskVol[0] ).ToString();
                data.buyPrice = FormatFracPrx(tdfdata.BidPrice[0]).ToString();
                data.buyNumber = (tdfdata.BidVol[0]).ToString();
                data.currPrice = FormatFracPrx(tdfdata.Match).ToString();
               
                if (tdfdata.Volume > CommonFunction.StringToDecimal(data.filledNum) && tdfdata.Volume > 0)
                {
                    //成交数据
                    data.currNumber = (tdfdata.Volume - (long)CommonFunction.StringToDecimal(data.filledNum)).ToString();
                    data.type = "2";
                }
                else
                {
                    data.currNumber = "0";
                    data.type = "Z";
                }
                data.filledNum = tdfdata.Volume.ToString();
                //data.currNumber = (Tdfdata.Match / 10000).ToString();
                data.high = FormatFracPrx(tdfdata.High).ToString();
                data.low = FormatFracPrx(tdfdata.Low).ToString();
                data.open = FormatFracPrx(tdfdata.Open).ToString();
                data.oldClose = FormatFracPrx(tdfdata.PreClose).ToString();
               

                data.salePrice2 = FormatFracPrx(tdfdata.AskPrice[1]).ToString();
                data.salePrice3 = FormatFracPrx(tdfdata.AskPrice[2]).ToString();
                data.salePrice4 = FormatFracPrx(tdfdata.AskPrice[3]).ToString();
                data.salePrice5 = FormatFracPrx(tdfdata.AskPrice[4]).ToString();
                data.saleNumber2 = (tdfdata.AskVol[1]).ToString();
                data.saleNumber3 = (tdfdata.AskVol[2]).ToString();
                data.saleNumber4 = (tdfdata.AskVol[3]).ToString();
                data.saleNumber5 = (tdfdata.AskVol[4]).ToString();
                data.buyPrice2 = FormatFracPrx(tdfdata.BidPrice[1]).ToString();
                data.buyPrice3 = FormatFracPrx(tdfdata.BidPrice[2]).ToString();
                data.buyPrice4 = FormatFracPrx(tdfdata.BidPrice[3]).ToString();
                data.buyPrice5 = FormatFracPrx(tdfdata.BidPrice[4]).ToString();
                data.buyNumber2 = (tdfdata.BidVol[1]).ToString();
                data.buyNumber3 = (tdfdata.BidVol[2]).ToString();
                data.buyNumber4 = (tdfdata.BidVol[3]).ToString();
                data.buyNumber5 = (tdfdata.BidVol[4]).ToString();
            }
        }
        
        private void SendUDPPacket(string strMarket)
        {
            int count = strMarket.Length;
            count = System.Text.ASCIIEncoding.ASCII.GetBytes(strMarket, 0, count, sendData, 0);
            udpSendSocket.SendTo(sendData, 0, count, SocketFlags.None, ipEndPoint);           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ChinaMarketValue.isStartMarket = false;
            if (dataSource != null)
            {
                dataSource.Close();
                dataSource.Dispose();
                dataSource = null;
            }
            try
            {
                if (ChinaMarketValue.threadMarket != null)
                {
                    ChinaMarketValue.threadMarket.Abort();
                    ChinaMarketValue.threadMarket = null;
                }
            }
            catch { }
          
           
            btnStart.Enabled = true;
            btnClose.Enabled = false;

        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnClose_Click(null, null);
        }

        private  string FormatFracPrx(uint price )
        {           
            decimal rawPrx = (decimal)price / 10000;
            return String.Format(rawPrx.ToString(), "#.00");
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (ChinaMarketValue.dicTDFCode != null && ChinaMarketValue.dicTDFCode.Count > 0)
                {
                    SaveFileDialog sf = new SaveFileDialog();
                    sf.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                    sf.FileName = @"A股合约.csv";
                    sf.AddExtension = true;
                    sf.Title = "导出合约";
                    if (sf.ShowDialog() == DialogResult.OK)
                    {
                        List<TDFCode> lstCode = ChinaMarketValue.dicTDFCode.Values.ToList();
                        IEnumerable<TDFCode> query = lstCode.OrderBy(p => p.Code);
                        string temp = ""; ;
                        FileStream fs = new FileStream(sf.FileName, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw.Write("万德商品编号,商品名称,市场编号,商品类型,原始商品编号" + "\r\n");
                        foreach (TDFCode tfdCode in query)
                        {
                            temp = tfdCode.WindCode + "," + tfdCode.CNName + "," + tfdCode.Market + "," +
                                 tfdCode.Type +  "," + tfdCode.Code + "\r\n";
                            sw.Write(temp);
                        }
                        sw.Flush();
                        sw.Close();
                        MessageBox.Show("导出成功！");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); 
            }
        }
    }
}
