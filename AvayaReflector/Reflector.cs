using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Xml.Serialization;
using System.IO;
using log4net.Appender;
using AvayaTsapiDLL;
using AvayaReflector.Configuration;
using System.Threading;
using System.Collections.Concurrent;

namespace AvayaReflector
{
    public class Reflector
    {
        public struct AvayaRequest
        {
            public int InvokeID;
            public AutoResetEvent Reset;
            public bool WaitForResponse;
        }

        public readonly ILog Log;
        public readonly ILog CallLog;

        public AvayaTsapiWrapper AvayaConnector;
        ConcurrentDictionary<AvayaRequest, string> AvayaRequestsWaitingForResponse = new ConcurrentDictionary<AvayaRequest, string>();

        private AutoResetEvent avayaRequestResponse = new AutoResetEvent(false);
        public Reflector(ReflectorConfig config, ILog mainLog, ILog callLog)
        {
            this.Log = mainLog;
            this.CallLog = callLog;
            AvayaConnector = new AvayaTsapiDLL.AvayaTsapiWrapper();

            AvayaConnector.SetEventCallback(new AvayaTsapiWrapper.CallBackFunction(AvayaEventHandler));


           // OpenStream(config.ServerName, config.StreamLogin, config.StreamPassword);

          //  DoAvayaRequest(() => OpenStream(config.ServerName, config.StreamLogin, config.StreamPassword), "open stream");

            DoAvayaRequest(() =>
                AvayaConnector.OpenAESStreamConnection(config.ServerName, config.StreamLogin, config.StreamPassword),
                "OPEN STREAM", true);

            DoAvayaRequest(() =>
                AgentLogin(config.ServerName, config.StreamLogin, config.StreamPassword, AvayaTsapiDLL.WorkMode.WM_MANUAL_IN), 
                "agent login", false);
           // DoAvayaRequest(() => AgentLogin(config.ServerName, config.StreamLogin, config.StreamPassword, AvayaTsapiDLL.WorkMode.WM_MANUAL_IN), "agent login");
            
           // AvayaConnector.AgentLogin(device.AgentID, device.AgentPassword, device.Extension, AvayaTsapiDLL.WorkMode.WM_MANUAL_IN, 0);
            
            int x = 0;
          //  ReflectorConfig x = new ReflectorConfig();
           
           // Serialize(x);
        }

        public int AgentLogin(string agentID, string agentPass, string extension, AvayaTsapiDLL.WorkMode mode)
        {
            return AvayaConnector.AgentLogin(agentID, agentPass, extension, mode, 0);
        }

        public void DoAvayaRequest(Func<int> methodToRun, string message, bool waitForResponse)
        {
            AvayaRequest request = new AvayaRequest
            {
                InvokeID = methodToRun(),
                WaitForResponse = waitForResponse,
                Reset = new AutoResetEvent(false)
            };


            AvayaRequestsWaitingForResponse.TryAdd(request, message);
            if (waitForResponse) request.Reset.WaitOne();
        }


        public int OpenStream(string server, string login, string password)
        {
            return AvayaConnector.OpenAESStreamConnection(server, login, password);
        }



        protected void AvayaEventHandler(int invokeID, string message)
        {
            var originalRequest = AvayaRequestsWaitingForResponse.FirstOrDefault(x => x.Key.InvokeID == invokeID);

            int key = originalRequest.Key.InvokeID;
            if (key > 0)
            {

                if (originalRequest.Key.WaitForResponse)
                {
                    originalRequest.Key.Reset.Set();
                }
   
                
              //  AvayaRequestsWaitingForResponse.Remove(originalRequest.Key);
                string originalMessage = originalRequest.Value;
                Log.Info(originalMessage + ":" + message);
                int c = 0;
            }


        }



        public void Serialize(ReflectorConfig list)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ReflectorConfig));
            using ( TextWriter writer = new StreamWriter( @"c:\users\maxd\documents\visual studio 2013\Projects\AvayaReflectorApp_2.0\AvayaReflector\Test.xml") )
            {
                serializer.Serialize(writer, list);
                string x = writer.ToString();
                int s = 0;
            } 


        }

    }
}
