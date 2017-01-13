using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CTPTraderTest.Properties;
using CalmBeltFund.Trading.CTP;
using System.Threading;

namespace CTPTraderTest
{
  public partial class LoginForm : Form
  {

    CTPTrader trader = new CTPTrader();
    CTPMarketData quote = new CTPMarketData();

    BackgroundWorker loginWorker = null;

    public CTPTrader Trader
    {
      get { return trader; }
      set { trader = value; }
    }


    public CTPMarketData Quote
    {
      get { return quote; }
      set { quote = value; }
    }

    public bool IsLogin { get; set; }

    /// <summary>
    /// 服务器登录成功事件
    /// </summary>
    public event EventHandler TradeServerLogged;

    public LoginForm()
    {
      InitializeComponent();

      this.idTradeServerAddressTextBox.Text = Settings.Default.TradeServerAddress;
      this.idQuoteServerAddressTextBox.Text = Settings.Default.QuoteServerAddress;
      this.idBrokerTextBox.Text = Settings.Default.BrokerID;
      this.idAccountTextBox.Text = Settings.Default.TradingAccountID;
      this.idPasswordTextBox.Text = Settings.Default.TradingAccountPassword;

    }

    private void idLoginButton_Click(object sender, EventArgs e)
    {

      loginWorker = new BackgroundWorker();
      loginWorker.WorkerSupportsCancellation = true;

      loginWorker.DoWork += new DoWorkEventHandler(new Action<object, DoWorkEventArgs>(
        (object obj, DoWorkEventArgs args) =>
        {

          trader.Connect(new string[] { Settings.Default.TradeServerAddress }, Settings.Default.BrokerID, Settings.Default.TradingAccountID, Settings.Default.TradingAccountPassword);
          quote.Connect(new string[] { Settings.Default.QuoteServerAddress }, Settings.Default.BrokerID, Settings.Default.TradingAccountID, Settings.Default.TradingAccountPassword);

          trader.UserLoginResponse += new EventHandler<CTPEventArgs<CThostFtdcRspUserLoginField>>(trader_UserLoginResponse);

          int timeoutCounter = 0;

          while (true)
          {

            if (trader.IsLogin && quote.IsLogin)
            {
              IsLogin = true;
              ShowMessage("连接成功");
              break;
            }

            //30秒超时设置
            if (timeoutCounter >= 60)
            {
              IsLogin = false;
              ShowMessage("连接超时");
              break;
            }

            if (loginWorker.CancellationPending)
            {
              return;
            }

            ShowMessage("正在连接");
            timeoutCounter++;
            Thread.Sleep(500);
          }


          //关闭登录窗口
          this.BeginInvoke(new Action(() => {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close(); 
          }));
        }));


      //开始登录
      loginWorker.RunWorkerAsync();
    }

    void trader_UserLoginResponse(object sender, CTPEventArgs<CThostFtdcRspUserLoginField> e)
    {
      if (e.ResponseInfo.ErrorID != 0)
      {
        //中止登录
        loginWorker.CancelAsync();

        //显示错误信息
        ShowMessage(e.ResponseInfo.Message);
      }
      else
      {
        //通知交易登录
        OnTradeServerLogged(sender);
      }
    }

    /// <summary>
    /// 引发TradeServerLogged事件
    /// </summary>
    protected void OnTradeServerLogged(object sender)
    {
      if (TradeServerLogged != null)
      {
        TradeServerLogged(sender, EventArgs.Empty);
      }
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
  }
}
