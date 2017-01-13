using System;
using System.Collections.Generic;
using System.Linq;
using TDFAPI;
using System.Windows.Forms;
using System.Diagnostics;

namespace ChinaStockMarket
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (PrevInstance())
            {
                MessageBox.Show("程序已经启动。", "多重启动", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow());
            }


            //var openSettins = new TDFOpenSetting()
            //{
            //    Ip = System.Configuration.ConfigurationManager.AppSettings["IP"],                                    //服务器Ip
            //    Port = System.Configuration.ConfigurationManager.AppSettings["Port"],                                //服务器端口
            //    Username = System.Configuration.ConfigurationManager.AppSettings["Username"],                        //服务器用户名
            //    Password = System.Configuration.ConfigurationManager.AppSettings["Password"],                        //服务器密码
            //    Subscriptions = System.Configuration.ConfigurationManager.AppSettings["SubScriptions"],              //订阅列表，以 ; 分割的代码列表，例如:if1406.cf;if1403.cf；如果置为空，则全市场订阅
            //    Markets = System.Configuration.ConfigurationManager.AppSettings["Markets"],                          //市场列表，以 ; 分割，例如: sh;sz;cf;shf;czc;dce
            //    ReconnectCount = uint.Parse(System.Configuration.ConfigurationManager.AppSettings["ReconnectCount"]),//当连接断开时重连次数，断开重连在TDFDataSource.Connect成功之后才有效
            //    ReconnectGap = uint.Parse(System.Configuration.ConfigurationManager.AppSettings["ReconnectGap"]),    //重连间隔秒数
            //    ConnectionID = uint.Parse(System.Configuration.ConfigurationManager.AppSettings["ConnectionID"]),    //连接ID，标识某个Open调用，跟回调消息中TDFMSG结构nConnectionID字段相同
            //    Date = uint.Parse(System.Configuration.ConfigurationManager.AppSettings["Date"]),                    //请求的日期，格式YYMMDD，为0则请求今天
            //    Time = (uint)int.Parse(System.Configuration.ConfigurationManager.AppSettings["Time"]),               //请求的时间，格式HHMMSS，为0则请求实时行情，为(uint)-1从头请求
            //    TypeFlags = (uint)int.Parse(System.Configuration.ConfigurationManager.AppSettings["TypeFlags"])      //unchecked((uint)DataTypeFlag.DATA_TYPE_ALL);   //为0请求所有品种，或者取值为DataTypeFlag中多种类别，比如DATA_TYPE_MARKET | DATA_TYPE_TRANSACTION
            //};

            //// 使用者请注意：
            ////  1. 请保持TDFSourceImp的实例到进程销毁。TDFSourceImp是数据接收者
            ////  2. 本Demo只是做最简单演示，在进程（main函数）结束的时候销毁TDFSourceImp实例
            //using (var dataSource = new TDFSourceImp(openSettins))
            //{
            //    #region 调用Open，登陆服务器。初始化过程到此结束，对数据的操作，请到TDFSourceImp的两个虚函数里进行
            //    dataSource.SetEnv(EnvironSetting.TDF_ENVIRON_OUT_LOG, 1);
            //    TDFERRNO nOpenRet = dataSource.Open();
            //    if (nOpenRet == TDFERRNO.TDF_ERR_SUCCESS)
            //    {
            //        Console.WriteLine("connect success!\n");
            //    }
            //    else
            //    {
            //        //这里判断错误代码，进行对应的操作，对于 TDF_ERR_NETWORK_ERROR，用户可以选择重连
            //        Console.WriteLine("open returned:{0}, program quit", nOpenRet);
            //        return;
            //    }
            //    #endregion

            //    var input = Console.ReadLine();

            //}
        }

        private static bool PrevInstance()
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
                string currentPath = Process.GetCurrentProcess().MainModule.FileName;
                if (processes != null && processes.Length > 1)
                {
                    int i = 0;
                    foreach (Process process in processes)
                    {
                        if (process.MainModule.FileName.Equals(currentPath))
                        {
                            i = i + 1;
                            if (i >= 2)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch
            { }


            return false;
        }
    }
}
