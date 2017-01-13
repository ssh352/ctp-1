using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Drawing;
using CommonClassLib;
using System.Threading;
using HPSocketCS;

namespace SaveBaseInfo
{
    public class MarketTcpIp : ClientConnectionBase
    {
        //private ZDLogger errorLogger = null;      

        public MainWindow mainWindow = null;

        public MarketTcpIp(string serverIP, int Port)
        {
            //this.errorLogger = errorLogger;
            this.serverIp = serverIP;
            this.serverPort = (ushort)Port;
            this.reconnectFlag = false;
           // this.reconnectStepTime = 1000;
        }

        /// <summary>
        /// 接受返回数据
        /// </summary>
        /// <param name="obj"></param>
        public override void doOneReceivedData(string str)
        {   
            if (!string.IsNullOrEmpty(str))
            {
                mainWindow.AddMarketBlock(str);
            }
           
        }

        /// <summary>
        /// 连接成功
        /// </summary>
        /// <param name="connId"></param>
        /// <returns></returns>
        public override void ServerConnected()
        {
            //请求登录         
            base.ServerConnected();
            this.isOnReconnecting = true;
            try
            {

                mainWindow.PrintToTxt("交易连接成功。");
               // Thread.Sleep(1000);
                mainWindow.SendLoginInfo();
            }
            catch { }    


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
                mainWindow.PrintToTxt("交易已断开 ServerDisconnected");
              // mainWindow.marketTcpIpBlockingCollection.TryAdd(this);
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
            mainWindow.PrintToTxt("交易已断开 OnError");
           // mainWindow.marketTcpIpBlockingCollection.TryAdd(this);
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
