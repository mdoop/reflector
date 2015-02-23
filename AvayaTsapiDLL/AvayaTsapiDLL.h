// AvayaTsapiDLL.h

#pragma once
#include "stdafx.h"
#include <string>
#include <Acs.h>
#include <Csta.h>
#include <Attpriv.h>
#include "CSTAConfEventInfo.h"
#include "CSTAEventInfo.h"
#define DLLEVENT_EXPORTS __declspec(dllexport)



namespace AvayaTsapiDLL
{

	using namespace std;
	using namespace System;

	ATTPrivateData_t g_privateData;
	Version_t g_szPrivateDataVersion;

	public enum class WorkMode
	{
		INVALID = -2,
		WM_NONE = -1,
		WM_AUX_WORK = 1,
		WM_AFTCAL_WK = 2,
		WM_AUTO_IN = 3,
		WM_MANUAL_IN = 4
	};
	public enum class AgentState
	{
		AM_NOT_READY = 2,
		AM_READY = 3,
		AM_WORK_NOT_READY = 4,
		AM_WORK_READY = 5
	};

	typedef void(CALLBACK * SoftPhoneCallback)(String^ eventMessage);
	typedef void(CALLBACK * UUIDataCallback)(string ucid, string uuiData);

	public ref class AvayaTsapiWrapper
	{
		 
	public:


	public:

		int pollingStatus;
		delegate void CallBackFunction(long invokeID, String^ message);
		delegate void CSTAConfCallback(CSTAConfEventInfo cstaConfEventInfo);
		delegate void CSTAUnsolicitedCallback(CSTAEventInfo^ cstaEventInfo);
		static CSTAConfCallback^ cstaCallback;
		static CallBackFunction^ cb;
		static CSTAUnsolicitedCallback^ cbCSTAUnsolicited;
		//typedef void(CALLBACK * SoftPhoneCallback)(String^ eventMessage);
		AvayaTsapiWrapper();
		~AvayaTsapiWrapper();
		//Methods

		int OpenAESStreamConnection(String^ l_ctiServerId, String^ l_ctiUserName, String^ l_ctiPassword);
		int CloseAESStreamConnection(String^ l_ctiServerId, String^ l_ctiUserName, String^ l_ctiPassword);
		int MonitorExtension(String^ l_agentExtension);
		int StopMonitorExtension(String^ l_agentExtension);
		bool AnswerCall(String^ agentExtension, long callID);
		bool HoldCall(String^ agentExtension, long callID);
		int AgentLogOut(String^ l_agentId, String^ l_agentPassword, String^ l_agentExtension);
		bool ResumeCall(String^ agentExtension, long callID);
		bool DropCall(String^ agentExtension, long callID);
		bool InitiateTransfer(String^ agentExtension, String^ remoteExtension);
		bool CompleteTransfer(String^ agentExtension, String^ remoteExtension);
		bool InitiateConference(String^ agentExtension, String^ remoteExtension);
		bool CompleteConference(String^ agentExtension, String^ remoteExtension);
		int AgentLogin(String^ l_agentId, String^ l_agentPassword, String^ l_agentExtension, WorkMode workMode, int reasonCode);
		bool SetAgentState(String^ l_agentLoginID, String^ l_agentPassword, String^ agentExtension, AgentState agentState, WorkMode workMode, int reasonCode);
		bool MakeCall(String^ agentExtension, String^ destinationExtension);
		void OnCSTAUnsolicited(CSTAEvent_t *cstaEvent);
		void OnCSTAConfirmation(CSTAEvent_t *cstaEvent);
		void OnACSConfirmation(CSTAEvent_t *cstaEvent);
		void PollForEvents(ACSHandle_t handle)
		{
			int pollingStatus = 0;
			CSTAEvent_t cstaEvent;
			RetCode_t retCode;
			unsigned short * bufSize;
			PrivateData_t* privateData;
			unsigned short * numEvents;

			while (pollingStatus == 0)
			{
				//Check for event.
				retCode = acsGetEventPoll(handle, (CSTAEvent_t*)&cstaEvent, bufSize, privateData, numEvents);
				switch (retCode)
				{
				case ACSPOSITIVE_ACK:
					//Do something with the event
					//this->OnSoftPhoneEvent(cstaEvent.eventHeader.eventType.ToString());
				case ACSERR_BADHDL:
					//Log
					break;
				case ACSERR_UBUFSMALL:
					//Log
					break;
				case ACSERR_NOMESSAGE:
					//Log
					break;
				default:
					break;
				}
			}
		}
		void SetEventCallback(CallBackFunction^ func);
		void SetCSTAUnsolicitedCallback(CSTAUnsolicitedCallback^ func);
		void FireEvent(String^ message);
		//Events
	};
}
