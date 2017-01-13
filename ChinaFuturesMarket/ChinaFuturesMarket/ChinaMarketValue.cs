using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using TDFAPI;
using System.Threading;
using CommonClassLib;

namespace ChinaStockMarket
{
     class ChinaMarketValue
    {
        //行情阻塞队列
        public static BlockingCollection<object> blkMarketInfo;

       
        // 发送行情线程       
        public static Thread threadMarket;

        // 发送行情线程
        public static bool isStartMarket;

        //行情列表
        public static ConcurrentDictionary<string, MarketInfo> dicMarketInfo;

        //合约列表
        public static ConcurrentDictionary<string, TDFCode> dicTDFCode;

        /// <summary>
        /// 记录错误的日志
        /// </summary>
        public static WriteLog exceptionLog = null;

        /// <summary>
        /// 记录错误的日志
        /// </summary>
        public static int  logLevel = 0;
    }
}
