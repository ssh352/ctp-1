using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveLastPriceForDaFutures
{
    [Serializable]
    public class MyMarketInfo
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        public string exchangeCode { get; set; }
        /// <summary>
        /// 合约代码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 买价
        /// </summary>
        public string buyPrice { get; set; }
        /// <summary>
        /// 买量
        /// </summary>
        public string buyNumber { get; set; }
        /// <summary>
        /// 卖价
        /// </summary>
        public string salePrice { get; set; }
        /// <summary>
        /// 卖量
        /// </summary>
        public string saleNumber { get; set; }
        /// <summary>
        /// 最新价
        /// </summary>
        public string currPrice { get; set; }
        /// <summary>
        /// 现量
        /// </summary>
        public string currNumber { get; set; }
        /// <summary>
        /// 当天最高价
        /// </summary>
        public string high { get; set; }
        /// <summary>
        /// 当天最低价
        /// </summary>
        public string low { get; set; }
        /// <summary>
        /// 开盘价
        /// </summary>
        public string open { get; set; }
        /// <summary>
        /// 昨结算
        /// </summary>
        public string oldClose { get; set; }
        /// <summary>
        /// 当天结算价
        /// </summary>
        public string close { get; set; }
        /// <summary>
        /// 行情时间
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 成交量
        /// </summary>
        public string filledNum { get; set; }
        /// <summary>
        /// 持仓量
        /// </summary>
        public string holdNum { get; set; }
        /// <summary>
        /// 买价2
        /// </summary>
        public string buyPrice2 { get; set; }
        /// <summary>
        /// 买价3
        /// </summary>
        public string buyPrice3 { get; set; }
        /// <summary>
        /// 买价4
        /// </summary>
        public string buyPrice4 { get; set; }
        /// <summary>
        /// 买价5
        /// </summary>
        public string buyPrice5 { get; set; }
        /// <summary>
        /// 买量2
        /// </summary>
        public string buyNumber2 { get; set; }
        /// <summary>
        /// 买量3
        /// </summary>
        public string buyNumber3 { get; set; }
        /// <summary>
        /// 买量4
        /// </summary>
        public string buyNumber4 { get; set; }
        /// <summary>
        /// 买量5
        /// </summary>
        public string buyNumber5 { get; set; }
        /// <summary>
        /// 卖价2
        /// </summary>
        public string salePrice2 { get; set; }
        /// <summary>
        /// 卖价3
        /// </summary>
        public string salePrice3 { get; set; }
        /// <summary>
        /// 卖价4
        /// </summary>
        public string salePrice4 { get; set; }
        /// <summary>
        /// 卖价5
        /// </summary>
        public string salePrice5 { get; set; }
        /// <summary>
        /// 卖量2
        /// </summary>
        public string saleNumber2 { get; set; }
        /// <summary>
        /// 卖量3
        /// </summary>
        public string saleNumber3 { get; set; }
        /// <summary>
        /// 卖量4
        /// </summary>
        public string saleNumber4 { get; set; }
        /// <summary>
        /// 卖量5
        /// </summary>
        public string saleNumber5 { get; set; }

        //201210925 add by dragon
        /// <summary>
        /// 隐藏买价
        /// </summary>
        public string hideBuyPrice { get; set; }
        /// <summary>
        /// 隐藏买量
        /// </summary>
        public string hideBuyNumber { get; set; }
        /// <summary>
        /// 隐藏卖价
        /// </summary>
        public string hideSalePrice { get; set; }
        /// <summary>
        /// 隐藏卖量
        /// </summary>
        public string hideSaleNumber { get; set; }

        /// <summary>
        /// 行情区分
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 跌停价
        /// </summary>
        public string limitDownPrice { get; set; }

        /// <summary>
        /// 涨停价
        /// </summary>
        public string limitUpPrice { get; set; }

        /// <summary>
        /// 交易日
        /// </summary>
        public string tradeDay { get; set; }

        public string buyPrice6 { get; set; }
        public string buyPrice7 { get; set; }
        public string buyPrice8 { get; set; }
        public string buyPrice9 { get; set; }
        public string buyPrice10 { get; set; }
        public string buyNumber6 { get; set; }
        public string buyNumber7 { get; set; }
        public string buyNumber8 { get; set; }
        public string buyNumber9 { get; set; }
        public string buyNumber10 { get; set; }

        public string salePrice6 { get; set; }
        public string salePrice7 { get; set; }
        public string salePrice8 { get; set; }
        public string salePrice9 { get; set; }
        public string salePrice10 { get; set; }
        public string saleNumber6 { get; set; }
        public string saleNumber7 { get; set; }
        public string saleNumber8 { get; set; }
        public string saleNumber9 { get; set; }
        public string saleNumber10 { get; set; }


        /// <summary>
        /// 港交所股票行情：成交类型
        /// </summary>
        public string tradeFlag { get; set; }
        /// <summary>
        /// 交易所数据时间戳
        /// </summary>
        public string dataTimestamp { get; set; }
        /// <summary>
        /// 数据来源。考虑到不同交易所可能有不同数据时间戳格式，可以用该字段确定数据来源
        /// </summary>
        public string dataSourceId { get; set; }
    }
}
