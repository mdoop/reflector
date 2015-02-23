using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AvayaReflector.CallInformation
{
    public class CallQueue : System.Collections.Concurrent.ConcurrentBag<Call>
    {
        public CallQueue()
        {
            
        }

        public IQSWebService.KeyValue[] GetCallInfoForWebService()
        {
            IQSWebService.KeyValue[] data;
            data = new IQSWebService.KeyValue[100];

            List<IQSWebService.KeyValue> keyValueList = new List<IQSWebService.KeyValue>();

            //Query to see what is available to send.
            foreach (Call call in this)
            {
                //List<CallInfo> callList = this[callID].ToList<CallInfo>();
                //If the call is in Ready To Send status, get the keyvalue data.
                if (call.CallState == CallState.ReadyToSend)
                {
                    foreach (CallInfo ci in call.CallInformation)
                    {
                        //convert to the KeyValue for the web service.
                        //data = ci.GetCallInfoKeyValues();
                    }
                }
            }


            return data;
        }
   
    }
}
