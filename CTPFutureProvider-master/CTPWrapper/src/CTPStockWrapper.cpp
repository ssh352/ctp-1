// CTPWrapper.cpp : ���� DLL Ӧ�ó������ڵ㡣
//
#include "..\header\include_CTPStock.h"


extern "C" 
{

  CTPWRAPPER_API int CTPStockProcess(void *instance,CTP_STOCK_REQUEST_TYPE type,int p1,char *p2)
  {

    printf("Process %s","Process");

    switch(type)
    {

    case TraderApiCreate:
      {
        //C#�ص�����
        CTPResponseCallback callback = (CTPResponseCallback)instance;

        //�������׽ӿ�
        CZQThostFtdcTraderApi* pTrader = CZQThostFtdcTraderApi::CreateFtdcTraderApi(p2);
        //�������׻ص�
        CTPStockTraderSpi *spi = new CTPStockTraderSpi(callback);

        //ע��ص�
        pTrader->RegisterSpi((CZQThostFtdcTraderSpi*)spi);

        //������
        pTrader->SubscribePublicTopic((ZQTHOST_TE_RESUME_TYPE)p1);
        pTrader->SubscribePrivateTopic((ZQTHOST_TE_RESUME_TYPE)p1);

        return (int)pTrader;
      }


      ///ɾ���ӿڶ�����
      ///@remark ����ʹ�ñ��ӿڶ���ʱ,���øú���ɾ���ӿڶ���
    case TraderApiRelease:
      {

        CZQThostFtdcTraderApi* pTrader = (CZQThostFtdcTraderApi *)instance;

        pTrader->RegisterSpi(NULL);
        pTrader->Release();

        delete instance;
        instance = NULL;

        return 0;
      }

      ///��ʼ��
      ///@remark ��ʼ�����л���,ֻ�е��ú�,�ӿڲſ�ʼ����
    case  TraderApiInit:
      {
        ((CZQThostFtdcTraderApi *)instance)->Init();
        return 0;
      }

      ///�ȴ��ӿ��߳̽�������
      ///@return �߳��˳�����
    case TraderApiJoin:
      {
        return ((CZQThostFtdcTraderApi *)instance)->Join();
      }

      ///��ȡ��ǰ������
      ///@retrun ��ȡ���Ľ�����
      ///@remark ֻ�е�¼�ɹ���,���ܵõ���ȷ�Ľ�����
    case TraderApiGetTradingDay:
      {
        const char* date = ((CZQThostFtdcTraderApi *)instance)->GetTradingDay();
        memcpy(p2,date,sizeof(date));
        return 0;
      }

      ///ע��ǰ�û������ַ
      ///@param pszFrontAddress��ǰ�û������ַ��
      ///@remark �����ַ�ĸ�ʽΪ����protocol://ipaddress:port�����磺��tcp://127.0.0.1:17001���� 
      ///@remark ��tcp��������Э�飬��127.0.0.1�������������ַ����17001������������˿ںš�
    case TraderApiRegisterFront:
      {
        ((CZQThostFtdcTraderApi *)instance)->RegisterFront(p2);
        return 0;
      }

      ///ע��ǰ�û������ַ
      ///@param pszFrontAddress��ǰ�û������ַ��
      ///@remark �����ַ�ĸ�ʽΪ����protocol://ipaddress:port�����磺��tcp://127.0.0.1:17001���� 
      ///@remark ��tcp��������Э�飬��127.0.0.1�������������ַ����17001������������˿ںš�
    //case TraderApiRegisterNameServer:
    //  {
    //    ((CZQThostFtdcTraderApi *)instance)->RegisterNameServer(p2);
    //    return 0;
    //  }

      ///ע��ص��ӿ�
      ///@param pSpi �����Իص��ӿ����ʵ��
    case TraderApiRegisterSpi:
      {
        ((CZQThostFtdcTraderApi *)instance)->RegisterSpi((CZQThostFtdcTraderSpi*)p1);
        return 0;
      }

      ///����˽������
      ///@param nResumeType ˽�����ش���ʽ  
      ///        THOST_TERT_RESTART:�ӱ������տ�ʼ�ش�
      ///        THOST_TERT_RESUME:���ϴ��յ�������
      ///        THOST_TERT_QUICK:ֻ���͵�¼��˽����������
      ///@remark �÷���Ҫ��Init����ǰ���á����������򲻻��յ�˽���������ݡ�
    case TraderApiSubscribePrivateTopic:
      {
        ((CZQThostFtdcTraderApi *)instance)->SubscribePrivateTopic((ZQTHOST_TE_RESUME_TYPE)p1);
        return 0;
      }

      ///���Ĺ�������
      ///@param nResumeType �������ش���ʽ  
      ///        THOST_TERT_RESTART:�ӱ������տ�ʼ�ش�
      ///        THOST_TERT_RESUME:���ϴ��յ�������
      ///        THOST_TERT_QUICK:ֻ���͵�¼�󹫹���������
      ///@remark �÷���Ҫ��Init����ǰ���á����������򲻻��յ������������ݡ�
    case TraderApiSubscribePublicTopic:
      {
        ((CZQThostFtdcTraderApi *)instance)->SubscribePublicTopic((ZQTHOST_TE_RESUME_TYPE)p1);
        return 0;
      }


    case MarketDataCreate:
      {
        //C#�ص�����
        CTPResponseCallback callback = (CTPResponseCallback)instance;

        //��������ӿ�
        CZQThostFtdcMdApi* pMdApi = CZQThostFtdcMdApi::CreateFtdcMdApi(p2);
        //�������׻ص�
        CTPStockMarketDataSpi *spi = new CTPStockMarketDataSpi(callback);

        //ע��ص�
        pMdApi->RegisterSpi(spi);

        //������
        //pMdApi->SubscribePublicTopic((THOST_TE_RESUME_TYPE)p1);
        //pMdApi->SubscribePrivateTopic((THOST_TE_RESUME_TYPE)p1);

        return (int)pMdApi;
      }

    case MarketDataRelease:
      {
        CZQThostFtdcMdApi* pMarketData = (CZQThostFtdcMdApi *)instance;

        pMarketData->RegisterSpi(NULL);
        pMarketData->Release();

        delete instance;
        instance = NULL;

        return 0;
      }

    case MarketDataInit:
      {
        ((CZQThostFtdcMdApi *)instance)->Init();
        return 0;
      }

    case MarketDataRegisterFront:
      {
        ((CZQThostFtdcMdApi *)instance)->RegisterFront(p2);
        return 0;
      }

    //case MarketDataRegisterNameServer:
    //  {
    //    ((CZQThostFtdcMdApi *)instance)->RegisterNameServer(p2);
    //    return 0;
    //  }
      
       ///��������
    case MarketDataSubscribeMarketData:
      {
        //char** p = (char**)calloc(p1, sizeof(char*));

        //p = (char**)p2;

        //p[0] = *p;

        //for(int i=1;i<p1-1;i++)
        //{
        //  p[i] = p[i-1] + strlen(p[i-1]) + 1;
        //}

        //return ((CZQThostFtdcMdApi*)instance)->SubscribeMarketData(p, p1, p2);
        return -1;
      }

       ///�˶�����
    case MarketDataUnSubscribeMarketData:
      {
        //char** p = (char**)calloc(p1, sizeof(char*));

        ////p = (char**)p2;
        //OutputDebugStringA(p2);
        //p[0] = p2;

        //for(int i=1;i<p1-1;i++)
        //{
        //  p[i] = p[i-1] + strlen(p[i-1]) + 1;
        //}

        //return ((CZQThostFtdcMdApi*)instance)->UnSubscribeMarketData(p, p1, p2);
        return -1;
      }
    }
  }

  ///�û���¼����
  CTPWRAPPER_API int CTPStockProcessRequest(void *instance, CTP_STOCK_REQUEST_TYPE type, void *pReqData, int nRequestID)
  {
    switch(type)
    {
      ///�ͻ�����֤����
      ///��20120828���ӡ�
    case TraderApiAuthenticate:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqAuthenticate((CZQThostFtdcReqAuthenticateField*)pReqData, nRequestID);
      }

      ///�û���¼����
    case TraderApiReqUserLogin:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqUserLogin((CZQThostFtdcReqUserLoginField*)pReqData, nRequestID);
      }


      ///�ǳ�����
    case TraderApiReqUserLogout:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqUserLogout((CZQThostFtdcUserLogoutField*)pReqData, nRequestID);
      }

      ///�û���¼����
    case MarketDataReqUserLogin:
      {
        return ((CZQThostFtdcMdApi*)instance)->ReqUserLogin((CZQThostFtdcReqUserLoginField*)pReqData, nRequestID);
      }


      ///�ǳ�����
    case MarketDataReqUserLogout:
      {
        return ((CZQThostFtdcMdApi*)instance)->ReqUserLogout((CZQThostFtdcUserLogoutField*)pReqData, nRequestID);
      }



      ///�û������������
    case ReqUserPasswordUpdate:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqUserPasswordUpdate((CZQThostFtdcUserPasswordUpdateField*)pReqData, nRequestID);
      }

      ///����¼������
    case ReqOrderInsert:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqOrderInsert((CZQThostFtdcInputOrderField*)pReqData, nRequestID);
      }


      ///������������
    case ReqOrderAction:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqOrderAction((CZQThostFtdcInputOrderActionField*)pReqData, nRequestID);
      }


      ///��ѯ��󱨵���������
    case ReqQueryMaxOrderVolume:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQueryMaxOrderVolume((CZQThostFtdcQueryMaxOrderVolumeField*)pReqData, nRequestID);
      }


      ///�����ѯ����
    case ReqQryOrder:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryOrder((CZQThostFtdcQryOrderField*)pReqData, nRequestID);
      }


      ///�����ѯ�ɽ�
    case ReqQryTrade:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryTrade((CZQThostFtdcQryTradeField*)pReqData, nRequestID);
      }


      ///�����ѯͶ���ֲ߳�
    case ReqQryInvestorPosition:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryInvestorPosition((CZQThostFtdcQryInvestorPositionField*)pReqData, nRequestID);
      }


      ///�����ѯ�ʽ��˻�
    case ReqQryTradingAccount:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryTradingAccount((CZQThostFtdcQryTradingAccountField*)pReqData, nRequestID);
      }


      ///�����ѯͶ����
    case ReqQryInvestor:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryInvestor((CZQThostFtdcQryInvestorField*)pReqData, nRequestID);
      }


      ///�����ѯ���ױ���
    case ReqQryTradingCode:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryTradingCode((CZQThostFtdcQryTradingCodeField*)pReqData, nRequestID);
      }

      ///�����ѯ��Լ��������
    case ReqQryInstrumentCommissionRate:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryInstrumentCommissionRate((CZQThostFtdcQryInstrumentCommissionRateField*)pReqData, nRequestID);
      }


      ///�����ѯ������
    case ReqQryExchange:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryExchange((CZQThostFtdcQryExchangeField*)pReqData, nRequestID);
      }


      ///�����ѯ��Լ
    case ReqQryInstrument:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryInstrument((CZQThostFtdcQryInstrumentField*)pReqData, nRequestID);
      }


      ///�����ѯ����
    case ReqQryDepthMarketData:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryDepthMarketData((CZQThostFtdcQryDepthMarketDataField*)pReqData, nRequestID);
      }

      ///�����ѯͶ���ֲ߳���ϸ
    case ReqQryInvestorPositionDetail:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryInvestorPositionDetail((CZQThostFtdcQryInvestorPositionDetailField*)pReqData, nRequestID);
      }
    }
  };

};
