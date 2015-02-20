#include "Stdafx.h"
#include "AvayaCallbackHandler.h"


void AvayaTsapiDLL::AvayaCallbackHandler::RegisterAvayaCallback(AvayaEventCallback^ cb)
{

}

void AvayaTsapiDLL::AvayaCallbackHandler::UnReisterAvayaCallback()
{

}

void AvayaTsapiDLL::AvayaCallbackHandler::FireCallback(String^ message)
{
	this->callback(message);

}