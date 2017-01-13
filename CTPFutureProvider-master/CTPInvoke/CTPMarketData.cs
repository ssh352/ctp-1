using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace CalmBeltFund.Trading.CTP
{
  public class CTPMarketData : CTPFutureClient
  {

    #region Event

    public event EventHandler<CTPEventArgs<CThostFtdcDepthMarketDataField>> DepthMarketDataResponse
    {
      add { AddHandler(CTPResponseType.DepthMarketDataResponse, value); }
      remove { RemoveHandler(CTPResponseType.DepthMarketDataResponse, value); }
    }

    public event EventHandler<CTPEventArgs<CThostFtdcRspAuthenticateField>> AuthenticateResponse
    {
        add { AddHandler(CTPResponseType.AuthenticateResponse, value); }
        remove { RemoveHandler(CTPResponseType.AuthenticateResponse, value); }
    }

    //public event EventHandler<CTPEventArgs<CThostFtdcRspUserLoginField>> UserLoginResponse
    //{
    //    add { AddHandler(CTPResponseType.UserLoginResponse, value); }
    //    remove { RemoveHandler(CTPResponseType.UserLoginResponse, value); }
    //}

    //public event EventHandler<CTPEventArgs> FrontConnectedResponse
    //{
    //    add { AddHandler(CTPResponseType.FrontConnectedResponse, value); }
    //    remove { RemoveHandler(CTPResponseType.FrontConnectedResponse, value); }
    //}

    //public event EventHandler<CTPEventArgs> ErrorResponse
    //{
    //    add { AddHandler(CTPResponseType.ErrorResponse, value); }
    //    remove { RemoveHandler(CTPResponseType.ErrorResponse, value); }
    //}

    //public event EventHandler<CTPEventArgs> FrontDisconnectedResponse
    //{
    //    add { AddHandler(CTPResponseType.FrontDisconnectedResponse, value); }
    //    remove { RemoveHandler(CTPResponseType.FrontDisconnectedResponse, value); }
    //}

    //public event EventHandler<CTPEventArgs> HeartBeatWarningResponse
    //{
    //    add { AddHandler(CTPResponseType.HeartBeatWarningResponse, value); }
    //    remove { RemoveHandler(CTPResponseType.HeartBeatWarningResponse, value); }
    //}

    //public event EventHandler<CTPEventArgs> UserLogoutResponse
    //{
    //    add { AddHandler(CTPResponseType.UserLogoutResponse, value); }
    //    remove { RemoveHandler(CTPResponseType.UserLogoutResponse, value); }
    //}

    #endregion


    public void Connect(string[] frontAddress, string brokerID, string userID, string password, bool restart = true)
    {
      this.BrokerID = brokerID;
      this.InvestorID = userID;
      this.Password = password;

      connTempFile = Path.GetTempFileName();
      //订阅
      int resumeMode = restart ? (int)CTPResumeType.TERT_RESTART : (int)CTPResumeType.TERT_QUICK;

      try
      {
          //创建
          _instance = (IntPtr)Process(Marshal.GetFunctionPointerForDelegate(this.callback), (int)CTPRequestAction.MarketDataCreate, (int)resumeMode, new StringBuilder(connTempFile));

          foreach (string front in frontAddress)
          {
              string address = front;

              if (address.StartsWith("tcp://", StringComparison.OrdinalIgnoreCase) == false)
              {//不是以tcp开头的就认为是域名连接
                  address = "tcp://" + address;
                  //Process(this._instance, (int)CTPRequestAction.MarketDataRegisterNameServer, 0, new StringBuilder(address));
              }
              else
              {
                  
              }
              Process(this._instance, (int)CTPRequestAction.MarketDataRegisterFront, 0, new StringBuilder(address));
              
              this.FrontAddress = address;
          }

          

          Process(this._instance, (int)CTPRequestAction.MarketDataInit, 0, null);
      }
      catch (Exception ex)
      {
      }
    }

    public void UserLogin()
    {
      CThostFtdcReqUserLoginField userLogin = new CThostFtdcReqUserLoginField();

      userLogin.BrokerID = this.BrokerID;
      userLogin.UserID = this.InvestorID;
      userLogin.Password = this.Password;
      userLogin.UserProductInfo = "CalmBelt";
      //Process(this._instance, (int)CTPRequestAction.MarketDataRegisterSpi, 0, null);
      int result = InvokeAPI(CTPRequestAction.MarketDataUserLoginAction, userLogin);
      //int result = InvokeAPI(CTPRequestAction.TraderApiUserLoginAction, userLogin);

    }


    /// <summary>
    /// 订阅行情
    /// </summary>
    /// <param name="symbols"></param>
    public void SubscribeMarketData(string[] symbols)
    {

      IntPtr[] handlers = new IntPtr[symbols.Length];

      for (int i = 0; i < symbols.Length; i++)
      {
        handlers[i] = Marshal.StringToHGlobalAnsi(symbols[i]);
      }

      CTPWrapper.SubscribeMarketData(this._instance, handlers, symbols.Length);

      //StringBuilder buffer = new StringBuilder();

      //foreach (var item in symbols)
      //{
      //  buffer.Append(item).Append('\0');
      //}

      //CTPWrapper.Process(this._instance, (int)CTPRequestAction.MarketDataSubscribeMarketData, symbols.Length, buffer);

    }

    /// <summary>
    /// 退订行情
    /// </summary>
    /// <param name="symbols"></param>
    public void UnSubscribeMarketData(string[] symbols)
    {

      if (symbols == null || symbols.Length == 0)
      {
        return;
      }

      IntPtr[] handlers = new IntPtr[symbols.Length];

      for (int i = 0; i < symbols.Length; i++)
      {
        handlers[i] = Marshal.StringToHGlobalAnsi(symbols[i]);
      }

      CTPWrapper.UnSubscribeMarketData(this._instance, handlers, symbols.Length);
    }

    
    /// <summary>
    /// 最大重连次数
    /// </summary>
    private int reconnectMaxCount = 10;
    /// <summary>
    /// 是否能登陆
    /// </summary>
    public bool canLogin = true;
    /// <summary>
    /// 是否需要登陆
    /// </summary>
    public bool needLogin = false;
    /// <summary>
    /// 已经重连的次数
    /// </summary>
    private int reconnectedCount = 0;
    

    protected override void ProcessBusinessResponse(CTPResponseType responseType, IntPtr pData, CTPResponseInfo rspInfo, int requestID)
    {
        try
        {
            switch (responseType)
            {
                #region 当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
                case CTPResponseType.FrontConnectedResponse:
                    {
                        this.isConnect = true;
                        if (this.isLogin == false)
                        {
                            if (this.canLogin)
                            {
                                this.canLogin = false;
                                this.UserLogin();
                                this.reconnectedCount++;
                                //if (this.reconnectedCount < this.reconnectMaxCount)
                                //{
                                //    this.canLogin = false;
                                //    this.UserLogin();
                                //    this.reconnectedCount++;
                                //}
                            }
                            else
                            {
                                //this.canLogin = true;
                                this.needLogin = true;                                
                            }
                        }

                        //调用事件
                        OnEventHandler(CTPResponseType.FrontConnectedResponse, new CTPEventArgs());

                        break;
                    }
                #endregion

                #region 错误返回
                case CTPResponseType.ErrorResponse:
                    {
                        CTPEventArgs obj = new CTPEventArgs();
                        obj.RequestID = requestID;
                        obj.ResponseInfo = rspInfo;
                        //调用事件
                        OnEventHandler(CTPResponseType.ErrorResponse, obj);
                        break;
                    }
                #endregion

                #region 连接断开返回
                case CTPResponseType.FrontDisconnectedResponse:
                    {
                        this.isConnect = false;
                        this.isLogin = false;
                        this.needLogin = false;
                        //this.canLogin = true;
                        CTPEventArgs obj = new CTPEventArgs();
                        obj.RequestID = requestID;
                        obj.ResponseInfo = rspInfo;
                        //调用事件
                        OnEventHandler(CTPResponseType.FrontDisconnectedResponse, obj);
                        break;
                    }
                #endregion

                #region 心跳警告返回
                case CTPResponseType.HeartBeatWarningResponse:
                    {
                        CTPEventArgs obj = new CTPEventArgs();
                        obj.RequestID = requestID;
                        obj.ResponseInfo = rspInfo;
                        //调用事件
                        OnEventHandler(CTPResponseType.HeartBeatWarningResponse, obj);
                        break;
                    }
                #endregion

                #region 客户端认证响应
                case CTPResponseType.AuthenticateResponse:
                    {
                        CTPEventArgs<CThostFtdcRspAuthenticateField> args = CreateEventArgs<CThostFtdcRspAuthenticateField>(requestID, rspInfo);

                        this.OnEventHandler(CTPResponseType.AuthenticateResponse, args);

                        break;
                    }
                #endregion

                #region 登出返回
                case CTPResponseType.UserLogoutResponse:
                    {
                        this.isLogin = false;
                        CTPEventArgs obj = new CTPEventArgs();
                        obj.RequestID = requestID;
                        obj.ResponseInfo = rspInfo;
                        //调用事件
                        OnEventHandler(CTPResponseType.UserLogoutResponse, obj);
                        break;
                    }
                #endregion

                #region 用户登录
                case CTPResponseType.UserLoginResponse:
                    {
                        CTPEventArgs<CThostFtdcRspUserLoginField> args = CreateEventArgs<CThostFtdcRspUserLoginField>(pData, rspInfo);
                        CThostFtdcRspUserLoginField userLogin = args.Value;


                        if (rspInfo.ErrorID == 0)
                        {
                            this.BrokerID = userLogin.BrokerID;
                            this.FrontID = userLogin.FrontID;
                            this.SessionID = userLogin.SessionID;
                            this.isLogin = true;
                            this.needLogin = false;
                            this.canLogin = true;
                            this.reconnectedCount = 0;
                            //this.loginTime = userLogin.LoginTime;
                        }
                        else
                        {
                            //this.canLogin = true;
                        }
                        this.OnEventHandler(CTPResponseType.UserLoginResponse, args);

                        
                    }
                    break;
                #endregion

                #region 接收到行情数据返回
                case CTPResponseType.DepthMarketDataResponse:
                    {
                        if (this == null || this.isDispose == true)
                        {
                            return;
                        }

                        CTPEventArgs<CThostFtdcDepthMarketDataField> args = CreateEventArgs<CThostFtdcDepthMarketDataField>(pData, rspInfo);

                        OnEventHandler(CTPResponseType.DepthMarketDataResponse, args);

                        break;
                    }
                #endregion

                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            try
            {
                CTPEventArgs obj = new CTPEventArgs();
                obj.RequestID = requestID;
                obj.ResponseInfo = rspInfo;
                obj.ResponseInfo.Message += ex.StackTrace; 
                //调用事件
                OnEventHandler(CTPResponseType.ErrorResponse, obj);
            }
            catch (Exception e)
            {
            }
        }
    }

    #region IDisposable 成员

    public override void Dispose()
    {
      isDispose = true;

      if (this._instance != IntPtr.Zero)
      {
        Process(this._instance, (int)CTPRequestAction.MarketDataRelease, 0, null);

        this._instance = IntPtr.Zero;
      }

      base.Dispose();
    }

    #endregion


    #region Response

    /// <summary>
    /// 订阅行情应答
    /// </summary>
    internal void OnSubMarketData(IntPtr pSpecificInstrument, IntPtr pRspInfo, int nRequestID, bool bIsLast)
    {
      CThostFtdcSpecificInstrumentField instrument = PInvokeUtility.GetObjectFromIntPtr<CThostFtdcSpecificInstrumentField>(pSpecificInstrument);
    }

    /// <summary>
    /// 取消订阅行情应答
    /// </summary>
    internal void OnUnSubMarketData(IntPtr pSpecificInstrument, IntPtr pRspInfo, int nRequestID, bool bIsLast)
    {

    }

    #endregion





  }
}

