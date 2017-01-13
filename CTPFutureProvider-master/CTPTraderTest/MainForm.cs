using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using CalmBeltFund.Trading.CTP;
using CTPTraderTest.Properties;

namespace CTPTraderTest
{
  public partial class MainForm : Form
  {

    CTPTrader trader = null;
    CTPMarketData quote = null;

    BindingList<CThostFtdcDepthMarketDataField> quoteList = new BindingList<CThostFtdcDepthMarketDataField>();

    public MainForm()
    {
      InitializeComponent();

    }

    void RegisterTraderResponseHandler(CTPTrader trader)
    {
      //注册事件处理函数
      trader.UserLoginResponse += new EventHandler<CTPEventArgs<CThostFtdcRspUserLoginField>>(trader_UserLoginResponse);
      trader.QueryExchangeResponse += new EventHandler<CTPEventArgs<CTPExchange>>(trader_QueryExchangeResponse);
      trader.QueryInstrumentResponse += new EventHandler<CTPEventArgs<List<CTPInstrument>>>(trader_QueryInstrumentResponse);

      trader.QueryInvestorResponse += new EventHandler<CTPEventArgs<CThostFtdcInvestorField>>(trader_QueryInvestorResponse);
      trader.QueryTradingAccountResponse += new EventHandler<CTPEventArgs<CThostFtdcTradingAccountField>>(trader_QueryTradingAccountResponse);
      trader.QueryInvestorPositionResponse += new EventHandler<CTPEventArgs<List<CThostFtdcInvestorPositionField>>>(trader_QueryInvestorPositionResponse);
      trader.QueryInvestorPositionDetailResponse += new EventHandler<CTPEventArgs<List<CThostFtdcInvestorPositionDetailField>>>(trader_QueryInvestorPositionDetailResponse);
      trader.QueryOrderResponse += new EventHandler<CTPEventArgs<List<CThostFtdcOrderField>>>(trader_QueryOrderResponse);
      trader.QueryTradeResponse += new EventHandler<CTPEventArgs<List<CThostFtdcTradeField>>>(trader_QueryTradeResponse);

      trader.OrderInsertResponse += new EventHandler<CTPEventArgs<CThostFtdcInputOrderField>>(trader_OrderInsertResponse);
      trader.OrderActionResponse += new EventHandler<CTPEventArgs<CThostFtdcInputOrderActionField>>(trader_OrderActionResponse);
      trader.ErrorReturnOrderInsertResponse += new EventHandler<CTPEventArgs<CThostFtdcInputOrderField>>(trader_ErrorReturnOrderInsertResponse);
      trader.ReturnOrderResponse += new EventHandler<CTPEventArgs<CThostFtdcOrderField>>(trader_ReturnOrderResponse);
      trader.ReturnTradeResponse += new EventHandler<CTPEventArgs<CThostFtdcTradeField>>(trader_ReturnTradeResponse);

      trader.ReturnInstrumentStatusResponse += new EventHandler<CTPEventArgs<CThostFtdcInstrumentStatusField>>(trader_ReturnInstrumentStatusResponse);
      trader.QueryDepthMarketDataResponse += new EventHandler<CTPEventArgs<CThostFtdcDepthMarketDataField>>(trader_QueryDepthMarketDataResponse);

      trader.SettlementInfoConfirmResponse += new EventHandler<CTPEventArgs<CThostFtdcSettlementInfoConfirmField>>(trader_SettlementInfoConfirmResponse);

    }

    void RegisterQuoteResponseHandler(CTPMarketData quote)
    {
      quote.DepthMarketDataResponse += new EventHandler<CTPEventArgs<CThostFtdcDepthMarketDataField>>(quote_DepthMarketDataResponse);
    }

    void trader_SettlementInfoConfirmResponse(object sender, CTPEventArgs<CThostFtdcSettlementInfoConfirmField> e)
    {
      ShowMessage("初始化完成");
    }

    void quote_DepthMarketDataResponse(object sender, CTPEventArgs<CThostFtdcDepthMarketDataField> e)
    {
      CThostFtdcDepthMarketDataField newValue = e.Value;

      UpdateBindTable(this.idQuoteDataGridView, e.Value, 
        (DataRow row, Object obj) => 
        { 
          CThostFtdcDepthMarketDataField data = (CThostFtdcDepthMarketDataField)obj;

          return (row["InstrumentID"].ToString() == data.InstrumentID);
        });
      

      //this.idQuoteDataGridView.InvalidateRow(index);
    }

    void trader_ReturnTradeResponse(object sender, CTPEventArgs<CThostFtdcTradeField> e)
    {
      PrintObject(e.Value);

       this.Invoke(new Action(() =>
      {
        UpdateBindTable(this.idTradeDataGridView, e.Value);
      }));

    }

    void trader_ReturnOrderResponse(object sender, CTPEventArgs<CThostFtdcOrderField> e)
    {
      PrintObject(e.Value);

      this.Invoke(new Action(() =>
      {
        UpdateBindTable(this.idOrderDataGridView, e.Value, new Func<DataRow, object, bool>(
          (DataRow row, object obj) =>
          {

            CThostFtdcOrderField order = (CThostFtdcOrderField)obj;

            if (row["OrderSysID"] != null && string.IsNullOrWhiteSpace(row["OrderSysID"].ToString()) == false)
            {
              return order.OrderSysID == (string)row["OrderSysID"];
            }
            else if (order.FrontID == (int)row["FrontID"] && order.SessionID == (int)row["SessionID"] && order.OrderRef == (string)row["OrderRef"])
            {
              return true;
            }
            else
            {
              return false;
            }
          }));
      }));
    }

    void trader_OrderActionResponse(object sender, CTPEventArgs<CThostFtdcInputOrderActionField> e)
    {
      Debug.WriteLine(e.ResponseInfo.Message);
      PrintObject(e.Value);
    }

    void trader_OrderInsertResponse(object sender, CTPEventArgs<CThostFtdcInputOrderField> e)
    {
      ShowMessage(e.ResponseInfo.Message);
      PrintObject(e.Value);


    }

    void trader_ErrorReturnOrderInsertResponse(object sender, CTPEventArgs<CThostFtdcInputOrderField> e)
    {
      PrintObject(e.Value);

      this.Invoke(new Action(() =>
      {
        UpdateBindTable(this.idOrderDataGridView, e.Value, new Func<DataRow, object, bool>(
          (DataRow row, object obj) => 
          {

            CThostFtdcInputOrderField order = (CThostFtdcInputOrderField)obj;

            if (order.OrderRef == (string)row["OrderRef"])
            {
              return true;
            }
            else
            {
              return false;
            }
          }));
      }));
    }

    void trader_QueryDepthMarketDataResponse(object sender, CTPEventArgs<CThostFtdcDepthMarketDataField> e)
    {

      PrintObject(e.Value);

      this.Invoke(new Action(() =>
      {
        this.idPriceNumericUpDown.Value = Convert.ToDecimal(e.Value.LastPrice);
      }));
    }

    void trader_QueryInvestorPositionDetailResponse(object sender, CTPEventArgs<List<CThostFtdcInvestorPositionDetailField>> e)
    {
      PrintObjectList(e.Value);
      BindObjectList(this.idPositionDetailDataGridView, e.Value);

      //订阅行情
      List<string> symbols = new List<string>();
      
      //筛选持仓合约
      foreach (var item in e.Value)
      {
        CTPInstrument inst = this.trader.GetInstrument(item.InstrumentID);

        if (inst != null)
        {

          if (symbols.Contains(inst.ID) == false)
          {
            symbols.Add(inst.ID);
          }
        }
      }

      //订阅行情
      this.quote.SubscribeMarketData(symbols.ToArray());
    }

    void trader_ReturnInstrumentStatusResponse(object sender, CTPEventArgs<CThostFtdcInstrumentStatusField> e)
    {
      PrintObject(e.Value,true);
    }

    void trader_QueryTradeResponse(object sender, CTPEventArgs<List<CThostFtdcTradeField>> e)
    {
      PrintObjectList(e.Value);
      BindObjectList(idTradeDataGridView,e.Value);
    }

    void trader_QueryOrderResponse(object sender, CTPEventArgs<List<CThostFtdcOrderField>> e)
    {
      PrintObjectList(e.Value);
      BindObjectList(idOrderDataGridView, e.Value);
    }

    void trader_QueryInvestorPositionResponse(object sender, CTPEventArgs<List<CThostFtdcInvestorPositionField>> e)
    {
      PrintObjectList(e.Value);
      //BindObjectList(idPositionDetailDataGridView, e.Value);
    }

    void trader_QueryInvestorResponse(object sender, CTPEventArgs<CThostFtdcInvestorField> e)
    {
      PrintObject(e.Value, true);
    }

    void trader_QueryTradingAccountResponse(object sender, CTPEventArgs<CThostFtdcTradingAccountField> e)
    {
      PrintObject(e.Value);
      BindObjectList(this.idAmountDataGridView, new List<CThostFtdcTradingAccountField>() { e.Value });
    }

    void trader_QueryExchangeResponse(object sender, CTPEventArgs<CTPExchange> e)
    {
      PrintObject(e.Value);
    }

    void trader_QueryInstrumentResponse(object sender, CTPEventArgs<List<CTPInstrument>> e)
    {
      PrintObjectList(e.Value);
      BindObjectList(this.idSymbolDataGridView,e.Value);

      //添加合约到下拉列表
      this.Invoke(new Action(() =>
        {
          string[] symbols = (from s in e.Value orderby s.ID select s.ID).ToArray();

          this.idSymbolCodeComboBox.Items.Clear();
          this.idSymbolCodeComboBox.Items.AddRange(symbols);

          this.idSymbolCodeComboBox.AutoCompleteCustomSource.Clear();
          this.idSymbolCodeComboBox.AutoCompleteCustomSource.AddRange(symbols);
        }));
    }

    void trader_UserLoginResponse(object sender, CTPEventArgs<CThostFtdcRspUserLoginField> e)
    {
      if (e.ResponseInfo.ErrorID != 0)
      {
        Debug.Write(e.ResponseInfo.Message);
        ShowMessage(e.ResponseInfo.Message);
      }
      PrintObject(e.Value);
    }

    void ShowMessage(string s)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { this.toolStripStatusLabel1.Text = s; }));
      }
      else
      {
        this.toolStripStatusLabel1.Text = s;
      }
    }

    private void Form1_Load(object sender, EventArgs e)
    {

      //买卖
      this.idDirectionComboBox.Items.Add(CTPDirectionType.Buy);
      this.idDirectionComboBox.Items.Add(CTPDirectionType.Sell);
      this.idDirectionComboBox.SelectedIndex = 0;

      //开平
      this.idOffsetComboBox.Items.Add(CTPOffsetFlagType.Open);
      this.idOffsetComboBox.Items.Add(CTPOffsetFlagType.Close);
      this.idOffsetComboBox.Items.Add(CTPOffsetFlagType.CloseToday);
      this.idOffsetComboBox.SelectedIndex = 0;

      //行情
      this.idQuoteDataGridView.AutoGenerateColumns = true;
      this.idQuoteDataGridView.DataSource = CreateObjectFiledTable(new CThostFtdcDepthMarketDataField());

    }

    private void idLoginMenuItem_Click(object sender, EventArgs e)
    {
      LoginForm login = new LoginForm();

      login.TradeServerLogged += new EventHandler(
        (object s, EventArgs args) =>
        {
          //可增加其他监听处理
          //比如多账户：traderList.Add(s as CTPTrader);


          //处理登录成功事件
          ShowMessage("交易账户登录成功，开始初始化信息...");

          CTPTrader trader = s as CTPTrader;

          RegisterTraderResponseHandler(trader);

          trader.QueryInstrument();
          trader.QueryOrder();
          trader.QueryTrade();
          trader.QueryInvestorPositionDetail();
          trader.QueryTradingAccount();
          //确认结算单
          trader.SettlementInfoConfirm();
        });

      if (login.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {

        this.trader = login.Trader;
        this.quote = login.Quote;
       
        RegisterQuoteResponseHandler(this.quote);

      }
    }

    private void idQueryUserMenuItem_Click(object sender, EventArgs e)
    {
      this.trader.QueryInvestor();
    }

    private void idTradeQueryOrderMenuItem_Click(object sender, EventArgs e)
    {
      trader.QueryOrder();
    }

    private void idTradeQueryAmountMenuItem_Click(object sender, EventArgs e)
    {
      trader.QueryTradingAccount();
    }

    private void idTradeQueryPositionMenuItem_Click(object sender, EventArgs e)
    {
      trader.QueryInvestorPosition();
      trader.QueryInvestorPositionDetail();
    }

    private void idSendOrderButton_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrWhiteSpace( idSymbolCodeComboBox.Text) == false)
      {

        string symbolCode = idSymbolCodeComboBox.Text;
        double price = Convert.ToDouble(idPriceNumericUpDown.Value);
        int volume = Convert.ToInt32(this.idVolumeNumericUpDown.Value);
        CTPDirectionType direction = (CTPDirectionType)this.idDirectionComboBox.SelectedItem;
        CTPOffsetFlagType offset = (CTPOffsetFlagType)this.idOffsetComboBox.SelectedItem;

        trader.InsertOrder(symbolCode, price, direction, volume, offset);
      }
    }

    private void idQuerySymbolMenuItem_Click(object sender, EventArgs e)
    {
      this.trader.QueryInstrument();
    }

    private void idTradeQueryTradeMenuItem_Click(object sender, EventArgs e)
    {
      this.trader.QueryTrade();
    }

    private void idOrderDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      e.Cancel = true;
      e.ThrowException = false;
    }

    private void idQueryQuoteButton_Click(object sender, EventArgs e)
    {
      string s = this.idSymbolCodeComboBox.Text;
      if (string.IsNullOrEmpty(s) == false)
      {

        CTPInstrument inst = this.trader.GetInstrument(s);

        if (inst != null)
        {
          this.quote.SubscribeMarketData(new string[] {s});
        }
      }
    }

    private void idCancelButton_Click(object sender, EventArgs e)
    {
      if (this.trader.IsLogin == false)
      {
        ShowMessage("未登录");
        return;
      }


      if (this.idOrderDataGridView.SelectedCells.Count == 0)
      {
        ShowMessage("未选择报单");
        return;
      }

      DataRowView row = this.idOrderDataGridView.SelectedCells[0].OwningRow.DataBoundItem as DataRowView;

      if (row != null)
      {
        string id = row["OrderSysID"].ToString();


        //CTPTrader.OrderList中缓存了当天的全部报单
        CThostFtdcOrderField order = (from item in this.trader.OrderList
                                      where item.OrderSysID == id
                                      select item).FirstOrDefault();

        //TODO：需检查是否未可撤报单
        
        //撤单
        this.trader.DeleteOrder(order);
      }
    }


    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (this.trader != null)
      {
        this.trader.Dispose();
      }

      if (this.quote != null)
      {
        this.quote.Dispose();
      }
    }

  }


}
