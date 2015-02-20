// This is the main DLL file.

#include "stdafx.h"
#include "AvayaTsapiDLL.h"
#include "windows.h"



using namespace std;
using namespace AvayaTsapiDLL;


HANDLE	LogFileHandle;
ACSHandle_t* g_acsHandle;
CSTAMonitorCrossRefID_t g_lMonitorCrossRefID;
int g_nOpenStreamInvokeID = 0;
// To store InvokeID for close stream request
int g_nCloseStreamInvokeID = 0;
// To store InvokeID for start monitor device request
int g_nStartMonitorInvokeID = 0;
// To store InvokeID for stop monitor device request
int g_nStopMonitorInvokeID = 0;
long l_callId = 0;
long l_heldCallId = 0;


bool GetPrivateDataFromDeliveredEvent(ATTPrivateData_t privateData)
{
	ATTEvent_t	attEvent;

	if (privateData.length > 0)
	{
		if (attPrivateData((ATTPrivateData_t *)&g_privateData, &attEvent) != ACSPOSITIVE_ACK)
		{
			// we are unable to get private data. still send confirmation
			//WriteToLogFile(__FILE__, __LINE__, 0, "Decoding failed");
		}
		else
		{
			if (attEvent.eventType == ATT_DELIVERED)
			{
				//skill_group_id = attEvent.u.deliveredEvent.split; 
				// universal_call_id = attEvent.u.deliveredEvent.ucid;
			}
		}
	}

	return TRUE;
}
bool GetPrivateDataFromEstablishedEvent()
{
	ATTEvent_t	attEvent;
	string l_ucid, l_uuiData;

	if (g_privateData.length > 0)
	{
		if (attPrivateData((ATTPrivateData_t *)&g_privateData, &attEvent) != ACSPOSITIVE_ACK)
		{
			// we are unable to get private data. still send confirmation
			//WriteToLogFile(__FILE__, __LINE__, 0, "Decoding failed");
		}
		else
		{
			if (attEvent.eventType == ATT_ESTABLISHED)
			{
				l_ucid = attEvent.u.establishedEvent.ucid;
				
				std::string l_uuiData(attEvent.u.establishedEvent.userInfo.data.value,
					attEvent.u.establishedEvent.userInfo.data.value + sizeof(attEvent.u.establishedEvent.userInfo.data.value) / sizeof(attEvent.u.establishedEvent.userInfo.data.value[0]));
				//uuiDataFuncCallback(l_ucid,l_uuiData);
				//FireSoftPhoneUUIEvent(std::wstring(l_ucid.begin(), l_ucid.end()).c_str(), std::wstring(l_uuiData.begin(), l_uuiData.end()).c_str());
				//FireSoftPhoneUUIEvent(String("adfasdf"), String("asdsdfsa"));

				//FireSoftPhoneUUIEvent(string(l_ucid.begin(), l_ucid.end()), string(l_uuiData.begin(), l_uuiData.end()));
				//WriteToLogFile(__FILE__, __LINE__, 0, "UCID is %s", attEvent.u.establishedEvent.ucid);
				//WriteToLogFile(__FILE__, __LINE__, 0, "ATTUserInfo is %s ", attEvent.u.establishedEvent.userInfo.data.value);
			}
		}
	}
	return true;

}
void ProcessDeliveredEvent(CSTAEvent_t *cstaEvent)
{
	l_callId = cstaEvent->event.cstaUnsolicited.u.delivered.connection.callID;
}

void HandleCSTAUnsolicited(CSTAEvent_t *cstaEvent, ATTPrivateData_t privateData)
{
	CSTAEventInfo^ eventInfo;
	eventInfo = gcnew CSTAEventInfo();
	switch (cstaEvent->eventHeader.eventType)
	{
	case CSTA_SERVICE_INITIATED:
	{
		eventInfo = gcnew CSTAEventInfo();
		eventInfo->CalledDevice = gcnew String(cstaEvent->event.cstaUnsolicited.u.serviceInitiated.initiatedConnection.deviceID);
		eventInfo->CallID = cstaEvent->event.cstaUnsolicited.u.serviceInitiated.initiatedConnection.callID;
		
		eventInfo->EventType = "CSTA_SERVICE_INITIATED";

		//WriteToLogFile(__FILE__, __LINE__, 0, "Received CSTA_SERVICE_INITIATED");
		
		//AvayaTsapiDLL::AvayaTsapiWrapper::cb("CSTA_SERVICE_INITIATED");
		AvayaTsapiDLL::AvayaTsapiWrapper::cbCSTAUnsolicited(eventInfo);
		break;
	}
	case CSTA_ORIGINATED:
	{
		//FireEvent("CSTA_ORIGINATED");
		//WriteToLogFile(__FILE__, __LINE__, 0, "Received CSTA_ORIGINATED with callid %lu", cstaEvent->event.cstaUnsolicited.u.originated.originatedConnection.callID);


		//AvayaTsapiDLL::AvayaTsapiWrapper::cb("CSTA_ORIGINATED");
		eventInfo->CalledDevice = gcnew String(cstaEvent->event.cstaUnsolicited.u.originated.calledDevice.deviceID);
		eventInfo->CallingDevice = gcnew String(cstaEvent->event.cstaUnsolicited.u.originated.callingDevice.deviceID);
		eventInfo->CallID = cstaEvent->event.cstaUnsolicited.u.originated.originatedConnection.callID;
		eventInfo->EventType = "CSTA_ORIGINATED";
		AvayaTsapiDLL::AvayaTsapiWrapper::cbCSTAUnsolicited(eventInfo);

		if (l_callId != 0)
		{
			l_heldCallId = l_callId;
		}
		break;
	}
	case CSTA_DELIVERED:
	{
		l_callId = cstaEvent->event.cstaUnsolicited.u.delivered.connection.callID;
		eventInfo->CalledDevice = gcnew String(cstaEvent->event.cstaUnsolicited.u.delivered.calledDevice.deviceID);
		eventInfo->CallingDevice = gcnew String(cstaEvent->event.cstaUnsolicited.u.delivered.callingDevice.deviceID);
		eventInfo->DNIS = gcnew String(cstaEvent->event.cstaUnsolicited.u.delivered.calledDevice.deviceID);
		eventInfo->ANI = gcnew String(cstaEvent->event.cstaUnsolicited.u.delivered.callingDevice.deviceID);
		eventInfo->CallID = cstaEvent->event.cstaUnsolicited.u.delivered.connection.callID;
		eventInfo->EventType = "CSTA_DELIVERED";
		
		ATTEvent_t	attEvent;
		if (attPrivateData((ATTPrivateData_t *)&g_privateData, &attEvent) != ACSPOSITIVE_ACK)
		{
			// we are unable to get private data. still send confirmation
			//WriteToLogFile(__FILE__, __LINE__, 0, "Decoding failed");
		}
		else
		{
			attEvent.u.deliveredEvent.distributingDevice.deviceID;
			eventInfo->UCID = gcnew String(attEvent.u.deliveredEvent.ucid);
			attEvent.u.deliveredEvent.originalCallInfo.userEnteredCode;

			if (attEvent.eventType == ATT_ESTABLISHED)
			{
				string l_ucid;
				l_ucid = attEvent.u.establishedEvent.ucid;
				//String^ l_uuiData = gcnew String(reinterpret_cast<const char*>(attEvent.u.deliveredEvent.userInfo.data.value));
				std::string l_uuiData(attEvent.u.establishedEvent.userInfo.data.value,
					attEvent.u.establishedEvent.userInfo.data.value + sizeof(attEvent.u.establishedEvent.userInfo.data.value) / sizeof(attEvent.u.establishedEvent.userInfo.data.value[0]));
					
				eventInfo->UUIData = gcnew String(l_uuiData.c_str());
			}
			else if (attEvent.eventType = ATT_DELIVERED)
			{
				string l_ucid;
				l_ucid = attEvent.u.establishedEvent.ucid;
				//String^ l_uuiData = gcnew String(reinterpret_cast<const char*>(attEvent.u.deliveredEvent.userInfo.data.value));
				
				std::string l_uuiData(attEvent.u.establishedEvent.userInfo.data.value,
				attEvent.u.establishedEvent.userInfo.data.value + sizeof(attEvent.u.establishedEvent.userInfo.data.value) / sizeof(attEvent.u.establishedEvent.userInfo.data.value[0]));

				Console::WriteLine("Delivered UUI: " + gcnew String(l_uuiData.c_str()));
				eventInfo->UUIData = gcnew String(l_uuiData.c_str());
			}
		}

		

		//WriteToLogFile(__FILE__, __LINE__, 0, "Received CSTA_DELIVERED with callId %lu", cstaEvent->event.cstaUnsolicited.u.delivered.connection.callID);
		//Make the callback to the client with information.

		//AvayaTsapiDLL::AvayaTsapiWrapper::cb("CSTA_DELIVERED");
		AvayaTsapiDLL::AvayaTsapiWrapper::cbCSTAUnsolicited(eventInfo);

		//ProcessDeliveredEvent(cstaEvent);
		//GetPrivateDataFromDeliveredEvent(privateData);
		break;
	}
	case CSTA_ESTABLISHED:
	{
		ATTEvent_t	attEvent;

		eventInfo->CallID = cstaEvent->event.cstaUnsolicited.u.established.establishedConnection.callID;
		eventInfo->CalledDevice = gcnew String(cstaEvent->event.cstaUnsolicited.u.established.calledDevice.deviceID);
		eventInfo->CallingDevice = gcnew String(cstaEvent->event.cstaUnsolicited.u.established.callingDevice.deviceID);
		eventInfo->EventType = "CSTA_ESTABLISHED";
		if (attPrivateData((ATTPrivateData_t *)&g_privateData, &attEvent) != ACSPOSITIVE_ACK)
		{
			// we are unable to get private data. still send confirmation
			//WriteToLogFile(__FILE__, __LINE__, 0, "Decoding failed");
		}
		else
		{
			Console::WriteLine("Established UUI Value:" + (gcnew String(reinterpret_cast<const char*>(attEvent.u.establishedEvent.userInfo.data.value))));
			Console::WriteLine("Established UCID: " + gcnew String(reinterpret_cast<const char*>(attEvent.u.establishedEvent.ucid)));
			eventInfo->UUIData = gcnew String(reinterpret_cast<const char*>(attEvent.u.establishedEvent.userInfo.data.value));
			eventInfo->UCID = gcnew String(reinterpret_cast<const char*>(attEvent.u.establishedEvent.ucid));
			eventInfo->SourceVDN = gcnew String(reinterpret_cast<const char*>(attEvent.u.establishedEvent.lookaheadInfo.sourceVDN));
			eventInfo->TrunkGroup = gcnew String(reinterpret_cast<const char*>(attEvent.u.establishedEvent.trunkGroup));
			eventInfo->TrunkMember = gcnew String(reinterpret_cast<const char*>(attEvent.u.establishedEvent.trunkMember));
			eventInfo->ANI = gcnew String(attEvent.u.establishedEvent.originalCallInfo.callingDevice.deviceID);
		}
		
		if (attEvent.eventType == ATT_ESTABLISHED)
		{
			Console::WriteLine("Established UUI Value:" + (gcnew String(reinterpret_cast<const char*>(attEvent.u.establishedEvent.userInfo.data.value))));
		}

		//WriteToLogFile(__FILE__, __LINE__, 0, "Received CSTA_ESTABLISHED with call id %lu", cstaEvent->event.cstaUnsolicited.u.established.establishedConnection.callID);
		//AvayaTsapiDLL::AvayaTsapiWrapper::cb("CSTA_ESTABLISHED");
		AvayaTsapiDLL::AvayaTsapiWrapper::cbCSTAUnsolicited(eventInfo);

		

		GetPrivateDataFromEstablishedEvent();
		break;
	}
	case CSTA_HELD:
	{
		eventInfo->CallID = cstaEvent->event.cstaUnsolicited.u.held.heldConnection.callID;
		eventInfo->CalledDevice = gcnew String(cstaEvent->event.cstaUnsolicited.u.held.heldConnection.deviceID);
		eventInfo->HeldDeviceID = gcnew String(cstaEvent->event.cstaUnsolicited.u.held.holdingDevice.deviceID);
		eventInfo->EventType = "CSTA_HELD";
		AvayaTsapiDLL::AvayaTsapiWrapper::cbCSTAUnsolicited(eventInfo);
		//AvayaTsapiDLL::AvayaTsapiWrapper::cb("CSTA_HELD");
		break;
	}
	case CSTA_RETRIEVED:
	{
		eventInfo->CallID = cstaEvent->event.cstaUnsolicited.u.retrieved.retrievedConnection.callID;
		eventInfo->DeviceID = gcnew String(cstaEvent->event.cstaUnsolicited.u.retrieved.retrievedConnection.deviceID);
		//FireEvent("CSTA_RETRIEVED");
		//WriteToLogFile(__FILE__, __LINE__, 0, "Received CSTA_RETRIEVED");
		eventInfo->EventType = "CSTA_RETRIEVED";
		AvayaTsapiDLL::AvayaTsapiWrapper::cbCSTAUnsolicited(eventInfo);
		break;
	}
	case CSTA_CALL_CLEARED:
	{
		eventInfo->CallID = cstaEvent->event.cstaUnsolicited.u.callCleared.clearedCall.callID;
		eventInfo->DeviceID = gcnew String(cstaEvent->event.cstaUnsolicited.u.callCleared.clearedCall.deviceID);
		//FireEvent("CSTA_CALL_CLEARED");
		//WriteToLogFile(__FILE__, __LINE__, 0, "Received CSTA_CALL_CLEARED");
		eventInfo->EventType = "CSTA_CALL_CLEARED";
		AvayaTsapiDLL::AvayaTsapiWrapper::cbCSTAUnsolicited(eventInfo);
		//AvayaTsapiDLL::AvayaTsapiWrapper::cb("CSTA_CALL_CLEARED");
		break;
	}
	case CSTA_CONNECTION_CLEARED:
	{
		eventInfo->CallID = cstaEvent->event.cstaUnsolicited.u.connectionCleared.droppedConnection.callID;
		eventInfo->DeviceID = gcnew String(cstaEvent->event.cstaUnsolicited.u.connectionCleared.droppedConnection.deviceID);
		
		//FireEvent("CSTA_CONNECTION_CLEARED");
		//WriteToLogFile(__FILE__, __LINE__, 0, "Received CSTA_CONNECTION_CLEARED");
		eventInfo->EventType = "CSTA_CONNECTION_CLEARED";
		AvayaTsapiDLL::AvayaTsapiWrapper::cbCSTAUnsolicited(eventInfo);
		break;
	}
	case CSTA_CONSULTATION_CALL_CONF:
	{
		//FireEvent("CSTA_CONSULTATION_CALL_CONF");
		//WriteToLogFile(__FILE__, __LINE__, 0, "Received CSTA_CONSULTATION_CALL_CONF");
										AvayaTsapiDLL::AvayaTsapiWrapper::cb(cstaEvent->event.cstaConfirmation.invokeID, "CSTA_CONSULTATION_CALL_CONF");
		break;
	}
	case CSTA_CONSULTATION_CALL:
	{
		//FireEvent("CSTA_CONSULTATION_CALL");
		//WriteToLogFile(__FILE__, __LINE__, 0, "Received CSTA_CONSULTATION_CALL");
								   AvayaTsapiDLL::AvayaTsapiWrapper::cb(cstaEvent->event.cstaConfirmation.invokeID, "CSTA_CONSULTATION_CALL");
		break;
	}
	case CSTA_TRANSFERRED:
	{
		ATTEvent_t	attEvent;
		if (attPrivateData((ATTPrivateData_t *)&g_privateData, &attEvent) != ACSPOSITIVE_ACK)
		{
			// we are unable to get private data. still send confirmation
			//WriteToLogFile(__FILE__, __LINE__, 0, "Decoding failed");
		}
		else
		{
			eventInfo->UCID = gcnew String(reinterpret_cast<const char*>(attEvent.u.transferredEvent.originalCallInfo.ucid));
			eventInfo->TrunkGroup = gcnew String(reinterpret_cast<const char*>(attEvent.u.transferredEvent.originalCallInfo.trunkGroup));
			eventInfo->TrunkMember = gcnew String(attEvent.u.transferredEvent.originalCallInfo.trunkMember);
			eventInfo->CalledDevice = gcnew String(reinterpret_cast<const char*>(attEvent.u.transferredEvent.originalCallInfo.calledDevice.deviceID));
			eventInfo->CallingDevice = gcnew String(reinterpret_cast<const char*>(attEvent.u.transferredEvent.originalCallInfo.callingDevice.deviceID));
			eventInfo->DeviceID = gcnew String(reinterpret_cast<const char*>(attEvent.u.transferredEvent.distributingDevice.deviceID));
			eventInfo->SourceVDN = gcnew String(reinterpret_cast<const char*>(reinterpret_cast<const char*>(attEvent.u.establishedEvent.lookaheadInfo.sourceVDN)));
			eventInfo->TrunkGroup = gcnew String(reinterpret_cast<const char*>(attEvent.u.establishedEvent.trunkGroup));
			eventInfo->TrunkMember = gcnew String(reinterpret_cast<const char*>(attEvent.u.establishedEvent.trunkMember));
			
		}
		
		//FireEvent("CSTA_TRANSFERRED");
		//WriteToLogFile(__FILE__, __LINE__, 0, "Received CSTA_TRANSFERRED");
		eventInfo->CallingDevice = gcnew String(cstaEvent->event.cstaUnsolicited.u.transferred.transferringDevice.deviceID);
		eventInfo->CalledDevice = gcnew String(cstaEvent->event.cstaUnsolicited.u.transferred.transferredDevice.deviceID);
		eventInfo->TransferredDeviceID = gcnew String(cstaEvent->event.cstaUnsolicited.u.transferred.transferredDevice.deviceID);
		eventInfo->TransferringDeviceID = gcnew String(cstaEvent->event.cstaUnsolicited.u.transferred.transferringDevice.deviceID);
		eventInfo->PartyCallID = cstaEvent->event.cstaUnsolicited.u.transferred.transferredConnections.connection->party.callID;
		eventInfo->PrimaryOldCallCallID = cstaEvent->event.cstaUnsolicited.u.transferred.primaryOldCall.callID;
		eventInfo->PrimaryOldCallDeviceID = gcnew String(cstaEvent->event.cstaUnsolicited.u.transferred.primaryOldCall.deviceID);
		eventInfo->SecondaryOldCallCallID = cstaEvent->event.cstaUnsolicited.u.transferred.secondaryOldCall.callID;
		eventInfo->SecondaryOldCallDeviceID = gcnew String(cstaEvent->event.cstaUnsolicited.u.transferred.secondaryOldCall.deviceID);
		eventInfo->EventType = "CSTA_TRANSFERRED";
		
		
		AvayaTsapiDLL::AvayaTsapiWrapper::cbCSTAUnsolicited(eventInfo);
		//AvayaTsapiDLL::AvayaTsapiWrapper::cb("CSTA_TRANSFERRED");
		break;
	}
	case CSTA_CONFERENCE_CALL_CONF:
	{
		eventInfo->ConferencedCallID = cstaEvent->event.cstaConfirmation.u.conferenceCall.newCall.callID;
		//eventInfo->ConferencedDevicedID = gcnew String(cstaEvent->event.cstaConfirmation.u.conferenceCall.newCall.deviceID);
		eventInfo->ConferencedDevicedID = gcnew String(cstaEvent->event.cstaConfirmation.u.conferenceCall.connList.connection[1].party.deviceID);
		eventInfo->EventType = "CSTA_CONFERENCE_CALL_CONF";
		//FireEvent("CSTA_CONFERENCE_CALL_CONF");
		//WriteToLogFile(__FILE__, __LINE__, 0, "Received CSTA_CONFERENCE_CALL_CONF");
		//AvayaTsapiDLL::AvayaTsapiWrapper::cb("CSTA_CONFERENCE_CALL_CONF");
		AvayaTsapiDLL::AvayaTsapiWrapper::cbCSTAUnsolicited(eventInfo);
	}
	case CSTA_CONFERENCED:
	{
		eventInfo->EventType = "CSTA_CONFERENCED";
		eventInfo->CallID = cstaEvent->event.cstaUnsolicited.u.conferenced.primaryOldCall.callID;
		eventInfo->ConferencedDevicedID = gcnew String(cstaEvent->event.cstaUnsolicited.u.conferenced.addedParty.deviceID);
		eventInfo->ConferenceAddedPartyDevice = gcnew String(cstaEvent->event.cstaUnsolicited.u.conferenced.addedParty.deviceID);
		eventInfo->ConferenceControllerDevice = gcnew String(cstaEvent->event.cstaUnsolicited.u.conferenced.confController.deviceID);
		eventInfo->PrimaryOldCallCallID = cstaEvent->event.cstaUnsolicited.u.conferenced.primaryOldCall.callID;
		eventInfo->PrimaryOldCallDeviceID = gcnew String(cstaEvent->event.cstaUnsolicited.u.conferenced.primaryOldCall.deviceID);
		eventInfo->SecondaryOldCallCallID = cstaEvent->event.cstaUnsolicited.u.conferenced.secondaryOldCall.callID;
		eventInfo->SecondaryOldCallDeviceID = gcnew String(cstaEvent->event.cstaUnsolicited.u.conferenced.secondaryOldCall.deviceID);
		
		eventInfo->CallingDevice = gcnew String(cstaEvent->event.cstaUnsolicited.u.conferenced.primaryOldCall.deviceID);

		ATTEvent_t	attEvent;
		if (attPrivateData((ATTPrivateData_t *)&g_privateData, &attEvent) != ACSPOSITIVE_ACK)
		{
			// we are unable to get private data. still send confirmation
			//WriteToLogFile(__FILE__, __LINE__, 0, "Decoding failed");
		}
		else
		{
			eventInfo->UCID = gcnew String(reinterpret_cast<const char*>(attEvent.u.conferencedEvent.ucid));
			eventInfo->TrunkGroup = gcnew String(reinterpret_cast<const char*>(attEvent.u.conferencedEvent.trunkList.trunks[0].trunkGroup));
			eventInfo->TrunkMember = gcnew String(reinterpret_cast<const char*>(attEvent.u.conferencedEvent.trunkList.trunks[0].trunkMember));
			//eventInfo->CalledDevice = gcnew String(reinterpret_cast<const char*>(attEvent.u.conferencedEvent.originalCallInfo.calledDevice.deviceID));
			//eventInfo->CallingDevice = gcnew String(reinterpret_cast<const char*>(attEvent.u.conferencedEvent.originalCallInfo.callingDevice.deviceID));
			eventInfo->DeviceID = gcnew String(reinterpret_cast<const char*>(attEvent.u.conferencedEvent.distributingDevice.deviceID));
		}

		//FireEvent("CSTA_CONFERENCED");
		//WriteToLogFile(__FILE__, __LINE__, 0, "Received CSTA_CONFERENCED");
		AvayaTsapiDLL::AvayaTsapiWrapper::cbCSTAUnsolicited(eventInfo);
		//AvayaTsapiDLL::AvayaTsapiWrapper::cb("CSTA_CONFERENCED");
		break;
	}
	default:
	{
		//WriteToLogFile(__FILE__, __LINE__, 0, "Received unknown event %d", cstaEvent->eventHeader.eventType);
	}

	}

	delete(eventInfo);
}
void HandleCSTAConfirmation(CSTAEvent_t *cstaEvent)
{

	string errorMsg;
	int nError;

	switch (cstaEvent->eventHeader.eventType)
	{
	case CSTA_MONITOR_CONF:
	{
		//WriteToLogFile(__FILE__, __LINE__, 0, "Received CSTA_MONITOR_CONF");
		//FireEvent("CSTA_MONITOR_CONF");
		g_lMonitorCrossRefID =
			cstaEvent->event.cstaConfirmation.u.
			monitorStart.monitorCrossRefID;
		
		AvayaTsapiDLL::AvayaTsapiWrapper::cb(cstaEvent->event.cstaConfirmation.invokeID, "CSTA_MONITOR_CONF");

		break;
	}
	case CSTA_MONITOR_STOP_CONF:
	{
		//WriteToLogFile(__FILE__, __LINE__, 0, "Received CSTA_MONITOR_STOP_CONF");
		//FireEvent("CSTA_MONITOR_STOP_CONF");
								   AvayaTsapiDLL::AvayaTsapiWrapper::cb(cstaEvent->event.cstaConfirmation.invokeID, "CSTA_MONITOR_STOP_CONF");
		break;
	}
	case CSTA_SET_AGENT_STATE_CONF:
	{
		//WriteToLogFile(__FILE__, __LINE__, 0, "Received CSTA_SET_AGENT_STATE_CONF");
		//FireEvent("CSTA_SET_AGENT_STATE_CONF");
		//SetEvent(g_agentloginConfEvent);
									  AvayaTsapiDLL::AvayaTsapiWrapper::cb(cstaEvent->event.cstaConfirmation.invokeID, "CSTA_SET_AGENT_STATE_CONF");
		break;
	}
	case CSTA_UNIVERSAL_FAILURE_CONF:
	{
		nError = cstaEvent->event.cstaConfirmation.u.universalFailure.error;
		//FireEvent("CSTA_UNIVERSAL_FAILURE_CONF");
		//WriteToLogFile(__FILE__, __LINE__, 0, " CSTA_UNIVERSAL_FAILURE_CONF event received.");
		AvayaTsapiDLL::AvayaTsapiWrapper::cb(cstaEvent->event.cstaConfirmation.invokeID, "CSTA_UNIVERSAL_FAILURE_CONF");
		switch (nError)
		{
		case INVALID_CSTA_DEVICE_IDENTIFIER:
		{
			//FireEvent("INVALID_CSTA_DEVICE_IDENTIFIER");
											   AvayaTsapiDLL::AvayaTsapiWrapper::cb(cstaEvent->event.cstaConfirmation.invokeID, "INVALID_CSTA_DEVICE_IDENTIFIER");
			errorMsg = "Error: Invalid CSTA Devie Identifier.";
			//WriteToLogFile(__FILE__, __LINE__, 0, errorMsg.c_str());
			break;
		}
		case RESOURCE_BUSY:
		{
			//FireEvent("RESOURCE_BUSY");
			//WriteToLogFile(__FILE__, __LINE__, 0, " Error: Resource is busy.");
							  AvayaTsapiDLL::AvayaTsapiWrapper::cb(cstaEvent->event.cstaConfirmation.invokeID, "RESOURCE_BUSY");
			break;
		}
		case GENERIC_OPERATION_REJECTION:
		{
			//FireEvent("GENERIC_OPERATION_REJECTION");
			//WriteToLogFile(__FILE__, __LINE__, 0, " Error: GENERIC_OPERATION_REJECTION");
											AvayaTsapiDLL::AvayaTsapiWrapper::cb(cstaEvent->event.cstaConfirmation.invokeID, "GENERIC_OPERATION_REJECTION");
			break;
		}
		case ATT_MAKE_CALL_CONF:
		{
								   AvayaTsapiDLL::AvayaTsapiWrapper::cb(cstaEvent->event.cstaConfirmation.invokeID, "ATT_MAKE_CALL_CONF");
			break;
		}
		default:
		{
			//WriteToLogFile(__FILE__, __LINE__, 0, " Error: CSTA_UNIVERSAL_FAILURE_CONF event received with unknown error code: %d", nError);
		}
		}// end of inner switch
		break;
	}// end of case
	}
}
void HandleACSConfirmation(CSTAEvent_t *cstaEvent, ATTPrivateData_t privateData)
{
	int nError;

	switch (cstaEvent->eventHeader.eventType)
	{
	case ACS_OPEN_STREAM_CONF:
	{
		if (g_nOpenStreamInvokeID == cstaEvent->event.acsConfirmation.invokeID)
		{
			//WriteToLogFile(__FILE__, __LINE__, 0, " acsOpenStremConfEvent received - Stream opened successfully. API Version: %d", cstaEvent->event.acsConfirmation.u.acsopen.apiVer);


			//FireEvent("ACS_OPEN_STREAM_CONF");
			//OnSoftPhoneEvent("ACS_OPEN_STREAM_CONF");
			AvayaTsapiDLL::AvayaTsapiWrapper::cb(cstaEvent->event.cstaConfirmation.invokeID, "ACS_OPEN_STREAM_CONF");
			if (g_privateData.length <= 0)
			{
				//WriteToLogFile(__FILE__, __LINE__, 0, " Private Data length is zero in acsOpenStreamConf event. Private data is not sent as a part of this event.");
				//PrintErrorAndExit(a_pAcsHandle);
			}
			else
			{
				//WriteToLogFile(__FILE__, __LINE__, 0, " Private Data length is not zero in acsOpenStreamConf event. Private data is sent as a part of this event.");
			}

			if (strcmp(g_privateData.vendor, ECS_VENDOR_STRING) != 0)
			{
				//WriteToLogFile(__FILE__, __LINE__, 0, "hanlde error condtion ( abort the Stream )");
				// return error
			}

			// 3rd check the One byte descriminator
			if (g_privateData.data[0] != PRIVATE_DATA_ENCODING)
			{
				//WriteToLogFile(__FILE__, __LINE__, 0, "handle error condtion ( abort the Stream )");
				// return error
			}
			else
			{
				// Retrieving the Private Data 
				//WriteToLogFile(__FILE__, __LINE__, 0, " PrivateData = VENDOR: %s ", g_privateData.vendor);

				char cPDVReturned = g_privateData.data[1];

				// To hold returned PDV as number
				int nReturnedPDV = atoi(&cPDVReturned);

				if (strchr(g_szPrivateDataVersion, '-') == NULL)
				{
					// Requested version is specific i.e. does not contain '-'
					int nRequestedPDV = atoi(g_szPrivateDataVersion);

					if (nRequestedPDV == nReturnedPDV)
					{
						//WriteToLogFile(__FILE__, __LINE__, 0, " Private data version negotiation is Negotiated private data version is: %d", cPDVReturned);
					}
					else
					{
						//WriteToLogFile(__FILE__, __LINE__, 0, " Private data version negotiation is failed.");
					}
				}
				else
				{
					char* szFirst = _strdup(g_szPrivateDataVersion);
					// To store second part of requested PDV
					char* szSecond = "";

					strtok_s(szFirst, "-", &szSecond);

					int nMinVersion = atoi(szFirst);
					int nMaxVersion = atoi(szSecond);

					if (nReturnedPDV >= nMinVersion
						&&
						nReturnedPDV <= nMaxVersion
						)
					{
						//WriteToLogFile(__FILE__, __LINE__, 0, " Private data version negotiation is Negotiated private data version is: %d ", cPDVReturned);
					}
					else
					{
						//WriteToLogFile(__FILE__, __LINE__, 0, " Private data version negotiation is failed.");
					}
				}
			}
			// Sets event object to signaled state.
			//SetEvent(g_hOpenStreamConfEvent);
		}
		else
		{
			//WriteToLogFile(__FILE__, __LINE__, 0, " A confirmation event received for an unknown open  stream request.");
		}
		break;
	}
	case ACS_CLOSE_STREAM_CONF:
	{
		if (g_nCloseStreamInvokeID == cstaEvent->event.acsConfirmation.invokeID)
		{
			//FireEvent("ACS_CLOSE_STREAM_CONF");
			//SetEvent(g_hCloseStreamConfEvent);
			AvayaTsapiDLL::AvayaTsapiWrapper::cb(cstaEvent->event.cstaConfirmation.invokeID, "ACS_CLOSE_STREAM_CONF");
		}
		else
		{
			//WriteToLogFile(__FILE__, __LINE__, 0, " A confirmation event received for an unknown close stream request.");
		}

		break;
	}
	case ACS_UNIVERSAL_FAILURE_CONF:
	{
		// Checking for the Failure of Open Stream request
		nError = cstaEvent->event.acsConfirmation.u.failureEvent.error;
		//WriteToLogFile(__FILE__, __LINE__, 0, " ACS_UNIVERSAL_FAILURE_CONF event received");
		// Verifying error is for open stream that this
		// application has opened 
		if (g_nOpenStreamInvokeID == cstaEvent->event.acsConfirmation.invokeID)
		{
			// Checking for the password of the loginID
			//FireEvent("ACS_UNIVERSAL_FAILURE_CONF");
			AvayaTsapiDLL::AvayaTsapiWrapper::cb(cstaEvent->event.cstaConfirmation.invokeID, "ACS_UNIVERSAL_FAILURE_CONF");
			switch (nError)
			{
			case TSERVER_BAD_PASSWORD_OR_LOGIN:
			{
				//FireEvent("TSERVER_BAD_PASSWORD_OR_LOGIN");
												  AvayaTsapiDLL::AvayaTsapiWrapper::cb(cstaEvent->event.cstaConfirmation.invokeID, "TSERVER_BAD_PASSWORD_OR_LOGIN");
				//WriteToLogFile(__FILE__, __LINE__, 0, " CTI login password is incorrect");
				break;
			}
			case TSERVER_NO_USER_RECORD:
			{
				//FireEvent("TSERVER_NO_USER_RECORD");
										   AvayaTsapiDLL::AvayaTsapiWrapper::cb(cstaEvent->event.cstaConfirmation.invokeID, "TSERVER_NO_USER_RECORD");
				//WriteToLogFile(__FILE__, __LINE__, 0, " No user object was found in the security database for the login specified in the ACSOpenStream request.");
				break;
			}
			default:
			{
				//WriteToLogFile(__FILE__, __LINE__, 0, " ACS_UNIVERSAL_FAILURE_CONF event received with unknown error %d", nError);
			}
			}
		}
		else if (g_nStartMonitorInvokeID == cstaEvent->event.cstaConfirmation.invokeID)
		{
			// Checking for the password of the loginID
			switch (nError)
			{
			case TSERVER_DEVICE_NOT_SUPPORTED:
			{
				//FireEvent("TSERVER_DEVICE_NOT_SUPPORTED");
												 AvayaTsapiDLL::AvayaTsapiWrapper::cb(cstaEvent->event.cstaConfirmation.invokeID, "TSERVER_DEVICE_NOT_SUPPORTED");
				//WriteToLogFile(__FILE__, __LINE__, 0, " Error: Device not supported.");
				break;
			}
			default:
			{
				//WriteToLogFile(__FILE__, __LINE__, 0, " ACS_UNIVERSAL_FAILURE_CONF event received with unknown error %d Error Code: ", nError);
			}
			}
		}
		else
		{
			//WriteToLogFile(__FILE__, __LINE__, 0, " An ACS_UNIVERSAL_FAILURE_CONF event received for an unknown request %d", nError);
		}
		break;
	}
	}
}
void AvayaEventsNotifyHandler(ACSHandle_t* a_pAcsHandle)
{
	
	bool isEventRetrived = false;
	const int APP_DEF_DEFAULT_BUFFER_SIZE = 10000;
	unsigned short usEventBufSize = APP_DEF_DEFAULT_BUFFER_SIZE;
	CSTAEvent_t *cstaEvent = NULL;
	unsigned short usNumEvents = 1;
	string		errorMsg;

	while (!isEventRetrived || (usNumEvents > 0 && usEventBufSize > 0))
	{
		int nError;

		//ATTPrivateData_t privateData;
		g_privateData.length = ATT_MAX_PRIVATE_DATA;

		if (NULL != cstaEvent)
		{
			//Free the buffer memory
			free(cstaEvent);
		}

		cstaEvent = (CSTAEvent_t*)malloc((SIZE_T)usEventBufSize);

		RetCode_t nRetCode;

		nRetCode = acsGetEventPoll(*a_pAcsHandle,
			(void *)cstaEvent,
			&usEventBufSize,
			(PrivateData_t *)&g_privateData,
			&usNumEvents);

		if (nRetCode != ACSPOSITIVE_ACK)
		{
			if (nRetCode == ACSERR_BADHDL)
			{
				//WriteToLogFile(__FILE__, __LINE__, 0, " The ACS Handle is invalid");
			} // end of if 
			else if (nRetCode == ACSERR_UBUFSMALL)
			{
				//WriteToLogFile(__FILE__, __LINE__, 0, " Passed event buffer size is smaller than the size of the next available event for this ACS Stream.");
				continue;
			}// end of else if	
			else if (nRetCode == ACSERR_NOMESSAGE)
			{
				// The acsGetEventPoll()method return this value to indicate
				// there were no events available in the Client library queue.
				//WriteToLogFile(__FILE__, __LINE__, 0, " No events available at this time.");
			}
			else
			{
				//WriteToLogFile(__FILE__, __LINE__, 0, " acsGetEventBlock()/acsGetEventPoll() failed with unknown error. ");
				break;
			}
		}// end of if 
		else
		{
			isEventRetrived = true;
			switch (cstaEvent->eventHeader.eventClass)
			{
			case ACSCONFIRMATION:
			{
				HandleACSConfirmation(cstaEvent, g_privateData);
			}
			case CSTACONFIRMATION:
			{
				HandleCSTAConfirmation(cstaEvent);
			}
			case CSTAUNSOLICITED:
			{
				HandleCSTAUnsolicited(cstaEvent, g_privateData);
			}
			}
		}
	}

	return;
}
void __stdcall AvayaEventsCallbackHandler(unsigned long esrParam)
{
	g_acsHandle = (ACSHandle_t*)esrParam;
	
	// As the event is now available in the Client library's queue,
	// other applications should be designed cautiously to retrieve
	// the event and process it without blocking this thread for a
	// long time. Writing event processing logic (specially when the 
	// logic is lengthy) in a seperate thread is recommended.

	// In this example as the event processing logic is simple and not
	// lengthy, we are calling method directly from this thread only. 
	AvayaEventsNotifyHandler((ACSHandle_t*)g_acsHandle);

}


int ConvertToCHARBuffer(char *str, int size, const char *format, ...)
{
	int     ret_val;
	char    err_str[1024];
	va_list arg_ptr;
	BOOL    buffer_overflow = FALSE;
	

	va_start(arg_ptr, format);
	ret_val = _vsnprintf(str, size - 1, format, arg_ptr);
	if (ret_val == -1)
	{
		buffer_overflow = TRUE;
		ret_val = size - 1;
	}
	str[ret_val] = '\0';
	va_end(arg_ptr);

	if (buffer_overflow == TRUE)
	{
		/*
		* Here, the formatted string should never exceed sizeof(err_str).
		* Otherwise, we will run into an stack overflow problem during recursion.
		*/
		//ConvertToCHARBuffer(err_str, sizeof(err_str), "Buffer overflow while formatting string: input size = %d", size);
		//LOG4CPLUS_ERROR(logger,err_str);
		//LOG4CPLUS_ERROR(logger,str);
	}
	return ret_val;
}

/*bool WriteToLogFile(const char *file_name, int line_no, int log_level, const char *format)
{
DWORD		dwNumRead;
SYSTEMTIME	SystemTime;
char		CurrentTime[1024];
char		OutputLogMessage[10000];
char		msg_str[1024];
va_list		arg_ptr;
BOOL		buffer_overflow = FALSE;
int			ret_val;

va_start(arg_ptr, format);
ret_val = _vsnprintf(msg_str, sizeof(msg_str)-1, format, arg_ptr);
if (ret_val == -1)
{
buffer_overflow = TRUE;
ret_val = sizeof(msg_str)-1;
}
msg_str[ret_val] = '\0';
va_end(arg_ptr);

GetLocalTime(&SystemTime);
ConvertToCHARBuffer(CurrentTime, sizeof(CurrentTime), "%04d-%02d-%02d %02d:%02d:%02d", SystemTime.wYear, SystemTime.wMonth, SystemTime.wDay, SystemTime.wHour, SystemTime.wMinute, SystemTime.wHour);
ConvertToCHARBuffer(OutputLogMessage, sizeof(OutputLogMessage), "%s %s %d  %s \r\n", CurrentTime, file_name, line_no, msg_str);
String outputLogMessage;
outputLogMessage = "";

if (LogFileHandle != INVALID_HANDLE_VALUE)
{
WriteFile(LogFileHandle, OutputLogMessage, strlen(OutputLogMessage) * sizeof(char), &dwNumRead, NULL);
}

return true;
}*/
bool WriteToLogFile(string file_name, int line_no, int log_level, string message)
{
	//string currentTime;
	//DWORD dwNumRead;
	//LPCVOID buffMess;
	//message.Insert(0, "%04d-%02d-%02d %02d:%02d:%02d");
	//buffMess = string::c_str(message.ToCharArray());
	//WriteFile(LogFileHandle, message, message.Length, &dwNumRead, NULL);
	return true;
}


extern "C"
{


	AvayaTsapiDLL::AvayaTsapiWrapper::AvayaTsapiWrapper()
	{

	}

	AvayaTsapiDLL::AvayaTsapiWrapper::~AvayaTsapiWrapper()
	{

	}

	long AvayaTsapiDLL::AvayaTsapiWrapper::OpenAESStreamConnection(String^ l_ctiServerId, String^ l_ctiUserName, String^ l_ctiPassword)
	{
		ACSHandle_t             *acsHandle = new ACSHandle_t;
		InvokeIDType_t          invokeIDType;
		InvokeID_t              invokeID;
		StreamType_t            streamType;
		ServerID_t              serverID;
		LoginID_t               loginID;
		Passwd_t                passwd;
		AppName_t               applicationName;
		Level_t                 acsLevelReq;
		Version_t               apiver;
		ATTPrivateData_t		privateData;
		string					apiVersion;
		RetCode_t               rCode;
		char                    msg_str[500];
		string 					errorMsg;

		//CStringA serverString(l_ctiServerId);
		//CStringA userString(l_ctiUserName);
		//CStringA passString(l_ctiPassword);
		//const char* pszServer = l_ctiServerId;
		//const char* pszUser = l_ctiUserName;
		//const char* pszPass = l_ctiPassword;
		sprintf(serverID, "%s", l_ctiServerId);
		sprintf(loginID, "%s", l_ctiUserName);
		sprintf(passwd, "%s", l_ctiPassword);

		apiVersion = "6";

		strcpy(g_privateData.vendor, "VERSION");
		ConvertToCHARBuffer(g_szPrivateDataVersion, sizeof(g_szPrivateDataVersion), "1-6");
		g_privateData.data[0] = PRIVATE_DATA_ENCODING;
		if (attMakeVersionString(g_szPrivateDataVersion, &g_privateData.data[1]) > 0)
		{
			g_privateData.length = (unsigned short)strlen(&g_privateData.data[1]) + 2;
		}
		else
		{
			g_privateData.length = 0;
		}

		invokeIDType = LIB_GEN_ID;
		//sprintf(serverID,"%s",l_ctiServerId);
		//sprintf(loginID,"%s",l_ctiUserName);
		//sprintf(passwd,"%s",l_ctiPassword);

		ConvertToCHARBuffer(applicationName, sizeof(applicationName), "Softphone");
		sprintf(apiver, "%s", apiVersion.c_str());

		try
		{
			rCode = acsOpenStream(acsHandle, LIB_GEN_ID, 0, ST_CSTA, &serverID, &loginID, &passwd, &applicationName, ACS_LEVEL1, (Version_t *) "TS1-2", 10000, 0, 10000, 0, (PrivateData_t *)&g_privateData);
		}
		catch (exception ex)
		{
			rCode = -44;
		}
		if (rCode < 0)
		{
			WriteToLogFile(__FILE__, __LINE__, 0, "ACSOpenStream failed with reason");

			switch (rCode)
			{
			case ACSPOSITIVE_ACK:
			{
				break;
			}
			case ACSERR_APIVERDENIED:
			{	// This return value indicates that the API Version requested is
				// invalid and not supported by the existing API Client Library.
				errorMsg = " Error: acsOpenStream method failed to open stream.. Error: API Version is incorrect. Try again..";
				//WriteToLogFile(__FILE__, __LINE__, 0, "%s", errorMsg);
				//FireEvent("ACSERR_APIVERDENIED");
				//OnSoftPhoneEvent("ACSERR_APIVERDENIED");
				FireEvent("ACSERR_APIVERDENIED");
				break;
			}
			case ACSERR_BADPARAMETER:
			{	// One or more parameters invalid.
				// Validate supplied parameter with the help of 
				// TSAPI Exerciser tool.
				errorMsg = " Error: acsOpenStream method failed to open stream.. Error: bad parameter. Try again..";
				//OnSoftPhoneEvent("ACSERR_BADPARAMETER");
				FireEvent("ACSERR_BADPARAMETER");
				//WriteToLogFile(__FILE__, __LINE__, 0, "%s", errorMsg);
				break;
			}
			default:
			{
				// Some unhandled error occured
				const int SLEEP_TIME = 3000;
				errorMsg = " Error: acsOpenStream method failed to open stream.. Try again..";
				//WriteToLogFile(__FILE__, __LINE__, 0, "%s", errorMsg);
				Sleep(SLEEP_TIME);
				return false;
			}
			}

			return (long)rCode;
		}
		else
		{
			//WriteToLogFile(__FILE__, __LINE__, 0, "ACSOpenStream Success");
			g_nOpenStreamInvokeID = (int)rCode;
			//handler = AvayaEventsCallbackHandler;

			rCode = acsSetESR(*acsHandle, AvayaEventsCallbackHandler, (unsigned long)acsHandle, TRUE);
			//this->PollForEvents(*acsHandle);
			// Verification for the positive response
			if (rCode != ACSPOSITIVE_ACK)
			{
				//WriteToLogFile(__FILE__, __LINE__, 0, " ERROR: acsSetESR() method return with an error.");

				if (rCode == ACSERR_BADHDL)
				{
					//WriteToLogFile(__FILE__, __LINE__, 0, " ulAcsHandle being used is not a valid handle");
				}
				else
				{
					//WriteToLogFile(__FILE__, __LINE__, 0, " acsSetESR() failed with unknown error. ");
					//WriteToLogFile(__FILE__, __LINE__, 0, " Error code: %d", rCode);
				}
			}
			else
			{
				//this->OnSoftPhoneEvent("ACSPOSITIVE_ACK");
				//OnSoftPhoneEvent("ACSPOSITIVE_ACK");
				//this->cb("ACSPOSITIVE_ACK");
			}
		}

		return (long)g_nOpenStreamInvokeID;
	}

	bool AvayaTsapiDLL::AvayaTsapiWrapper::CloseAESStreamConnection(String^ l_ctiServerId, String^ l_ctiUserName, String^ l_ctiPassword)
	{
		RetCode_t m_nRetCode;
		if (g_acsHandle != NULL)
		{
			//m_nRetCode = acsCloseStream(*g_acsHandle, 0, NULL);
			m_nRetCode = acsAbortStream(*g_acsHandle, NULL);
			if (m_nRetCode >= 0)
			{
				
				return TRUE;
			}
			else
			{
				//SendMessage(WM_SPEVENT, 0, ACS_UNIVERSAL_FAILURE);
				return FALSE;
			}
		}
		else
		{
			return FALSE;
		}

		//FireEvent("COOL!");
		return true;
	}

	int AvayaTsapiDLL::AvayaTsapiWrapper::AgentLogin(String^ l_agentId, String^ l_agentPassword, String^ l_agentExtension, WorkMode workMode, int reasonCode)
	{
		RetCode_t               m_nRetCode;
		//int						workMode = 3;

		DeviceID_t device_id;
		AgentID_t	agent_id;
		AgentMode_t	agent_mode;
		AgentGroup_t agent_group;
		AgentPassword_t agent_password;


		/*CStringA extensionString(l_agentExtension);
		CStringA userString(l_agentId);
		CStringA passString(l_agentPassword);
		const char* pszExtension = extensionString;
		const char* pszUser = userString;
		const char* pszPass = passString;*/
		sprintf(device_id, "%s", l_agentExtension);
		sprintf(agent_id, "%s", l_agentId);
		sprintf(agent_password, "%s", l_agentPassword);

		//ConvertToCHARBuffer(device_id,sizeof(device_id),"%s",l_agentExtension);
		//ConvertToCHARBuffer(agent_id,sizeof(agent_id),"%s",l_agentId);
		//ConvertToCHARBuffer(agent_group,sizeof(agent_group),"");
		//ConvertToCHARBuffer(agent_password,sizeof(agent_password),"%s",l_agentPassword);

		bool bResult = FALSE;
		if (workMode == AvayaTsapiDLL::WorkMode::WM_AUTO_IN)
		{	// work mode used for logging an Agent with workMode Auto In
			m_nRetCode = attV6SetAgentState(&g_privateData   // Private Data Buffer is prepared here
				, ATTWorkMode_t::WM_AUTO_IN	       // Work Mode for the Agent, (Auto In)
				, 0				   // Reason, code, this parameter is ignored 
				// agentMode is AM_LOG_IN				
				, FALSE);		   // EnablePending flag, by default value is FALSE;
		}
		else if (workMode == AvayaTsapiDLL::WorkMode::WM_MANUAL_IN)
		{	// work mode used for logging an Agent with workMode Manual
			m_nRetCode = attV6SetAgentState(&g_privateData
				, ATTWorkMode_t::WM_MANUAL_IN	// Manual In Work Mode
				, 0
				, TRUE);
		}
		else
		{
			m_nRetCode = attV6SetAgentState(&g_privateData   // Private Data Buffer is prepared here
				, (ATTWorkMode_t)workMode
				, reasonCode
				, FALSE);
		}


		if (m_nRetCode == 0)
		{
			m_nRetCode = cstaSetAgentState(*g_acsHandle				// handle returned by acsOpenStream
				, 0 // applicaiton generated invokedID
				, &device_id     // Extension on which Agent logs at
				, AgentMode_t::AM_LOG_IN				// Agent mode (AM_LOG_IN)
				, &agent_id		// ID with which Agent logs in
				, (AgentGroup_t *)""			// Huntgroup Extension (optional)
				, &agent_password // Password with which Agent logs in
				, (PrivateData_t *)&g_privateData); // private Data buffer, prepared above

			//m_nAgtLoginInvokeID = m_ulInvokeID;

			if (m_nRetCode < 0)
			{
				bResult = false;
			}
			else
			{
				bResult = true;
			}

		}


		return (int)m_nRetCode;
	}

	bool AvayaTsapiDLL::AvayaTsapiWrapper::AgentLogOut(String^ l_agentId, String^ l_agentPassword, String^ l_agentExtension)
	{
		bool bResult;
		DeviceID_t	deviceId;
		AgentID_t	agentId;
		AgentPassword_t	agentPassword;
		RetCode_t		nRetCode;

		/*CStringA extensionString(l_agentExtension);
		CStringA userString(l_agentId);
		CStringA passString(l_agentPassword);
		const char* pszServer = extensionString;
		const char* pszUser = userString;
		const char* pszPass = passString;*/
		sprintf(deviceId, "%s", l_agentExtension);
		sprintf(agentId, "%s", l_agentId);
		sprintf(agentPassword, "%s", l_agentPassword);

		//ConvertToCHARBuffer(deviceId,sizeof(deviceId),"%s",l_agentExtension);
		//ConvertToCHARBuffer(agentId,sizeof(agentId),"%s",l_agentId);
		//ConvertToCHARBuffer(agentPassword,sizeof(agentPassword),"%s",l_agentPassword);

		nRetCode = cstaSetAgentState(*g_acsHandle, 0, &deviceId, AgentMode_t::AM_LOG_OUT, &agentId, (AgentGroup_t *)"", &agentPassword, NULL);

		if (nRetCode < 0)
		{
			// TSAPI client has rejected the request.
			switch (nRetCode)
			{
			case ACSERR_BADHDL:
			{
				//m_pCXCtrl->OnSoftPhoneEvent(L"ACSERR_BADHDL");
				WriteToLogFile(__FILE__, __LINE__, 0, "Proble to logout agent with error ACSERR_BADHDL");
				break;
			}
			default:
			{
				bResult = false;
			}break;
			}
			return false;
		}
		else
		{
			//m_pCXCtrl->OnSoftPhoneEvent(L"Loggedout");		
		}

		return true;
	}

	bool AvayaTsapiDLL::AvayaTsapiWrapper::MonitorExtension(String^ l_agentExtension)
	{
		CSTAMonitorFilter_t		m_noFilter;
		RetCode_t               m_nRetCode;
		m_noFilter.call = 0;
		DeviceID_t				deviceID;

		///*IntPtr p = Marshal::StringToHGlobalAnsi(l_agentExtension);
		//const char* pAnsi = static_cast<const char*>(p.ToPointer());
		//Marshal::FreeHGlobal(p);*/

		sprintf(deviceID, "%s", l_agentExtension);
		//ConvertToCHARBuffer(deviceID, , "%s");
		//sprintf(deviceID, "%s", l_agentExtension);
		//
		//ConvertToCHARBuffer(deviceID,sizeof(deviceID),"%s",l_agentExtension);

		m_nRetCode = cstaMonitorDevice(*g_acsHandle,  // handle returned by acsOpenStream method.
			0, // application generated invokeID
			(DeviceID_t *)deviceID,		// ID of the device(Station) to be monitored
			&m_noFilter,  // Filter setting that will apply on the monitor. Set to 0
			// to receive all events 
			(PrivateData_t *)&g_privateData);        // This Private Data Parameter is optional here, and passed as NULL here.	
		if (m_nRetCode < 0)
		{
			switch (m_nRetCode)
			{
			case ACSERR_BADHDL:
			{
				//WriteToLogFile(__FILE__, __LINE__, 0, "Received ACSERR_BADHDL");
				//m_pCXCtrl->OnSoftPhoneEvent(L"ACSERR_BADHDL");
				break;
			}
			case ACSERR_BADPARAMETER:
			{
				//WriteToLogFile(__FILE__, __LINE__, 0, "Received ACSERR_BADPARAMETER");
				//m_pCXCtrl->OnSoftPhoneEvent(L"ACSERR_BADPARAMETER");
				break;
			}
			default:
			{
				//WriteToLogFile(__FILE__, __LINE__, 0, "Received Unknown event while monitoring device");
				break;
			}
			}
			return false;
		}
		else
		{
			//WriteToLogFile(__FILE__, __LINE__, 0, "Device Monitoring is successfuly for %s", deviceID);
			//this->PollForEvents(*g_acsHandle);
		}

		return true;
	}

	bool AvayaTsapiDLL::AvayaTsapiWrapper::StopMonitorExtension(String^ l_agentExtension)
	{
		DeviceID_t monitoredDevice;
		CSTAMonitorCrossRefID_t crossRefId;
		sprintf(monitoredDevice, "%s", l_agentExtension);

		cstaMonitorStop(*g_acsHandle, 0, g_lMonitorCrossRefID, NULL);
			
		return true;
	}

	bool AvayaTsapiDLL::AvayaTsapiWrapper::AnswerCall(String^ agentExtension, long callID)
	{
		ConnectionID_t connID;
		RetCode_t	rCode;

		if (callID != NULL)
		{
			connID.callID = callID;
		}
		else{
			connID.callID = l_callId;
		}
		sprintf(connID.deviceID, "%s", agentExtension);
		
		//ConvertToCHARBuffer(connID.deviceID,sizeof(connID.deviceID),"%s",agentExtension);
		connID.devIDType = ConnectionID_Device_t::STATIC_ID;

		rCode = cstaAnswerCall(*g_acsHandle, 0, &connID, NULL);
		if (rCode < 0)
		{
			WriteToLogFile(__FILE__, __LINE__, 0, "cstaAnswerCall failed");
			return false;
		}

		return true;
	}

	bool AvayaTsapiDLL::AvayaTsapiWrapper::HoldCall(String^ agentExtension, long callID)
	{
		ConnectionID_t connID;
		RetCode_t	rCode;

		//connID.callID = l_callId;
		connID.callID = callID;
		
		//CStringA extensionString(agentExtension);
		//const char* pszExtension = extensionString;
		sprintf(connID.deviceID, "%s", agentExtension);
		
		connID.devIDType = ConnectionID_Device_t::STATIC_ID;
		
		rCode = cstaHoldCall(*g_acsHandle, 0, &connID, false, NULL);
		if (rCode < 0)
		{
			WriteToLogFile(__FILE__, __LINE__, 0, "cstaHoldCall failed");
			return false;
		}
		return true;
	}

	bool AvayaTsapiDLL::AvayaTsapiWrapper::ResumeCall(String^ agentExtension, long callID)
	{
		ConnectionID_t connID;
		RetCode_t	rCode;

		//connID.callID = l_callId;
		connID.callID = callID;
		//CStringA extensionString(agentExtension);
		//const char* pszExtension = extensionString;
		sprintf(connID.deviceID, "%s", agentExtension);
		//ConvertToCHARBuffer(connID.deviceID,sizeof(connID.deviceID),"%s",agentExtension);
		connID.devIDType = ConnectionID_Device_t::STATIC_ID;

		rCode = cstaRetrieveCall(*g_acsHandle, 0, &connID, NULL);
		if (rCode < 0)
		{
			WriteToLogFile(__FILE__, __LINE__, 0, "cstaHoldCall failed");
			return false;
		}

		return true;
	}

	bool AvayaTsapiDLL::AvayaTsapiWrapper::DropCall(String^ agentExtension, long callID)
	{

		ConnectionID_t connID;
		RetCode_t	rCode;

		connID.callID = l_callId;
		//CStringA extensionString(agentExtension);
		//const char* pszExtension = extensionString;
		sprintf(connID.deviceID, "%s", agentExtension);
		//ConvertToCHARBuffer(connID.deviceID,sizeof(connID.deviceID),"%s",agentExtension);
		connID.devIDType = ConnectionID_Device_t::STATIC_ID;

		rCode = cstaClearConnection(*g_acsHandle, 0, &connID, NULL);
		if (rCode < 0)
		{
			WriteToLogFile(__FILE__, __LINE__, 0, "cstaHoldCall failed");
			return false;
		}

		return true;
	}

	bool AvayaTsapiDLL::AvayaTsapiWrapper::InitiateTransfer(String^ agentExtension, String^ remoteExtension)
	{
		ConnectionID_t connID;
		RetCode_t	rCode;
		DeviceID_t	consult_device_id;

		connID.callID = l_callId;
		//CStringA extensionString(agentExtension);
		//const char* pszExtension = extensionString;
		sprintf(connID.deviceID, "%s", agentExtension);
		//ConvertToCHARBuffer(connID.deviceID,sizeof(connID.deviceID),"%s",agentExtension);
		connID.devIDType = ConnectionID_Device_t::STATIC_ID;
		//CStringA remoteExtensionString(remoteExtension);
		//const char* pszRemoteExtension = remoteExtensionString;
		sprintf(consult_device_id, "%s", remoteExtension);
		//ConvertToCHARBuffer(consult_device_id,sizeof(consult_device_id),"%s",remoteExtension);

		rCode = cstaConsultationCall(*g_acsHandle, 0, &connID, &consult_device_id, NULL);
		if (rCode < 0)
		{
			WriteToLogFile(__FILE__, __LINE__, 0, "csta ConsultationCall failed");
			return false;
		}
		return true;
	}

	bool AvayaTsapiDLL::AvayaTsapiWrapper::CompleteTransfer(String^ agentExtension, String^ remoteExtension)
	{
		ConnectionID_t connID;
		ConnectionID_t heldConnID;
		RetCode_t	rCode;
		DeviceID_t	consult_device_id;

		connID.callID = l_callId;
		//CStringA extensionString(agentExtension);
		//const char* pszExtension = extensionString;
		sprintf(connID.deviceID, "%s", agentExtension);
		//ConvertToCHARBuffer(connID.deviceID,sizeof(connID.deviceID),"%s",agentExtension);
		connID.devIDType = ConnectionID_Device_t::STATIC_ID;

		heldConnID.callID = l_heldCallId;
		//CStringA heldExtensionString(agentExtension);
		//const char* pszHeldExtension = heldExtensionString;
		sprintf(heldConnID.deviceID, "%s", agentExtension);
		//ConvertToCHARBuffer(heldConnID.deviceID,sizeof(heldConnID.deviceID),"%s",agentExtension);
		heldConnID.devIDType = ConnectionID_Device_t::STATIC_ID;

		//CStringA remoteExtensionString(remoteExtension);
		//const char* pszRemoteExtension = remoteExtensionString;
		sprintf(consult_device_id, "%s", remoteExtension);
		//ConvertToCHARBuffer(consult_device_id,sizeof(consult_device_id),"%s",remoteExtension);

		rCode = cstaTransferCall(*g_acsHandle, 0, &heldConnID, &connID, NULL);
		if (rCode < 0)
		{
			WriteToLogFile(__FILE__, __LINE__, 0, "cstaTransferCall failed");
			return false;
		}
		return true;
	}

	bool AvayaTsapiDLL::AvayaTsapiWrapper::InitiateConference(String^ agentExtension, String^ remoteExtension)
	{
		ConnectionID_t connID;
		RetCode_t	rCode;
		DeviceID_t	consult_device_id;

		connID.callID = l_callId;
		//CStringA extensionString(agentExtension);
		//const char* pszExtension = extensionString;
		sprintf(connID.deviceID, "%s", agentExtension);
		//ConvertToCHARBuffer(connID.deviceID,sizeof(connID.deviceID),"%s",agentExtension);
		connID.devIDType = ConnectionID_Device_t::STATIC_ID;
		//CStringA remoteExtensionString(remoteExtension);
		//const char* pszRemoteExtension = remoteExtensionString;
		sprintf(consult_device_id, "%s", remoteExtension);
		//ConvertToCHARBuffer(consult_device_id,sizeof(consult_device_id),"%s",remoteExtension);

		rCode = cstaConsultationCall(*g_acsHandle, 0, &connID, &consult_device_id, NULL);
		if (rCode < 0)
		{
			WriteToLogFile(__FILE__, __LINE__, 0, "csta ConsultationCall failed");
			return false;
		}
		return true;
	}

	bool AvayaTsapiDLL::AvayaTsapiWrapper::CompleteConference(String^ agentExtension, String^ remoteExtension)
	{
		ConnectionID_t connID;
		ConnectionID_t heldConnID;
		RetCode_t	rCode;
		DeviceID_t	consult_device_id;

		connID.callID = l_callId;
		//CStringA extensionString(agentExtension);
		//const char* pszExtension = extensionString;
		sprintf(connID.deviceID, "%s", agentExtension);
		//ConvertToCHARBuffer(connID.deviceID,sizeof(connID.deviceID),"%s",agentExtension);
		connID.devIDType = ConnectionID_Device_t::STATIC_ID;

		heldConnID.callID = l_heldCallId;
		//CStringA heldExtensionString(agentExtension);
		//const char* pszHeldExtension = heldExtensionString;
		sprintf(heldConnID.deviceID, "%s", agentExtension);
		//ConvertToCHARBuffer(heldConnID.deviceID,sizeof(heldConnID.deviceID),"%s",agentExtension);
		heldConnID.devIDType = ConnectionID_Device_t::STATIC_ID;
		//CStringA remoteExtensionString(remoteExtension);
		//const char* pszRemoteExtension = remoteExtensionString;
		sprintf(consult_device_id, "%s", remoteExtension);
		//ConvertToCHARBuffer(consult_device_id,sizeof(consult_device_id),"%s",remoteExtension);

		rCode = cstaConferenceCall(*g_acsHandle, 0, &heldConnID, &connID, NULL);
		if (rCode < 0)
		{
			WriteToLogFile(__FILE__, __LINE__, 0, "csta cstaConferenceCall failed");
			return false;
		}

		return true;
	}

	bool AvayaTsapiDLL::AvayaTsapiWrapper::SetAgentState(String^ l_agentLoginID, String^ l_agentPassword, String^ agentExtension, AgentState agentState, WorkMode workMode, int reasonCode)
	{
		RetCode_t	m_nRetCode;
		bool bResult = FALSE;
		DeviceID_t device_id;
		AgentID_t	agent_id;
		AgentMode_t	agent_mode;
		AgentGroup_t agent_group;
		AgentPassword_t agent_password;
		ATTPrivateData_t priv_Data;


		//CStringA extensionString(agentExtension);
		/*CStringA userString(l_agentLoginID);
		CStringA passString(l_agentPassword);
		const char* pszExtension = extensionString;
		const char* pszUser = userString;
		const char* pszPass = passString;*/
		sprintf(device_id, "%s", agentExtension);
		sprintf(agent_id, "%s", l_agentLoginID);
		sprintf(agent_password, "%s", l_agentPassword);

		ATTWorkMode_t wMode = (ATTWorkMode_t)workMode;

		m_nRetCode = attV6SetAgentState(&g_privateData, (ATTWorkMode_t)workMode, reasonCode, FALSE);
		m_nRetCode = cstaSetAgentState(*g_acsHandle, 0, &device_id, (AgentMode_t)agentState, &agent_id, (AgentGroup_t*)"", &agent_password, (PrivateData_t *)&g_privateData);

		if (m_nRetCode < 0)
		{
			bResult = false;
		}
		else
		{
			bResult = true;
		}
		return bResult;
	}

	bool AvayaTsapiDLL::AvayaTsapiWrapper::MakeCall(String^ agentExtension, String^ destinationExtension)
	{
		DeviceID_t callingDevice;
		DeviceID_t calledDevice;
		RetCode_t retCode;

		sprintf(callingDevice, "%s", agentExtension);
		sprintf(calledDevice, "%s", destinationExtension);
		retCode = cstaMakeCall(*g_acsHandle, 0, (DeviceID_t *)callingDevice, (DeviceID_t *)calledDevice, NULL);
		if (retCode < 0)
		{

			//Handle Error

			return false;
		}
		return true;
	}

	void AvayaTsapiDLL::AvayaTsapiWrapper::OnACSConfirmation(CSTAEvent_t *cstaEvent)
	{

	}

	void AvayaTsapiDLL::AvayaTsapiWrapper::OnCSTAConfirmation(CSTAEvent_t *cstaEvent)
	{

	}

	void AvayaTsapiDLL::AvayaTsapiWrapper::OnCSTAUnsolicited(CSTAEvent_t *cstaEvent)
	{

	}

	void AvayaTsapiDLL::AvayaTsapiWrapper::FireEvent(String^ message)
	{
		//if (this->cb)
		//{
		//	//cb(message);
		//}
	}

	void AvayaTsapiDLL::AvayaTsapiWrapper::SetEventCallback(AvayaTsapiDLL::AvayaTsapiWrapper::CallBackFunction^ func)
	{

		//this->func = func;
		this->cb = func;
	}

	void AvayaTsapiDLL::AvayaTsapiWrapper::SetCSTAUnsolicitedCallback(AvayaTsapiDLL::AvayaTsapiWrapper::CSTAUnsolicitedCallback^ func)
	{
		this->cbCSTAUnsolicited = func;
		
	}


}