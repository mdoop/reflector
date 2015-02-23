using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvayaReflector.IQSWebService;

namespace AvayaReflector.CallInformation
{
    public class Call
    {
        #region Public Properties
            public long CallID { get; set; }
            public List<CallInfo> CallInformation { get; set; }
            public CallState CallState { get; set; }
            public String ApplicationName { get; set; }
            public String AppVersion { get; set; }
            public String DrillName { get; set; }
            public String Platform { get; set; }
            public String ServerName { get; set; }
            public String SiteName { get; set; }
            public String TestCase { get; set; }
            public String DeviceID { get; set; }
            public String CalledDeviceID { get; set; }
            public String UCID { get; set; }
            public String UUIData { get; set; }
            public String DNIS { get; set; }
            public String ANI { get; set; }
            public String Queue { get; set; }
            public String Trunk { get; set; }
            public DateTime? RingingEventDateTime { get; set; }
            public DateTime? EstablishedEventDateTime { get; set; }
            public DateTime? ReleasedEventDateTime { get; set; }
            public DateTime? AbandonedEventDateTime { get; set; }
            public DateTime? LastUpdated { get; set; }  

        #endregion
        #region Public Methods
            public List<KeyValuePair<string, string>> AsKeyValue()
            {
                List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>();
                kvpList.Add(new KeyValuePair<string, string>("CallID", this.CallID.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("CallState", this.CallState.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("ApplicationName", this.ApplicationName.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("AppVersion", this.AppVersion.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("DrillName", this.DrillName.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("Platform", this.Platform.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("ServerName", this.ServerName.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("SiteName", this.SiteName.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("TestCase", this.TestCase.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("DeviceID", this.DeviceID.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("CalledDeviceID", this.CalledDeviceID.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("ANI", this.CalledDeviceID.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("DNIS", this.CalledDeviceID.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("Queue", this.Queue.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("Trunk", this.Trunk.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("UCID", this.UCID.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("UUIData", this.UUIData.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("RingingEventDateTime", this.RingingEventDateTime.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("EstablishedEventDateTime", this.EstablishedEventDateTime.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("ReleasedEventDateTime", this.ReleasedEventDateTime.ToString()));
                kvpList.Add(new KeyValuePair<string, string>("AbandonedEventDateTime", this.AbandonedEventDateTime.ToString()));

                return kvpList;
            }

            public Call()
            {
                CallInformation = new List<CallInfo>();
                CallState = AvayaReflector.CallInformation.CallState.NotReady;
                ApplicationName = "";
                AppVersion = "";
                DrillName = "";
                Platform = "";
                ServerName = "";
                SiteName = "";
                TestCase = "";
                UUIData = "";
                DNIS = "";
                ANI = "";
                Queue = "";
                UCID = "";
                Trunk = "";
                RingingEventDateTime = null;
                EstablishedEventDateTime = null;
                ReleasedEventDateTime = null;
                AbandonedEventDateTime = null;
                LastUpdated = null;
            }

            public IQSWebService.KeyValue[] GetKeyValueInfo()
            {
                int counter = 0;

                List<KeyValuePair<String, String>> kvpList = this.AsKeyValue();
                IQSWebService.KeyValue[] kv = new IQSWebService.KeyValue[this.AsKeyValue().Count];

                foreach (KeyValuePair<string, string> kvp in kvpList)
                {
                    IQSWebService.KeyValue keyValue = new IQSWebService.KeyValue();
                    keyValue.Name = kvp.Key;
                    keyValue.Value = kvp.Value;
                    kv[counter] = keyValue;
                    counter++;
                }
                                  
                return kv;
            }

        #endregion
    }

    public enum CallState
    {
        NotReady,
        ReadyToSend,
        Processing,
        Completed
    }


}
