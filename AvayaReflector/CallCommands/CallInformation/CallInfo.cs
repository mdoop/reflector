using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvayaReflector.CallCommands;
using AvayaReflector.IQSWebService;
using System.Reflection;

namespace AvayaReflector.CallInformation
{
    public class CallInfo
    {
        #region Public Members

            public int CallID { get; set; }
            public string DeviceID { get; set; }
            public string CalledDeviceID { get; set; }
            public string CallingDeviceID { get; set; }
            public string EventType { get; set; }
            public string UCID { get; set; }
            public string UUIData { get; set; }
            public string UUIType { get; set; }
            public string UUILength { get; set; }
            public string ANI { get; set; }
            public string DNIS { get; set; }
            public string Trunk { get; set; }
            public string ConnectionID { get; set; }
            public string PreviousConnectionID { get; set; }
            public string CallType { get; set; }
            public long NetworkCallID { get; set; }
            public string Queue { get; set; }
            public DateTime? EventDateTime { get; set; }
            public String ScriptName { get; set; }
            public bool SentToWebService { get; set; }

        #endregion

        #region Constructor/Destructor
            public CallInfo()
            {
                CallID = 0;
                DeviceID = "";
                CalledDeviceID = "";
                CallingDeviceID = "";
                EventType = "";
                UCID = "";
                UUIData = "";
                UUIType = "";
                UUILength = "";
                ANI = ""; 
                DNIS = ""; 
                Trunk = ""; 
                ConnectionID  = "";
                PreviousConnectionID  = "";
                CallType  = "";
                NetworkCallID = 0;
                Queue  = "";
                EventDateTime = null;
                ScriptName = "";
                SentToWebService = false;
            }
        #endregion

        #region Public Methods
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("****" + EventType + "****");
                sb.AppendLine("CallID: " + CallID);
                sb.AppendLine("DeviceID: " + DeviceID);
                sb.AppendLine("CalledDeviceID: " + CalledDeviceID);
                sb.AppendLine("CallingDeviceID: " + CallingDeviceID);
                sb.AppendLine("UCID: " + UCID);
                sb.AppendLine("UUIData: " + UUIData);
                sb.AppendLine("UUIType: " + UUIType);
                sb.AppendLine("UUILength: " + UUILength);
                sb.AppendLine("EventType: " + EventType);
                sb.AppendLine("ANI: " + ANI);
                sb.AppendLine("ConnectionID: " + ConnectionID);
                sb.AppendLine("PreviousConnectionID: " + PreviousConnectionID);
                sb.AppendLine("CallType: " + CallType);
                sb.AppendLine("NetworkCallID: " + NetworkCallID);
                sb.AppendLine("Queue: " + Queue);
                sb.AppendLine("Script: " + ScriptName);

                return sb.ToString();
            }
            
            public List<KeyValuePair<string, string>> AsKeyValue()
            {
                List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>();
                kvpList.Add(new KeyValuePair<string, string>("CallID", this.CallID.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("DeviceID", this.DeviceID.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("CalledDeviceID", this.CalledDeviceID.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("UCID", this.UCID.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("UUIData", this.UUIData.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("UUIType", this.UUIType.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("UUILength", this.UUILength.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("EventType", this.EventType.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("ANI", this.CallID.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("DNIS", this.DNIS.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("Trunk", this.Trunk.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("ConnectionID", this.ConnectionID.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("PreviousConnectionID", this.PreviousConnectionID.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("CallType", this.CallType.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("NetworkCallID", this.NetworkCallID.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("Queue ", this.Queue.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("EventDateTime", this.EventDateTime.ToString()));

                //kvpList.Add(new KeyValuePair<string, string>("Script", this.Script.ToString()));


                return kvpList;
            }

        #endregion

    }
}
