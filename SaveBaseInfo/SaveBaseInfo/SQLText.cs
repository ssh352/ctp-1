using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveBaseInfo
{
    public class SQLText
    {
       public static string insertfuturesSQL = "insert into sc_futures (exchange_no,exchange_name,commodity_no,commodity_name,code,"
                        + "contract_no,contract_name,futures_type,product_dot,upper_tick,reg_date,"
                        + "expiry_date,dot_num,currency_no,currency_name,lower_tick,exchange_no2,deposit,"
                        + "deposit_percent,first_notice_day,create_by,create_date,update_by,update_date,commodity_type,py_name )"
                        + "values ("
                        + "@exchange_no,@exchange_name,@commodity_no,@commodity_name,@code,"
                        + "@contract_no,@contract_name,@futures_type,@product_dot,@upper_tick,@reg_date,"
                        + "@expiry_date,@dot_num,@currency_no,@currency_name,@lower_tick,@exchange_no2,@deposit,"
                        + "@deposit_percent,@first_notice_day,@create_by,@create_date,@update_by,@update_date,@commodity_type,@py_name )";

       public static string selectfuturesSQL = "select code from sc_futures where exchange_no = @exchange_no and code = @code";
       public static string deletefuturesSQL = "delete  from sc_futures where exchange_no = @exchange_no and code = @code";
       public static string deleteExpiredfuturesSQL = "delete  from sc_futures where expiry_date < @expiry_date";
       public static string futuresMaxDate = "select max(update_date) from sc_futures";
      
       public static string deleteCurrencySQL = "delete  from sc_currency where currency = @currency_no ";
       public static string insertCurrencySQL = "insert into sc_currency (currency,currency_name,is_primary,exchange,create_by,create_date,update_by,update_date)"
                                               + "values (@currency_no,@currency_name,@is_primary,@exchange,@create_by,@create_date,@update_by,@update_date)";
       public static string selectExchange = "select  `value` as exchange_no , `label` as exchange_name  from  sys_dict where type = 'exchange_no'";

       public static string insertStockSQL = "insert into sc_stock (exchange_no,exchange_name,stock_no,stock_name,"
                   + "stock_type,reg_date,"
                   + "currency_no,currency_name,lot_size,mortgage_percent,upper_tick_code,"
                   + "create_by,create_date,update_by,update_date,commodity_type,py_name )"
                   + "values ("
                   + "@exchange_no,@exchange_name,@stock_no,@stock_name,"
                   + "@stock_type,@reg_date,"
                   + "@currency_no,@currency_name,@lot_size,@mortgage_percent,@upper_tick_code,"
                   + "@create_by,@create_date,@update_by,@update_date,@commodity_type ,@py_name)";

       public static string selectStockSQL = "select stock_no,stock_name,mortgage_percent,upper_tick_code from sc_stock where exchange_no = @exchange_no and stock_no = @stock_no";
       public static string deleteStockSQL = "delete  from sc_stock where exchange_no = @exchange_no and stock_no = @stock_no";
       public static string deleteExpiredStockSQL = "delete  from sc_stock where maturity_date < @maturity_date";
       public static string stockMaxDate = "select max(update_date) from sc_stock";

       public static string selectPlateView = "select plate_group_id,plate_group_name,plate_id,plate_name,exchange_no,commodity_no,commodity_type,group_sort_id,sort_id from sc_plate_view where commodity_type = @commodity_type";
       public static string deletePlateView = "delete from sc_plate_view where plate_group_id = @plate_group_id and plate_id = @plate_id and exchange_no = @exchange_no and commodity_no = @commodity_no and commodity_type = @commodity_type";
       public static string insertPlateViewSQL = "insert into sc_plate_view (plate_group_id,plate_group_name,plate_id,plate_name,"
                 + "exchange_no,commodity_no,"
                 + "commodity_type,group_sort_id,sort_id,"
                 + "create_by,create_date,update_by,update_date )"
                 + "values ("
                 + "@plate_group_id,@plate_group_name,@plate_id,@plate_name,"
                 + "@exchange_no,@commodity_no,"
                 + "@commodity_type,@group_sort_id,@sort_id,"
                 + "@create_by,@create_date,@update_by,@update_date)";
    }
}
