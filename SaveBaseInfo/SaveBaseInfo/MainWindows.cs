using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Configuration;
using CommonClassLib;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace SaveBaseInfo
{
    public partial class MainWindow : Form
    {

        private WriteLog  marketErrorLoger = null;

        private bool isSave = false;

        /// <summary>
        /// 返回保存集合
        /// </summary>
        private BlockingCollection<string> marketBlockingCollection;
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

        private MarketTcpIp marketTcpClient = null;
        string marketType;
        string user = null;
        string pwd = null;

        string maxdate = "";
        /// <summary>
        /// 保存线程
        /// </summary>
        private Thread saveToFile;
        /// <summary>
        /// 重连线程
        /// </summary>
        private Thread reconnectThread;

        private const string DaFutures = "DaFutures";
        private const string DaStock = "DaStock";
        private string startRequestTime = null;

        private List<Exchange> lstExchange = null;

        private List<string> lstUnExchange = null;


        public MainWindow()
        {
            InitializeComponent();
            string serverIP = ConfigurationManager.AppSettings["MarketIp"];
            string serverPort = ConfigurationManager.AppSettings["MarketPort"];
            marketType = ConfigurationManager.AppSettings["MarketType"];
            user = ConfigurationManager.AppSettings["user"];
            pwd = ConfigurationManager.AppSettings["pwd"];
            startRequestTime = ConfigurationManager.AppSettings["Req_product_time"];
            marketBlockingCollection = new BlockingCollection<string>();
            marketTcpIpBlockingCollection = new BlockingCollection<MarketTcpIp>(); 
            lstExchange = new List<Exchange>();
            string value = ConfigurationManager.AppSettings["exceptExchange"];
            lstUnExchange = new List<string>();
            if (!string.IsNullOrEmpty(value))
            {
                lstUnExchange = new List<string>(value.Split(','));
            }
            this.Text = this.Text + "_" + marketType;
            marketErrorLoger = new WriteLog("./log/marketError.log",LogLevel.ALL);
            marketTcpClient = new MarketTcpIp(serverIP, int.Parse(serverPort));
            marketTcpClient.mainWindow = this;
            marketTcpClient.setVersion(100);
            marketTcpClient.Connect();
            marketTcpClient.setSize(1024 * 1024 * 4);
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


        /// <summary>
        /// 及时器启动发送心跳的方法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void sendHeartHit(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (marketTcpClient.client.State == HPSocketCS.ServiceState.Stoped)
                {
                    marketTcpClient.ReConnect();
                }
                if (startRequestTime.Equals(e.SignalTime.ToString("hh:mm")))
                {
                    
                    btnRequest_Click(null, null);
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

        private void MainWindow_Load(object sender, EventArgs e)
        {
            saveToFile = new Thread(ExecuteMarketThread);
            saveToFile.IsBackground = true;
            saveToFile.Start();
            //reconnectThread = new Thread(ExecuteReconnectThread);
            //reconnectThread.IsBackground = true;
            //reconnectThread.Start();

            Thread.Sleep(1000);
            //执行心跳
            sendHeartBitThread();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        public void SendLoginInfo()
        {
            try
            {
                if (marketTcpClient != null && marketTcpClient.isOnReconnecting)
                {
                    Thread.Sleep(1000);
                    NetInfo ni = new NetInfo();
                    if (DaFutures.Equals(marketType))
                    {
                        ni.code = CommandCode.LOGIN;
                        ni.todayCanUse = "I";
                        LoginInfo tp = new LoginInfo();
                        tp.userId = user;
                        tp.userPwd = pwd;
                        ni.infoT = tp.MyToString();
                    }
                    else if (DaStock.Equals(marketType))
                    {
                        ni.code = CommandCode.LOGINHK;
                        ni.todayCanUse = "I";
                        LoginInfo tp = new LoginInfo();
                        tp.userId = user;
                        tp.userPwd = pwd;
                        tp.userType = "1";
                        ni.infoT = tp.MyToString();
                    }
                    marketTcpClient.sendData(ni.MyToString());
                }
               
            }
            catch (Exception ex)
            {
                marketErrorLoger.log(LogLevel.SYSTEMERROR, ex.ToString());
            }
        }
        /// <summary>
        /// 往阻塞里面加返回数据
        /// </summary>
        /// <param name="marketinfo"></param>
        public void AddMarketBlock(string value)
        {
            try
            {
                if (!isSave)
                {
                    return;
                }

                marketBlockingCollection.TryAdd(value);

            }
            catch (Exception ex)
            {
                marketErrorLoger.log(LogLevel.SYSTEMERROR, ex.ToString());
            }
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
             
            }
            catch
            {
            }

        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否要关闭程序？", "警告", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            this.CloseForm();
        }

        /// <summary>
        /// 执行行情接受线程
        /// </summary>
        public void ExecuteMarketThread()
        {
            string value = "";
            while (isSave)
            {
                try
                {
                    value = marketBlockingCollection.Take();
                    NetInfo ni = new NetInfo();
                    ni.MyReadString(value);
                    
                    if (CommandCode.LOGIN.Equals(ni.code))
                    {
                        if (string.IsNullOrEmpty(ni.errorCode) || "00000".Equals(ni.errorCode))
                        {
                            PrintToTxt(user + " 登录成功!");
                        }
                        else
                        {
                            PrintToTxt(user + " 登录失败!");
                        }
                    }

                    if (CommandCode.LOGINHK.Equals(ni.code))
                    {
                        if (string.IsNullOrEmpty(ni.errorCode) || "00000".Equals(ni.errorCode))
                        {
                            PrintToTxt(user + " 登录成功!");
                        }
                        else
                        {
                            PrintToTxt(user + " 登录失败!");
                        }
                    }

                    if (CommandCode.GETTCONTRACT.Equals(ni.code))
                    {
                        //期货品种
                        List<UseContract> lstUc = CommonFunction.ObjectListFromString<UseContract>(UseContract.MyPropToString(), ni.infoT);
                        if (lstUc != null && lstUc.Count > 0)
                        {
                            InsertOrUpdateDaFutures(lstUc);
                            int received = CommonFunction.StringToInt(ni.systemCode);
                            if (received != 0)
                            {

                                RequestContract(received);
                            }
                        }
                        else
                        {
                            
                            PrintToTxt("期货产品更新完成 " + ReloadData(ConfigurationManager.AppSettings["futures_reload_url"]));
                        }
                      
                      
                    }
                    if (CommandCode.CURRENCYLIST.Equals(ni.code))
                    {
                        //期货汇率
                        List<CurrencyInfo> lstUc = CommonFunction.ObjectListFromString<CurrencyInfo>(CurrencyInfo.MyPropToString(), ni.infoT);
                        foreach (CurrencyInfo ci in lstUc)
                        {
                            UpdateCurrency(ci);
                        }
                        
                        PrintToTxt("汇率更新完成 " + ReloadData(ConfigurationManager.AppSettings["currency_reload_url"]));
                    }
                    if (CommandCode.GetContractStock.Equals(ni.code))
                    {
                        List<ContractStockHK> lstUc = CommonFunction.ObjectListFromString<ContractStockHK>(ContractStockHK.MyPropToString(), ni.infoT);
                        if (lstUc != null && lstUc.Count > 0)
                        {
                            foreach (ContractStockHK ci in lstUc)
                            {
                                InsertOrUpdateDaStock(ci);
                            }
                            int received = CommonFunction.StringToInt(ni.systemCode);
                            if (received != 0)
                            {

                                RequestContract(received);
                            }
                        }
                        else
                        {

                            PrintToTxt("证券产品更新完成 " + ReloadData(ConfigurationManager.AppSettings["stock_reload_url"]));
                        }
                  
                    }
                    if (CommandCode.GetPlateList.Equals(ni.code) || CommandCode.GetPlateListStockHK.Equals(ni.code))
                    {
                        List<MyPlateCommodityInfo> list = CommonFunction.ObjectListFromString<MyPlateCommodityInfo>(PlateCommodityInfo.MyPropToString(), ni.infoT);
                        int i = 0;

                        foreach (MyPlateCommodityInfo ci in list)
                        {
                            ci.FSortId = i++.ToString();
                            
                        }
                        InsertOrUpdatePlateView(list);
                        PrintToTxt("板块更新完成 " + ReloadData(ConfigurationManager.AppSettings["plate_reload_url"]));
                    }
                }
                catch (Exception ex)
                {
                    marketErrorLoger.log(LogLevel.SYSTEMERROR, ex.ToString());
                }
                  
            }
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            PrintToTxt("更新开始");
            //删除过期合约
            //deleteExpiredContract();
            //取交易所
            getExchange();
            //取汇率
            RequestCurrency();
            Thread.Sleep(1000);
            maxdate = "";
            maxdate = getMaxDate();
            //取合约
            RequestContract(0);
            Thread.Sleep(1000);
            //取板块
            RequestPlate();
        }

        private void RequestCurrency()
        {
           
            if (DaFutures.Equals(marketType))
            {
                NetInfo ni = new NetInfo();
                //请求期货汇率
                ni.code = CommandCode.CURRENCYLIST;
                marketTcpClient.sendData(ni.MyToString());

            }
        }

        private void RequestPlate()
        {
            NetInfo ni = new NetInfo();
            if (DaFutures.Equals(marketType))
            {
                
                //请求期货板块
                ni.code = CommandCode.GetPlateList;
                marketTcpClient.sendData(ni.MyToString());

            }
            else
            {
                //股票板块
                ni.clientNo = "1";
                ni.code = CommandCode.GetPlateListStockHK;
                marketTcpClient.sendData(ni.MyToString());
            }
        }

        private void RequestContract(int number)
        {
            NetInfo ni = new NetInfo();
            if (DaFutures.Equals(marketType))
            {
                //请求期货合约
                ni.code = CommandCode.GETTCONTRACT;
                ni.systemCode = number.ToString();
                ni.localSystemCode = maxdate;
                ni.infoT = "";
                marketTcpClient.sendData(ni.MyToString());

            }
            else if (DaStock.Equals(marketType))
            {
                //股票合约
                ni.code = CommandCode.GetContractStock;
                ni.systemCode = number.ToString();
                ni.localSystemCode = maxdate;
                ni.infoT = "";
                ni.clientNo = "1";
                marketTcpClient.sendData(ni.MyToString());
            }
        }

      
        private void InsertOrUpdateDaFutures(List<UseContract> lstUc)
        {

           
            MySqlCommand sqlselectcom = new MySqlCommand();
            sqlselectcom.CommandText = SQLText.selectfuturesSQL;
           
            foreach (UseContract uc in lstUc){
                if (lstUnExchange.FirstOrDefault(p=>p == uc.FExchangeNo) != null)
                {
                    //在直达的数据库中有国内期货，不要加载
                    continue ;
                }

                MySqlParameter[] commandParameters = new MySqlParameter[]{
                    new MySqlParameter("@exchange_no",uc.FExchangeNo ),
                     new MySqlParameter("@code",uc.code )
                 };

                MySqlDataReader reader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionStringManager, CommandType.Text, sqlselectcom.CommandText, commandParameters);
                if (reader.Read() == true)
                {

                    insertSQL(true, uc);
                }
                else
                {
                    insertSQL(false, uc);
                }
                reader.Close();
                reader.Dispose();
            }

        }

        private void InsertOrUpdateDaStock(List<UseContract> lstUc)
        {

            MySqlCommand sqlselectcom = new MySqlCommand();
            sqlselectcom.CommandText = SQLText.selectfuturesSQL;

            foreach (UseContract uc in lstUc)
            {
                MySqlParameter[] commandParameters = new MySqlParameter[]{
                    new MySqlParameter("@exchange_no",uc.FExchangeNo ),
                     new MySqlParameter("@code",uc.code )
                 };
                if (uc.FExchangeNo == "LME")
                {
                    uc.FTradeMonth = "208512";
                }
                MySqlDataReader reader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionStringManager, CommandType.Text, sqlselectcom.CommandText, commandParameters);
                if (reader.Read() == true)
                {

                    insertSQL(true, uc);
                }
                else
                {
                    insertSQL(false, uc);
                }
                reader.Close();
                reader.Dispose();
            }

        }

        private string getMaxDate()
        {
            string maxDate = "";
            MySqlCommand sqlcom = new MySqlCommand();
            if (marketType.Equals(DaFutures))
            {
                //期货
                sqlcom.CommandText = SQLText.futuresMaxDate;
            }
            else if (marketType.Equals(DaStock))
            {
                //港股
                sqlcom.CommandText = SQLText.stockMaxDate;
            }
           
            
            MySqlDataReader reader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionStringManager, CommandType.Text, sqlcom.CommandText);
            if (reader.Read() == true)
            {
                if (reader["max(update_date)"] != DBNull.Value){
                     maxDate = ((DateTime)reader["max(update_date)"]).ToString("yyyyMMdd");
                }
               
            }
            reader.Close();
            reader.Dispose();
            return maxDate;
        }

        private void insertSQL(bool isdelete,UseContract uc){
            string[] cmdTexts = null;
            MySqlParameter[][] commandParameters = null;
            MySqlParameter[] delcommandParameters = new MySqlParameter[]{
                    new MySqlParameter("@exchange_no",uc.FExchangeNo ),
                     new MySqlParameter("@code",uc.code )
                    };

            Exchange ec  =  lstExchange.FirstOrDefault(p=>p.FExchangeNo == uc.FExchangeNo);
            if (ec !=null){
                //将期货的交易所名字换成中文
                uc.FName = ec.FName;
            }
            MySqlParameter[] insertcommandParameters = new MySqlParameter[]{
                  new MySqlParameter("@exchange_no",uc.FExchangeNo ),
                  new MySqlParameter("@exchange_name",uc.FName ),
                  new MySqlParameter("@commodity_no",uc.FCommodityNo ),
                  new MySqlParameter("@commodity_name",uc.CommodityFName ),
                  new MySqlParameter("@code",uc.code ),
                  new MySqlParameter("@contract_no",uc.FContractNo ),
                  new MySqlParameter("@contract_name",uc.ContractFName ),
                  new MySqlParameter("@futures_type",uc.FCommodityType ),
                  new MySqlParameter("@product_dot",uc.FProductDot ),
                  new MySqlParameter("@upper_tick",uc.FUpperTick ),
                  new MySqlParameter("@reg_date",DateTime.Now.ToString("yyyyMMdd")),
                  new MySqlParameter("@expiry_date",uc.FTradeMonth ),
                  new MySqlParameter("@dot_num",uc.FDotNum ),
                  new MySqlParameter("@currency_no",uc.CommodityFCurrencyNo ),
                  new MySqlParameter("@currency_name",uc.CurrencyFName ),
                  new MySqlParameter("@lower_tick",uc.FLowerTick ),
                  new MySqlParameter("@exchange_no2",uc.FExchange2 ),
                  new MySqlParameter("@deposit",uc.FFreezenMoney ),
                  new MySqlParameter("@deposit_percent",uc.FFreezenPercent ),
                  new MySqlParameter("@first_notice_day",uc.FTradeMonth ),
                  new MySqlParameter("@create_by","batch_id" ),
                  new MySqlParameter("@create_date",DateTime.Now ),
                  new MySqlParameter("@update_by","batch_id"  ),
                  new MySqlParameter("@update_date",DateTime.Now ),
                  new MySqlParameter("@commodity_type","00" ),
                  new MySqlParameter("@py_name",GetStringSpell.GetChineseSpell(uc.ContractFName) )
                
                };
            if (isdelete)
            {
                //需要更新的时候
                cmdTexts = new string[]
                {
                    SQLText.deletefuturesSQL,
                    SQLText.insertfuturesSQL
                };
                commandParameters = new MySqlParameter[][]{
                    delcommandParameters,
                    insertcommandParameters
                };
            }
            else
            {
                cmdTexts = new string[]
                {
                   // deletefuturesSQL,
                    SQLText.insertfuturesSQL
                };
                commandParameters = new MySqlParameter[][]{
                    //delcommandParameters,
                    insertcommandParameters
                };
            }
           bool isInsert =  MySqlHelper.ExecuteTransaction(MySqlHelper.ConnectionStringManager, CommandType.Text, cmdTexts, commandParameters);
           if (!isInsert)
           {
               PrintToTxt(uc.code + " 产品更新失败");
               marketErrorLoger.log(LogLevel.SYSTEMERROR, uc.code + " 产品更新失败");
           }
          
        }

        //更新货币
        private void UpdateCurrency(CurrencyInfo ci)
        {
            string[] cmdTexts = null;
            MySqlParameter[][] commandParameters = null;
            MySqlParameter[] delcommandParameters = new MySqlParameter[]{
                    new MySqlParameter("@currency_no",ci.currencyNo)
             };

            MySqlParameter[] insertcommandParameters = new MySqlParameter[]{
                  new MySqlParameter("@currency_no",ci.currencyNo ),
                  new MySqlParameter("@currency_name",ci.currencyName ),
                  new MySqlParameter("@is_primary",ci.isBase ),
                  new MySqlParameter("@exchange",ci.rate ),
                 new MySqlParameter("@create_by","batch_id" ),
                  new MySqlParameter("@create_date",DateTime.Now ),
                  new MySqlParameter("@update_by","batch_id"  ),
                  new MySqlParameter("@update_date",DateTime.Now )
                };
            //需要更新的时候
            cmdTexts = new string[]
                {
                    SQLText.deleteCurrencySQL,
                    SQLText.insertCurrencySQL
                };
            commandParameters = new MySqlParameter[][]{
                   delcommandParameters,
                    insertcommandParameters
                };
            bool isInsert = MySqlHelper.ExecuteTransaction(MySqlHelper.ConnectionStringManager, CommandType.Text, cmdTexts, commandParameters);
            if (!isInsert)
            {
                PrintToTxt(ci.currencyNo + " 汇率更新失败");
                marketErrorLoger.log(LogLevel.SYSTEMERROR, ci.currencyNo + " 汇率更新失败");
            }

        }

        private void getExchange(){
            
            lstExchange.Clear();
            MySqlDataReader reader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionStringManager, CommandType.Text, SQLText.selectExchange);
            while (reader.Read() == true)
            {

                Exchange ec = new Exchange();
                ec.FExchangeNo = reader["exchange_no"].ToString();
                ec.FName = reader["exchange_name"].ToString();
                lstExchange.Add(ec);
            }
           
            reader.Close();
            reader.Dispose();
        }

        private void InsertOrUpdateDaStock(ContractStockHK uc)
        {


            MySqlCommand sqlselectcom = new MySqlCommand();
            sqlselectcom.CommandText = SQLText.selectStockSQL;

         

            MySqlParameter[] commandParameters = new MySqlParameter[]{
                    new MySqlParameter("@exchange_no",uc.FExchangeNo ),
                     new MySqlParameter("@stock_no",uc.FCommodityNo )
                 };

            MySqlDataReader reader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionStringManager, CommandType.Text, sqlselectcom.CommandText, commandParameters);
            if (reader.Read() == true)
            {
                string stock_name = DBValueToStr(reader, "stock_name");
                string mortgage_percent = DBValueToStr(reader, "mortgage_percent");
                string upper_tick_code = DBValueToStr(reader, "upper_tick_code");

                if (stock_name.Equals(uc.FCommodityName) && upper_tick_code.Equals(uc.FUpperTickCode) && CommonFunction.StringToDecimal(mortgage_percent) == CommonFunction.StringToDecimal(uc.FMortgagePercent))
                {
                }
                else
                {
                    insertStockSQL(true, uc);
                }
               
            }
            else
            {
                insertStockSQL(false, uc);
            }
            reader.Close();
            reader.Dispose();

        }

        
        private void InsertOrUpdatePlateView(List<MyPlateCommodityInfo> lstUc)
        {
            MySqlCommand sqlselectcom = new MySqlCommand();
            sqlselectcom.CommandText = SQLText.selectPlateView;
             MySqlCommand sqldelcom = new MySqlCommand();
            sqldelcom.CommandText = SQLText.deletePlateView;
            string type = null;
            if (marketType.Equals(DaFutures))
            {
                type = "00";
                //期货
              
            }
            else
            {
                type = "10";
                //股票
                
            }
              MySqlParameter[] commandParameters = commandParameters = new MySqlParameter[]{
                    new MySqlParameter("@commodity_type",type)};
            MySqlDataReader reader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionStringManager, CommandType.Text, sqlselectcom.CommandText, commandParameters);
            while (reader.Read() == true)
            {
                string plate_group_id = DBValueToStr(reader, "plate_group_id");
                string plate_id = DBValueToStr(reader, "plate_id");
                string exchange_no = DBValueToStr(reader, "exchange_no");
                string commodity_no = DBValueToStr(reader, "commodity_no");
                string plate_group_name = DBValueToStr(reader, "plate_group_name");
                string plate_name = DBValueToStr(reader, "plate_name");
                string group_sort_id = DBValueToStr(reader, "group_sort_id");
                string sort_id = DBValueToStr(reader, "sort_id");

                MyPlateCommodityInfo uc = lstUc.FirstOrDefault(p => p.FPlateGroupId == plate_group_id && p.FPlateId == plate_id && p.FExchangeNo == exchange_no && p.FComodityNo == commodity_no);
                if (uc != null)
                {
                    if (plate_group_name != uc.FPlateGroupName || plate_name != uc.FPlateName || uc.FPlateGroupId != group_sort_id || uc.FSortId != sort_id)
                    {
                       
                    }else{
                        lstUc.Remove(uc);
                    }
                }else{

                    //服务器没有的合约的时候把数据库删除
                    // commandParameters = commandParameters = new MySqlParameter[]{
                    //new MySqlParameter("@plate_group_id",plate_group_id),new MySqlParameter("@plate_id",plate_id),
                    //        new MySqlParameter("@exchange_no",exchange_no),new MySqlParameter("@commodity_no",commodity_no),new MySqlParameter("@commodity_type",type)};

                    // MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionStringManager, CommandType.Text, sqldelcom.CommandText, commandParameters);

                }

                //将服务器的数据更新到数据库
            }
            reader.Close();
            reader.Dispose();

            foreach (MyPlateCommodityInfo uc in lstUc)
            {
                 insertPlateViewSQL(uc);
            }

        }
        private void insertPlateViewSQL( MyPlateCommodityInfo  uc)
        {
            string type = null;
            if (marketType.Equals(DaFutures))
            {
                type = "00";
                //期货

            }
            else
            {
                type = "10";
                //股票

            }
            string[] cmdTexts = null;
            MySqlParameter[][] commandParameters = null;
            MySqlParameter[] delcommandParameters = new MySqlParameter[]{new MySqlParameter("@plate_group_id",uc.FPlateGroupId),new MySqlParameter("@plate_id",uc.FPlateId),
                            new MySqlParameter("@exchange_no",uc.FExchangeNo),new MySqlParameter("@commodity_no",uc.FComodityNo),new MySqlParameter("@commodity_type",type)};

           
            MySqlParameter[] insertcommandParameters = new MySqlParameter[]{
                  new MySqlParameter("@plate_group_id",uc.FPlateGroupId ),
                  new MySqlParameter("@plate_group_name",uc.FPlateGroupName ),
                  new MySqlParameter("@plate_id",uc.FPlateId ),
                  new MySqlParameter("@plate_name",uc.FPlateName ),
                  new MySqlParameter("@exchange_no",uc.FExchangeNo ),
                  new MySqlParameter("@commodity_no",uc.FComodityNo),
                  new MySqlParameter("@commodity_type",type ),
                  new MySqlParameter("@group_sort_id","0" ),
                  new MySqlParameter("@sort_id",uc.FSortId ),
                  new MySqlParameter("@create_by","batch_id" ),
                  new MySqlParameter("@create_date",DateTime.Now ),
                  new MySqlParameter("@update_by","batch_id"  ),
                  new MySqlParameter("@update_date",DateTime.Now ) 
                };
            //需要更新的时候
            cmdTexts = new string[]
                {
                    SQLText.deletePlateView,
                    SQLText.insertPlateViewSQL
                };
            commandParameters = new MySqlParameter[][]{
                    delcommandParameters,
                    insertcommandParameters
                };


            bool isInsert = MySqlHelper.ExecuteTransaction(MySqlHelper.ConnectionStringManager, CommandType.Text, cmdTexts, commandParameters);
            if (!isInsert)
            {
                PrintToTxt(uc.MyToString() + " 板块更新失败");
                marketErrorLoger.log(LogLevel.SYSTEMERROR, uc.MyToString() + " 产品更新失败");
            }

        }

        private void insertStockSQL(bool isdelete, ContractStockHK uc)
        {
            string[] cmdTexts = null;
            MySqlParameter[][] commandParameters = null;
            MySqlParameter[] delcommandParameters = new MySqlParameter[]{
                    new MySqlParameter("@exchange_no",uc.FExchangeNo ),
                     new MySqlParameter("@stock_no",uc.FCommodityNo )
                    };

            Exchange ec = lstExchange.FirstOrDefault(p => p.FExchangeNo == uc.FExchangeNo);
            if (ec != null)
            {
                //将期货的交易所名字换成中文
                uc.FExchangeName = ec.FName;
            }
            MySqlParameter[] insertcommandParameters = new MySqlParameter[]{
                  new MySqlParameter("@exchange_no",uc.FExchangeNo ),
                  new MySqlParameter("@exchange_name",uc.FExchangeName ),
                  new MySqlParameter("@stock_no",uc.FCommodityNo ),
                  new MySqlParameter("@stock_name",uc.FCommodityName ),
                  new MySqlParameter("@stock_type",uc.FCommodityType ),
                  new MySqlParameter("@reg_date",DateTime.Now.ToString("yyyyMMdd")),
                  new MySqlParameter("@currency_no",uc.FCurrencyNo ),
                  new MySqlParameter("@currency_name",uc.FCurrencyName ),
                  new MySqlParameter("@lot_size",uc.FLotSize ),
                  new MySqlParameter("@mortgage_percent",uc.FMortgagePercent ),
                  new MySqlParameter("@upper_tick_code",uc.FUpperTickCode ),
                  new MySqlParameter("@create_by","batch_id" ),
                  new MySqlParameter("@create_date",DateTime.Now ),
                  new MySqlParameter("@update_by","batch_id"  ),
                  new MySqlParameter("@update_date",DateTime.Now ),
                  new MySqlParameter("@commodity_type","10" ),
                  new MySqlParameter("@py_name",GetStringSpell.GetChineseSpell(uc.FCommodityName) )
                
                };
            if (isdelete)
            {
                //需要更新的时候
                cmdTexts = new string[]
                {
                    SQLText.deleteStockSQL,
                    SQLText.insertStockSQL
                };
                commandParameters = new MySqlParameter[][]{
                    delcommandParameters,
                    insertcommandParameters
                };
            }
            else
            {
                cmdTexts = new string[]
                {
                   // deletefuturesSQL,
                    SQLText.insertStockSQL
                };
                commandParameters = new MySqlParameter[][]{
                    //delcommandParameters,
                    insertcommandParameters
                };
            }
            bool isInsert = MySqlHelper.ExecuteTransaction(MySqlHelper.ConnectionStringManager, CommandType.Text, cmdTexts, commandParameters);
            if (!isInsert)
            {
                PrintToTxt(uc.FCommodityNo  + " 产品更新失败");
                marketErrorLoger.log(LogLevel.SYSTEMERROR, uc.FCommodityNo + " 产品更新失败");
            }

        }

        private void deleteExpiredContract()
        {
            MySqlCommand sqlcom = new MySqlCommand();
            MySqlParameter[] delcommandParameters = null;
            if (marketType.Equals(DaFutures))
            {
                //期货
                sqlcom.CommandText = SQLText.deleteExpiredfuturesSQL;
                delcommandParameters = new MySqlParameter[]{
                    new MySqlParameter("@expiry_date",DateTime.Now.ToString("yyyyMM") ),
                    };
              
            }
            else if (marketType.Equals(DaStock))
            {
                //股票
                sqlcom.CommandText = SQLText.deleteExpiredStockSQL;
                delcommandParameters = new MySqlParameter[]{
                    new MySqlParameter("@maturity_date",DateTime.Now.ToString("yyyy-MM-dd") ),
                    };
            }
          

            int isInsert = MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionStringManager, CommandType.Text, sqlcom.CommandText, delcommandParameters);
            if (isInsert != 0)
            {
                PrintToTxt(" 过期产品删除失败");
                marketErrorLoger.log(LogLevel.SYSTEMERROR, " 过期产品删除失败");
            }
        
        }

        private string ReloadData(string url)
        {
            try
            {
                WebRequest wRequest = WebRequest.Create(url);
                WebResponse wResponse = wRequest.GetResponse();
                Stream stream = wResponse.GetResponseStream();
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.Default);
                return reader.ReadToEnd();   //url返回的值  
            }
            catch
            {
                return "";
            }
          
        }

        private string DBValueToStr(MySqlDataReader reader,string key )
        {
            if (reader[key] == DBNull.Value)
            {
                return "";
            }
            else
            {
                return reader[key].ToString();
            }
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
