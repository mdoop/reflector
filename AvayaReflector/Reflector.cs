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
using AvayaReflector.AvayaRequests;
using AvayaReflector.CallInformation;

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
        private AvayaConnector avayaConnector;

        public ConcurrentBag<Call> GetCompletedCalls()
        {
            return avayaConnector.CompletedCalls;
        }

        public Reflector(ReflectorConfig config, ILog mainLog, ILog callLog)
        {
            Log = mainLog;
            CallLog = callLog;
            avayaConnector = new AvayaConnector();

            OpenStreamRequest openStreamRequest = new OpenStreamRequest(config.ServerName, config.StreamLogin, config.StreamPassword);
            avayaConnector.SendAndWait(openStreamRequest);

            if (!openStreamRequest.Result.Success)
            {
                Log.Error(openStreamRequest.Result.GetResultMessage());
                Log.Error("Failed to open stream. Aborting.");
                return;
            }

            Log.Info(openStreamRequest.Result.GetResultMessage());




            if (config.MonitoredDevices.LogoutAgents)
            {
                LogoutAgents(config.MonitoredDevices.Items);
            }


            if (config.MonitoredDevices.LoginAgents)
            {
                WorkMode workMode = GetWorkModeFromString(config.MonitoredDevices.AgentWorkMode);
                if (workMode != WorkMode.INVALID)
                {
                    LoginAgents(config.MonitoredDevices.Items, workMode);
                }
                else
                {
                    Log.Error("Unable to login agents: invalid workmode specified. Aborting all agent login attempts.");
                }
            }



            foreach(MonitoredDevice device in config.MonitoredDevices.Items)
            {
                MonitorExtensionRequest monitorRequest = new MonitorExtensionRequest(device.Extension);
                avayaConnector.SendAndWait(monitorRequest);

                if (!monitorRequest.Result.Success)
                {
                    Log.Error(monitorRequest.Result.GetResultMessage());
                }
                else
                {
                    Log.Info(monitorRequest.Result.GetResultMessage());
                }
            }




            //  ReflectorConfig x = new ReflectorConfig();

            // Serialize(x);
        }

        public void LoginAgents(List<MonitoredDevice> devices, WorkMode workMode)
        {
            foreach (MonitoredDevice device in devices)
            {
                AgentLoginRequest loginRequest = new AgentLoginRequest(device.AgentID, device.AgentPassword, device.Extension, workMode);
                avayaConnector.SendAndWait(loginRequest);

                if (!loginRequest.Result.Success)
                {
                    Log.Error(loginRequest.Result.GetResultMessage());
                }
                else
                {
                    Log.Info(loginRequest.Result.GetResultMessage());
                }
            }
        }


        public void LogoutAgents(List<MonitoredDevice> devices)
        {
            foreach (MonitoredDevice device in devices)
            {
                AgentLogoutRequest logoutRequest = new AgentLogoutRequest(device.AgentID, device.AgentPassword, device.Extension);
                avayaConnector.SendAndWait(logoutRequest);

                if (!logoutRequest.Result.Success)
                {
                    Log.Error(logoutRequest.Result.GetResultMessage());
                }
                else
                {
                    Log.Info(logoutRequest.Result.GetResultMessage());
                }
            }
        }



        public WorkMode GetWorkModeFromString(string mode)
        {
            switch(mode)
            {
                case "MANUAL_IN":
                    return WorkMode.WM_MANUAL_IN;

                case "AUX_WORK":
                    return WorkMode.WM_AUX_WORK;

                case "AFTCAL_WK":
                        return WorkMode.WM_AFTCAL_WK;

                case "AUTO_IN":
                    return WorkMode.WM_AUTO_IN;

                case "NONE":
                    return WorkMode.WM_NONE;

                default:
                    return WorkMode.INVALID;
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
