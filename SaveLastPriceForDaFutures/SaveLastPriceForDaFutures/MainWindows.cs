using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;
using System.Collections.Concurrent;
using ServiceStack.Redis;
using System.IO;  
using System.Runtime.Serialization.Json;
using ServiceStack.Redis.Support;
using CommonClassLib;


namespace SaveLastPriceForDaFutures
{
    public partial class MainWindows : Form
    {
        /// <summary>
        /// 连接行情服务器的客户端
        /// </summary>
        private MarketTcpIp marketTcpClient = null;
        public WriteLog marketErrorLoger = null;
        private bool isSave = false;

        /// <summary>
        /// 行情保存集合
        /// </summary>
        public BlockingCollection<MarketInfo> marketBlockingCollection;

        /// <summary>
        /// 行情连接集合
        /// </summary>
        public BlockingCollection<MarketTcpIp> marketTcpIpBlockingCollection;

        /// <summary>
        /// 心跳执行
        /// </summary>
        private System.Timers.Timer HeartTimer;

        /// <summary>
        /// 发送心跳的时间间隔(30秒)
        /// </summary>
        private int headBitTime = 30000;

        /// <summary>
        /// 保存线程
        /// </summary>
        private Thread saveToFile;

        /// <summary>
        /// 重连线程
        /// </summary>
        private Thread reconnectThread;

        public string marketType;
        IRedisClient Redis = null;
        public  string DaFutures = "DaFutures";
        public  string DaStock = "DaStock";
        private  string LastPrice = "LastPrice";

        public MainWindows()
        {
            InitializeComponent();
            marketBlockingCollection = new BlockingCollection<MarketInfo>();
            marketTcpIpBlockingCollection = new BlockingCollection<MarketTcpIp>(); 
            string serverIP = ConfigurationManager.AppSettings["MarketIp"];
            string serverPort = ConfigurationManager.AppSettings["MarketPort"];
            marketType = ConfigurationManager.AppSettings["MarketType"];
            this.Text = this.Text + "_" + marketType;
            marketErrorLoger = new WriteLog("./log/marketError.log",LogLevel.ALL);
            //获取Redis操作接口
            Redis = RedisManager.GetClient();
            //放在1号数据库
            //Redis.Db = 1;
            marketTcpClient = new MarketTcpIp(serverIP, int.Parse(serverPort));
            marketTcpClient.mainWindow = this;
            marketTcpClient.setVersion(100);
            marketTcpClient.setSize(1024 *1024 *4);
            //marketTcpClient.isWriteLog = true;
            marketTcpClient.Connect();
            isSave = true;

        }

        public void PrintToTxt(string content)
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    if (this.txtInfo.Text.Length > 1024)
                    {
                        this.txtInfo.Text = "";
                    }
                    this.txtInfo.AppendText(DateTime.Now.ToString() + " " + content);
                    this.txtInfo.AppendText(Environment.NewLine);

                }));
            }
            catch (Exception ex)
            {
                marketErrorLoger.log(LogLevel.SYSTEMERROR, ex.ToString());
            }

        }

        /// <summary>
        /// 请求行情
        /// </summary>
        /// <param name="code"></param>
        public void AskMarket()
        {
            if (marketTcpClient != null && marketTcpClient.isOnReconnecting)
            {
                Thread.Sleep(1000);
                NetInfo netInfo = new NetInfo();
                //if (marketType.Equals(DaFutures))
                //{
                //    //请求结算价
                //    netInfo.code = CommandCode.GetSettlePrice;
                //    netInfo.infoT = "";
                //    netInfo.todayCanUse = "";
                //    Thread.Sleep(1000);
                //    marketTcpClient.sendData(netInfo.MyToString());

                //}

                //结算价完成以后 请求最新行情
                netInfo = new NetInfo();
                netInfo.code = CommandCode.MARKET01;
                netInfo.infoT = "";
                netInfo.todayCanUse = "++";
                marketTcpClient.sendData(netInfo.MyToString());
              
            }
            PrintToTxt("请求行情");

        }

        private string ReadRequestContract()
        {
            //string requestFile = "";
            //string filepath = ".\\config\\requestContract.txt";
            //string[] AllLine = File.ReadAllLines(filepath, System.Text.Encoding.Default);
            //if (AllLine != null && AllLine.Length > 0)
            //{
            //    foreach (string data in AllLine)
            //    {
            //        if (!string.IsNullOrEmpty(data))
            //        {
            //            requestFile = requestFile + data;
            //        }
            //    }
            //}
            return "++";
        }

        /// <summary>
        /// 往行情阻塞里面加行情
        /// </summary>
        /// <param name="marketinfo"></param>
        public void AddMarketBlock(MarketInfo marketinfo)
        {
            try
            {
                if (!isSave)
                {
                    return;
                }
              
                marketBlockingCollection.TryAdd(marketinfo);

            }
            catch (Exception ex)
            {
                marketErrorLoger.log(LogLevel.SYSTEMERROR, ex.ToString());
            }
        }

        private void MainWindows_FormClosing(object sender, FormClosingEventArgs e)
        {
       
            if (MessageBox.Show("是否要关闭程序？", "警告", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            this.CloseForm();

        }

        private void CloseForm()
        {
            try
            {
                isSave = false;
                if (marketTcpClient != null)
                {
                    marketTcpClient.shutdown();
                }
                if (marketErrorLoger != null)
                {
                    marketErrorLoger.close();
                }
                if (saveToFile != null)
                {
                    saveToFile.Abort();
                }
                if (reconnectThread != null)
                {
                    reconnectThread.Abort();
                }
                if (Redis != null)
                {
                    Redis.Dispose();
                }
            }
            catch
            {
            }

        }

        /// <summary>
        /// 启动发送心跳包数据的线程
        /// </summary>
        private void sendHeartBitThread()
        {
            if (HeartTimer == null)
            {
                HeartTimer = new System.Timers.Timer(headBitTime);  //实例化Timer类，设置间隔时间为毫秒；   
                HeartTimer.Elapsed += new System.Timers.ElapsedEventHandler(sendHeartHit);  //到达时间的时候执行事件   
                HeartTimer.AutoReset = false;  //设置是执行一次（false）还是一直执行(true)   
                HeartTimer.Enabled = true;  //是否执行System.Timers.Timer.Elapsed事件
                HeartTimer.Start();
            }
            else
            {
                HeartTimer.Enabled = true;
                HeartTimer.Start();
            }
        }

        private void MainWindows_Load(object sender, EventArgs e)
        {
            saveToFile = new Thread(ExecuteMarketThread);
            saveToFile.IsBackground = true;
            saveToFile.Start();
            PrintToTxt("开始保存。");
            //reconnectThread = new Thread(ExecuteReconnectThread);
            //reconnectThread.IsBackground = true;
            //reconnectThread.Start();

            Thread.Sleep(1000);
            //执行心跳
            sendHeartBitThread();
        }

        /// <summary>
        /// 及时器启动发送心跳的方法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void sendHeartHit(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (marketTcpClient != null)
                {
                    NetInfo info = new NetInfo();
                    info.code = CommandCode.HEARTBIT;
                    marketTcpClient.sendData(info.MyToString());
                }

                if (marketTcpClient.client.State == HPSocketCS.ServiceState.Stoped){
                    marketTcpClient.ReConnect();
                }
                if (e.SignalTime.Minute == 0)
                {
                   /// AskMarket();
                    marketErrorLoger.close();
                    marketErrorLoger = new WriteLog("./log/marketError.log",LogLevel.ALL);
                }
            }
            catch
            {
            }
            finally
            {
                HeartTimer.Start();
            }
        }

        /// <summary>
        /// 执行行情接受线程
        /// </summary>
        public void ExecuteMarketThread()
        {
            string key = "";
            string value = "";
            MarketInfo marketInfo = null;
            MyMarketInfo mm = null;
            var ser = new ObjectSerializer();
            HashOperator operators = new HashOperator();

            while (isSave)
            {
                try
                {
                    marketInfo = marketBlockingCollection.Take();
                    if (marketInfo.tradeFlag == "D" || marketInfo.tradeFlag == "X" || marketInfo.tradeFlag == "P")
                    {
                        //港股这种行情的成交价,成交量不要往列表里面刷
                        marketInfo.currPrice = "";
                        marketInfo.filledNum = "";
                        marketInfo.currNumber = "";
                    }
                    //合并隐藏价格
                    //if (!string.IsNullOrEmpty(marketInfo.hideBuyPrice) || !string.IsNullOrEmpty(marketInfo.hideSalePrice))
                    //{
                    //    if (!string.IsNullOrEmpty(marketInfo.hideBuyPrice))
                    //    {
                    //        marketInfo.type = "B";
                    //    }
                    //    if (!string.IsNullOrEmpty(marketInfo.hideSalePrice))
                    //    {
                    //        marketInfo.type = "O";
                    //    }
                    //    if (!string.IsNullOrEmpty(marketInfo.hideBuyPrice) && !string.IsNullOrEmpty(marketInfo.hideSalePrice))
                    //    {
                    //        marketInfo.type = "Z";
                    //    }
                    //    marketInfo.mergeHidePrice("");
                    //}
                    //value = Encoding.UTF8.GetString(ser.Serialize(marketInfo));
                    //operators.Set<byte[]>("test", "key1", ser.Serialize(marketInfo));
                   
                    if (DaFutures.Equals(this.marketType))
                    {
                        //直达期货
                        key = "00," + marketInfo.code;
                    }
                    else if (DaStock.Equals(this.marketType))
                    {
                        //直达股票
                        key = "10," + marketInfo.code;
                        //Console.WriteLine(marketInfo.exchangeCode);
                        if ("US".Equals(marketInfo.exchangeCode))
                        {
                            marketInfo.exchangeCode = "NASD";
                        }
                    }
                    
                    value = Redis.GetValueFromHash(LastPrice, key);
                    mm = new MyMarketInfo();
                    if (!string.IsNullOrEmpty(value))
                    {
                        mm = (MyMarketInfo)JsonToObject(value, mm);
                    }
                   
                    upDateMarketData(marketInfo, mm);
                    value = ObjectToJson(mm);
                    Redis.SetEntryInHash(LastPrice, key, value);
   
                }
                catch (Exception ex)
                {
                    marketErrorLoger.log(LogLevel.SYSTEMERROR, ex.ToString());
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            AskMarket();
            PrintToTxt("重新请求行情");
        }


        /// <summary>
        /// 更新全局的行情数据
        /// </summary>     
        private  void upDateMarketData(MarketInfo dataFrom, MyMarketInfo dateTo)
        {
            dateTo.code = dataFrom.code;
            dateTo.exchangeCode = dataFrom.exchangeCode;
            //最高价
            if (!String.IsNullOrEmpty(dataFrom.high))
            {
                dateTo.high = dataFrom.high;
            }
            //收盘价
            if (!String.IsNullOrEmpty(dataFrom.close))
            {
                //收盘价为空的时候，放最新价
                dateTo.close = dataFrom.close;
            }

            //开盘价
            if (!String.IsNullOrEmpty(dataFrom.open))
            {
                dateTo.open = dataFrom.open;
            }
            //最低价
            if (!String.IsNullOrEmpty(dataFrom.low))
            {
                dateTo.low = dataFrom.low;
            }
            //成交量
            if (!String.IsNullOrEmpty(dataFrom.filledNum) || dataFrom.filledNum != "0")
            {
                dateTo.filledNum = dataFrom.filledNum;
            }
            //持仓量
            if (!String.IsNullOrEmpty(dataFrom.holdNum))
            {
                dateTo.holdNum = dataFrom.holdNum;
            }
            //昨结算
            if (!String.IsNullOrEmpty(dataFrom.oldClose) && !"0".Equals(dataFrom.oldClose))
            {
                dateTo.oldClose = dataFrom.oldClose;
            }
            //现量
            if (!String.IsNullOrEmpty(dataFrom.currNumber) || dataFrom.currNumber != "0")
            {
                dateTo.currNumber = dataFrom.currNumber;
            }
            //最新价
            if (!String.IsNullOrEmpty(dataFrom.currPrice))
            {
                dateTo.currPrice = dataFrom.currPrice;
            }
            //更新时间
            if (!String.IsNullOrEmpty(dataFrom.time))
            {
                dateTo.time = dataFrom.time;
            }
            //行情类型
            if (!String.IsNullOrEmpty(dataFrom.type))
            {
                dateTo.type = dataFrom.type;
            }
             if (DaFutures.Equals(this.marketType)){
                 if (!string.IsNullOrEmpty(dataFrom.buyPrice) && !string.IsNullOrEmpty(dataFrom.buyNumber))
                 {
                     //买量1
                     dateTo.buyNumber = dataFrom.buyNumber;
                     //买量2
                     dateTo.buyNumber2 = dataFrom.buyNumber2;
                     //买量3
                     dateTo.buyNumber3 = dataFrom.buyNumber3;
                     //买量4
                     dateTo.buyNumber4 = dataFrom.buyNumber4;
                     //买量5
                     dateTo.buyNumber5 = dataFrom.buyNumber5;
                     //买价1
                     dateTo.buyPrice = dataFrom.buyPrice;
                     //买价2
                     dateTo.buyPrice2 = dataFrom.buyPrice2;
                     //买价3
                     dateTo.buyPrice3 = dataFrom.buyPrice3;
                     //买价4
                     dateTo.buyPrice4 = dataFrom.buyPrice4;
                     //买价5
                     dateTo.buyPrice5 = dataFrom.buyPrice5;
                 }

                 //隐藏买量
                 dateTo.hideBuyNumber = dataFrom.hideBuyNumber;
                 //隐藏买价
                 dateTo.hideBuyPrice = dataFrom.hideBuyPrice;

                 if (!string.IsNullOrEmpty(dataFrom.salePrice) && !string.IsNullOrEmpty(dataFrom.saleNumber))
                 {
                     //卖量1
                     dateTo.saleNumber = dataFrom.saleNumber;
                     //卖量2
                     dateTo.saleNumber2 = dataFrom.saleNumber2;
                     //卖量3
                     dateTo.saleNumber3 = dataFrom.saleNumber3;
                     //卖量4
                     dateTo.saleNumber4 = dataFrom.saleNumber4;
                     //卖量5
                     dateTo.saleNumber5 = dataFrom.saleNumber5;
                     //卖价1
                     dateTo.salePrice = dataFrom.salePrice;
                     //卖价2
                     dateTo.salePrice2 = dataFrom.salePrice2;
                     //卖价3
                     dateTo.salePrice3 = dataFrom.salePrice3;
                     //卖价4
                     dateTo.salePrice4 = dataFrom.salePrice4;
                     //卖价5
                     dateTo.salePrice5 = dataFrom.salePrice5;
                 }

                 //隐藏卖量
                 dateTo.hideSaleNumber = dataFrom.hideSaleNumber;
                 //隐藏卖价
                 dateTo.hideSalePrice = dataFrom.hideSalePrice;
             }
                    
           

           
        }

        //从一个对象信息生成Json串
        private  string ObjectToJson(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }
        // 从一个Json串生成对象信息
        private object JsonToObject(string jsonString, object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            return serializer.ReadObject(mStream);
        }

         /// <summary>
        /// 执行重连线程
        /// </summary>
        public void ExecuteReconnectThread()
        {
            while (isSave)
            {
                try
                {
                    MarketTcpIp marketTcpIp = marketTcpIpBlockingCollection.Take();
                   // if (!marketTcpIp.client.isConnected){
                    if (!marketTcpIp.isOnReconnecting)
                    {
                        Thread.Sleep(1000);
                        PrintToTxt("正在重连...");
                        marketTcpIp.ReConnect();
                        
                    }   
                }
                catch (Exception ex)
                {
                    marketErrorLoger.log(LogLevel.SYSTEMERROR, ex.ToString());
                }
            }
        }

    }
}
