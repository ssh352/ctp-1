using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Drawing;
using CommonClassLib;
using HPSocketCS;
using System.Threading;

namespace SaveLastPriceForDaFutures
{
    public class MarketTcpIp : ClientConnectionBase
    {
        //private ZDLogger errorLogger = null;      

        public MainWindows mainWindow = null;

        public MarketTcpIp( string serverIP, int Port)
        {
            //this.errorLogger = errorLogger;
            this.serverIp = serverIP;
            this.serverPort = (ushort)Port;  
            this.reconnectFlag = false;
           // this.reconnectStepTime = 1000;

        }

        /// <summary>
        /// 接受行情
        /// </summary>
        /// <param name="obj"></param>
        public override void doOneReceivedData(string str)
        {
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    NetInfo ni = new NetInfo();
                    ni.MyReadString(str);
                    if (!string.IsNullOrEmpty(ni.infoT) && ni.code == CommandCode.MARKET01)
                    {
                        if (mainWindow != null)
                        {
                            //将行情收到阻塞线程里面
                            MarketInfo data = new MarketInfo();
                            data.MyReadString(ni.infoT);

                            //if (mainWindow.DaFutures.Equals(mainWindow.marketType))
                            //{
                            //    if ("2".Equals(data.type))
                            //    {
                            //        //if (data.code == "CL1609")
                            //        //{
                            //        //}
                            //        if ("0".Equals(data.currPrice) || "0".Equals(data.currNumber))
                            //        {
                            //            return;
                            //        }
                            //        mainWindow.AddMarketBlock(data);
                            //    }
                            //}
                            //else
                            //{
                            //    if ("0".Equals(data.currPrice) || "0".Equals(data.currNumber))
                            //    {
                            //        return;
                            //    }
                            //    mainWindow.AddMarketBlock(data);
                            //}
                            if (mainWindow.marketBlockingCollection.Count > 500)
                            {
                                Console.WriteLine(mainWindow.marketBlockingCollection.Count);
                                //超过500项先扔掉
                                return;
                            }
                            mainWindow.AddMarketBlock(data);

                        }
                    }
                    else if ((!string.IsNullOrEmpty(ni.infoT) && ni.code == CommandCode.GetSettlePrice))
                    {
                        string temp = ni.infoT;
                        string[] arr = temp.Split('^');
                        for (int i = 0; i < arr.Count(); i++)
                        {
                            string t = arr[i];
                            string[] value = t.Split('@');
                            MarketInfo data = new MarketInfo();
                            data.code = value[0];
                            data.oldClose = value[1];
                            mainWindow.AddMarketBlock(data);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                mainWindow.marketErrorLoger.log(LogLevel.SYSTEMERROR, str + Environment.NewLine +  ex.ToString());
            }
            
           
        }
        /// <summary>
        /// 连接成功
        /// </summary>
        /// <param name="connId"></param>
        /// <returns></returns>
        public override void ServerConnected()
        {
            //添加自己的处理逻辑
            //请求行情         
           
            base.ServerConnected();
            this.isOnReconnecting = true;
            try
            {
                mainWindow.PrintToTxt("行情连接成功。");
            }
            catch { }
            //Thread.Sleep(1000);
            mainWindow.AskMarket();
           

        }

        /// <summary>
        /// 连接断开
        /// </summary>
        /// <param name="connId"></param>
        /// <returns></returns>
        public override void ServerDisconnected()
        {
          
            base.ServerDisconnected();
            this.isOnReconnecting = false;
            try
            {
                mainWindow.PrintToTxt("行情已断开 ServerDisconnected");
                //mainWindow.marketTcpIpBlockingCollection.TryAdd(this);
            }
            catch { }

        }

        /// <summary>
        /// 客户连接出错
        /// </summary>
        /// <param name="connId"></param>
        /// <param name="enOperation"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public override HPSocketCS.HandleResult OnError(TcpClient sender, SocketOperation enOperation, int errorCode)
        {
            //添加自己的处理逻辑
            //this.form.AddMsg("连接出错：connid=" + connId+";errorcode="+errorCode+";operation="+enOperation.ToString());
            this.isOnReconnecting = false;
            mainWindow.PrintToTxt("行情已断开 OnError");
            //mainWindow.marketTcpIpBlockingCollection.TryAdd(this);
            return base.OnError(sender, enOperation, errorCode);

        }

        /// <summary>
        /// 发送数据完成
        /// </summary>
        /// <param name="connId"></param>
        /// <param name="pData"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override HPSocketCS.HandleResult OnSend(TcpClient sender, IntPtr pData, int length)
        {
            //添加自己的处理逻辑

            return base.OnSend(sender, pData, length);
        }
    }
}
