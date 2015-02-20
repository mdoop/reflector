
#pragma once
using namespace System;

namespace AvayaTsapiDLL
{

	public ref class CSTAEventInfo
	{
	public: 

		long CallID;
		String^ ANI;
		String^ DNIS;
		String^ CallingDevice;
		String^ CalledDevice;
		String^ AnsweringDevice;
		String^ DeviceID;
		String^ HeldDeviceID;
		String^ ConferencedDevicedID;
		String^ TransferringDeviceID;
		String^ TransferredDeviceID;
		String^ UCID;
		String^ EventType;
		String^ UUIData;
		String^ TrunkGroup;
		String^ TrunkMember;
		String^ SourceVDN;
		long PartyCallID;
		String^ PrimaryOldCallDeviceID;
		long PrimaryOldCallCallID;
		String^ SecondaryOldCallDeviceID;
		long SecondaryOldCallCallID;
		long ConferencedCallID;
		String^ ConferenceAddedPartyDevice;
		String^ ConferenceControllerDevice;
		String^ Cause;
		

		//Contstructor
		CSTAEventInfo()
		{
			ANI = "";
			DNIS = "";
			CallID = 0;
			CallingDevice = "";
			CalledDevice = "";
			DeviceID = "";
			HeldDeviceID = "";
			ConferencedDevicedID = "";
			TransferredDeviceID = "";
			UCID = "";
			EventType = "";
			UUIData = "";
			TrunkGroup = "";
			TrunkMember = "";
			SourceVDN = "";
			Cause = "";
		}
	};
}
