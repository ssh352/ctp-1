// CTPWrapper.cpp : ���� DLL Ӧ�ó������ڵ㡣
//
#include "..\header\stdafx.h"

using namespace std;  

#ifdef _MANAGED
#pragma managed(push, off)
#endif

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
    return TRUE;
}

#ifdef _MANAGED
#pragma managed(pop)
#endif



extern "C" 
{

  CTPWRAPPER_API int Process(void *instance,CTP_REQUEST_TYPE type,int p1,char *p2)
  {

    printf("Process %s","Process");

    switch(type)
    {

    case TraderApiCreate:
      {
        //C#�ص�����
        CTPResponseCallback callback = (CTPResponseCallback)instance;

        //�������׽ӿ�
        CThostFtdcTraderApi* pTrader = CThostFtdcTraderApi::CreateFtdcTraderApi(p2);
        //�������׻ص�
        CTPTraderSpi *spi = new CTPTraderSpi(callback);

        //ע��ص�
        pTrader->RegisterSpi((CThostFtdcTraderSpi*)spi);

        //������
        pTrader->SubscribePublicTopic((THOST_TE_RESUME_TYPE)p1);
        pTrader->SubscribePrivateTopic((THOST_TE_RESUME_TYPE)p1);

        return (int)pTrader;
      }


      ///ɾ���ӿڶ�����
      ///@remark ����ʹ�ñ��ӿڶ���ʱ,���øú���ɾ���ӿڶ���
    case TraderApiRelease:
      {

        CThostFtdcTraderApi* pTrader = (CThostFtdcTraderApi *)instance;

        pTrader->RegisterSpi(NULL);
        pTrader->Release();

        //delete pTrader;
        instance = NULL;

        return 0;
      }

      ///��ʼ��
      ///@remark ��ʼ�����л���,ֻ�е��ú�,�ӿڲſ�ʼ����
    case  TraderApiInit:
      {
        ((CThostFtdcTraderApi *)instance)->Init();
        return 0;
      }

      ///�ȴ��ӿ��߳̽�������
      ///@return �߳��˳�����
    case TraderApiJoin:
      {
        return ((CThostFtdcTraderApi *)instance)->Join();
      }

      ///��ȡ��ǰ������
      ///@retrun ��ȡ���Ľ�����
      ///@remark ֻ�е�¼�ɹ���,���ܵõ���ȷ�Ľ�����
    case TraderApiGetTradingDay:
      {
        const char* date = ((CThostFtdcTraderApi *)instance)->GetTradingDay();
        memcpy(p2,date,sizeof(date));
        return 0;
      }

      ///ע��ǰ�û������ַ
      ///@param pszFrontAddress��ǰ�û������ַ��
      ///@remark �����ַ�ĸ�ʽΪ����protocol://ipaddress:port�����磺��tcp://127.0.0.1:17001���� 
      ///@remark ��tcp��������Э�飬��127.0.0.1�������������ַ����17001������������˿ںš�
    case TraderApiRegisterFront:
      {
        ((CThostFtdcTraderApi *)instance)->RegisterFront(p2);
        return 0;
      }

      ///ע��ǰ�û������ַ
      ///@param pszFrontAddress��ǰ�û������ַ��
      ///@remark �����ַ�ĸ�ʽΪ����protocol://ipaddress:port�����磺��tcp://127.0.0.1:17001���� 
      ///@remark ��tcp��������Э�飬��127.0.0.1�������������ַ����17001������������˿ںš�
    case TraderApiRegisterNameServer:
      {
        ((CThostFtdcTraderApi *)instance)->RegisterNameServer(p2);
        return 0;
      }

      ///ע��ص��ӿ�
      ///@param pSpi �����Իص��ӿ����ʵ��
    case TraderApiRegisterSpi:
      {
        ((CThostFtdcTraderApi *)instance)->RegisterSpi((CThostFtdcTraderSpi*)p1);
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
        ((CThostFtdcTraderApi *)instance)->SubscribePrivateTopic((THOST_TE_RESUME_TYPE)p1);
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
        ((CThostFtdcTraderApi *)instance)->SubscribePublicTopic((THOST_TE_RESUME_TYPE)p1);
        return 0;
      }


    case MarketDataCreate:
      {
        //C#�ص�����
        CTPResponseCallback callback = (CTPResponseCallback)instance;

        //��������ӿ�
        CThostFtdcMdApi* pMdApi = CThostFtdcMdApi::CreateFtdcMdApi(p2);
        //�������׻ص�
        CTPMarketDataSpi *spi = new CTPMarketDataSpi(callback);

        //ע��ص�
        pMdApi->RegisterSpi(spi);
        
        //������
        //pMdApi->SubscribePublicTopic((THOST_TE_RESUME_TYPE)p1);
        //pMdApi->SubscribePrivateTopic((THOST_TE_RESUME_TYPE)p1);

        return (int)pMdApi;
      }

    case MarketDataRelease:
      {
        CThostFtdcMdApi* pMarketData = (CThostFtdcMdApi *)instance;

        pMarketData->RegisterSpi(NULL);
        pMarketData->Release();

        //delete instance;
        instance = NULL;

        return 0;
      }

    case MarketDataInit:
      {
        ((CThostFtdcMdApi *)instance)->Init();
        return 0;
      }

    case MarketDataRegisterFront:
      {
        ((CThostFtdcMdApi *)instance)->RegisterFront(p2);
        return 0;
      }

    case MarketDataRegisterNameServer:
      {
        ((CThostFtdcMdApi *)instance)->RegisterNameServer(p2);
        return 0;
      }
      
       ///��������
    case MarketDataSubscribeMarketData:
      {
        char** p = (char**)calloc(p1, sizeof(char*));

        p = (char**)p2;

        p[0] = *p;

        for(int i=1;i<p1-1;i++)
        {
          p[i] = p[i-1] + strlen(p[i-1]) + 1;
        }

        return ((CThostFtdcMdApi*)instance)->SubscribeMarketData(p, p1);
      }

       ///�˶�����
    case MarketDataUnSubscribeMarketData:
      {
        char** p = (char**)calloc(p1, sizeof(char*));

        

        //p = (char**)p2;
        OutputDebugStringA(p2);
        p[0] = p2;

        for(int i=1;i<p1-1;i++)
        {
          p[i] = p[i-1] + strlen(p[i-1]) + 1;
        }

        return ((CThostFtdcMdApi*)instance)->UnSubscribeMarketData(p, p1);
      }
    }
  }

  ///�û���¼����
  CTPWRAPPER_API int ProcessRequest(void *instance, CTP_REQUEST_TYPE type, void *pReqData, int nRequestID)
  {
    switch(type)
    {
      ///�ͻ�����֤����
      ///��20120828���ӡ�
    case TraderApiAuthenticate:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqAuthenticate((CThostFtdcReqAuthenticateField*)pReqData, nRequestID);
      }

      ///�û���¼����
    case TraderApiReqUserLogin:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqUserLogin((CThostFtdcReqUserLoginField*)pReqData, nRequestID);
      }


      ///�ǳ�����
    case TraderApiReqUserLogout:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqUserLogout((CThostFtdcUserLogoutField*)pReqData, nRequestID);
      }

      ///�û���¼����
    case MarketDataReqUserLogin:
      {
        return ((CThostFtdcMdApi*)instance)->ReqUserLogin((CThostFtdcReqUserLoginField*)pReqData, nRequestID);
      }


      ///�ǳ�����
    case MarketDataReqUserLogout:
      {
        return ((CThostFtdcMdApi*)instance)->ReqUserLogout((CThostFtdcUserLogoutField*)pReqData, nRequestID);
      }



      ///�û������������
    case ReqUserPasswordUpdate:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqUserPasswordUpdate((CThostFtdcUserPasswordUpdateField*)pReqData, nRequestID);
      }


      ///�ʽ��˻������������
    case ReqTradingAccountPasswordUpdate:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqTradingAccountPasswordUpdate((CThostFtdcTradingAccountPasswordUpdateField*)pReqData, nRequestID);
      }


      ///����¼������
    case ReqOrderInsert:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqOrderInsert((CThostFtdcInputOrderField*)pReqData, nRequestID);
      }


      ///Ԥ��¼������
    case ReqParkedOrderInsert:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqParkedOrderInsert((CThostFtdcParkedOrderField*)pReqData, nRequestID);
      }


      ///Ԥ�񳷵�¼������
    case ReqParkedOrderAction:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqParkedOrderAction((CThostFtdcParkedOrderActionField*)pReqData, nRequestID);
      }


      ///������������
    case ReqOrderAction:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqOrderAction((CThostFtdcInputOrderActionField*)pReqData, nRequestID);
      }


      ///��ѯ��󱨵���������
    case ReqQueryMaxOrderVolume:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQueryMaxOrderVolume((CThostFtdcQueryMaxOrderVolumeField*)pReqData, nRequestID);
      }


      ///Ͷ���߽�����ȷ��
    case ReqSettlementInfoConfirm:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqSettlementInfoConfirm((CThostFtdcSettlementInfoConfirmField*)pReqData, nRequestID);
      }


      ///����ɾ��Ԥ��
    case ReqRemoveParkedOrder:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqRemoveParkedOrder((CThostFtdcRemoveParkedOrderField*)pReqData, nRequestID);
      }


      ///����ɾ��Ԥ�񳷵�
    case ReqRemoveParkedOrderAction:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqRemoveParkedOrderAction((CThostFtdcRemoveParkedOrderActionField*)pReqData, nRequestID);
      }


      ///�����ѯ����
    case ReqQryOrder:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryOrder((CThostFtdcQryOrderField*)pReqData, nRequestID);
      }


      ///�����ѯ�ɽ�
    case ReqQryTrade:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryTrade((CThostFtdcQryTradeField*)pReqData, nRequestID);
      }


      ///�����ѯͶ���ֲ߳�
    case ReqQryInvestorPosition:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryInvestorPosition((CThostFtdcQryInvestorPositionField*)pReqData, nRequestID);
      }


      ///�����ѯ�ʽ��˻�
    case ReqQryTradingAccount:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryTradingAccount((CThostFtdcQryTradingAccountField*)pReqData, nRequestID);
      }


      ///�����ѯͶ����
    case ReqQryInvestor:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryInvestor((CThostFtdcQryInvestorField*)pReqData, nRequestID);
      }


      ///�����ѯ���ױ���
    case ReqQryTradingCode:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryTradingCode((CThostFtdcQryTradingCodeField*)pReqData, nRequestID);
      }


      ///�����ѯ��Լ��֤����
    case ReqQryInstrumentMarginRate:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryInstrumentMarginRate((CThostFtdcQryInstrumentMarginRateField*)pReqData, nRequestID);
      }


      ///�����ѯ��Լ��������
    case ReqQryInstrumentCommissionRate:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryInstrumentCommissionRate((CThostFtdcQryInstrumentCommissionRateField*)pReqData, nRequestID);
      }


      ///�����ѯ������
    case ReqQryExchange:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryExchange((CThostFtdcQryExchangeField*)pReqData, nRequestID);
      }


      ///�����ѯ��Լ
    case ReqQryInstrument:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryInstrument((CThostFtdcQryInstrumentField*)pReqData, nRequestID);
      }


      ///�����ѯ����
    case ReqQryDepthMarketData:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryDepthMarketData((CThostFtdcQryDepthMarketDataField*)pReqData, nRequestID);
      }


      ///�����ѯͶ���߽�����
    case ReqQrySettlementInfo:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQrySettlementInfo((CThostFtdcQrySettlementInfoField*)pReqData, nRequestID);
      }


      ///�����ѯת������
    case ReqQryTransferBank:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryTransferBank((CThostFtdcQryTransferBankField*)pReqData, nRequestID);
      }


      ///�����ѯͶ���ֲ߳���ϸ
    case ReqQryInvestorPositionDetail:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryInvestorPositionDetail((CThostFtdcQryInvestorPositionDetailField*)pReqData, nRequestID);
      }


      ///�����ѯ�ͻ�֪ͨ
    case ReqQryNotice:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryNotice((CThostFtdcQryNoticeField*)pReqData, nRequestID);
      }


      ///�����ѯ������Ϣȷ��
    case ReqQrySettlementInfoConfirm:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQrySettlementInfoConfirm((CThostFtdcQrySettlementInfoConfirmField*)pReqData, nRequestID);
      }


      ///�����ѯͶ���ֲ߳���ϸ
    case ReqQryInvestorPositionCombineDetail:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryInvestorPositionCombineDetail((CThostFtdcQryInvestorPositionCombineDetailField*)pReqData, nRequestID);
      }


      ///�����ѯ��֤����ϵͳ���͹�˾�ʽ��˻���Կ
    case ReqQryCFMMCTradingAccountKey:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryCFMMCTradingAccountKey((CThostFtdcQryCFMMCTradingAccountKeyField*)pReqData, nRequestID);
      }

      ///�����ѯ�ֵ��۵���Ϣ
      ///��20120828���ӡ�
    case ReqQryEWarrantOffset:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryEWarrantOffset((CThostFtdcQryEWarrantOffsetField*)pReqData, nRequestID);
      }

      ///�����ѯת����ˮ
    case ReqQryTransferSerial:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryTransferSerial((CThostFtdcQryTransferSerialField*)pReqData, nRequestID);
      }

      ///�����ѯ����ǩԼ��ϵ
      ///��20120828���ӡ�
    case ReqQryAccountregister:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryAccountregister((CThostFtdcQryAccountregisterField*)pReqData, nRequestID);
      }

      ///�����ѯǩԼ����
    case ReqQryContractBank:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryContractBank((CThostFtdcQryContractBankField*)pReqData, nRequestID);
      }


      ///�����ѯԤ��
    case ReqQryParkedOrder:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryParkedOrder((CThostFtdcQryParkedOrderField*)pReqData, nRequestID);
      }


      ///�����ѯԤ�񳷵�
    case ReqQryParkedOrderAction:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryParkedOrderAction((CThostFtdcQryParkedOrderActionField*)pReqData, nRequestID);
      }


      ///�����ѯ����֪ͨ
    case ReqQryTradingNotice:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryTradingNotice((CThostFtdcQryTradingNoticeField*)pReqData, nRequestID);
      }


      ///�����ѯ���͹�˾���ײ���
    case ReqQryBrokerTradingParams:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryBrokerTradingParams((CThostFtdcQryBrokerTradingParamsField*)pReqData, nRequestID);
      }


      ///�����ѯ���͹�˾�����㷨
    case ReqQryBrokerTradingAlgos:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryBrokerTradingAlgos((CThostFtdcQryBrokerTradingAlgosField*)pReqData, nRequestID);
      }


      ///�ڻ����������ʽ�ת�ڻ�����
    case ReqFromBankToFutureByFuture:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqFromBankToFutureByFuture((CThostFtdcReqTransferField*)pReqData, nRequestID);
      }


      ///�ڻ������ڻ��ʽ�ת��������
    case ReqFromFutureToBankByFuture:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqFromFutureToBankByFuture((CThostFtdcReqTransferField*)pReqData, nRequestID);
      }


      ///�ڻ������ѯ�����������
    case ReqQueryBankAccountMoneyByFuture:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQueryBankAccountMoneyByFuture((CThostFtdcReqQueryAccountField*)pReqData, nRequestID);
      }
    }
  };

  typedef void (WINAPI *PTRFUN)(const char*);

  PTRFUN debug_output;

  CTPWRAPPER_API void WINAPI SetOutputCallback(PTRFUN pfun) 
  {
	  debug_output = pfun;
  }

  void OutputLog(const char* msg)
  {
    if(debug_output)
	    debug_output(msg);
  }
  


};
