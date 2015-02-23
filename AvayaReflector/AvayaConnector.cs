using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvayaTsapiDLL;
using System.Threading;
using System.Reflection;
using AvayaReflector.AvayaRequests;
using System.Collections.Concurrent;
using log4net.Appender;
using log4net;
using AvayaReflector.CallInformation;
using AvayaReflector.CallCommands;

namespace AvayaReflector
{
    public class AvayaConnector
    {
        public ConcurrentBag<Call> ApplicationCallQueue;
        public ConcurrentBag<Call> CompletedCalls;
        public const int REQUEST_TIMEOUT = 5000;
        AvayaTsapiWrapper tsapi;
        private AutoResetEvent waitingForResponse;
        private static volatile int waitingOnInvokeID;
        ConcurrentDictionary<int, AvayaRequest> AvayaRequestsWaitingForResponse = new ConcurrentDictionary<int, AvayaRequest>();

        public AvayaConnector()
        {
            CompletedCalls = new ConcurrentBag<Call>();
            ApplicationCallQueue = new ConcurrentBag<Call>();
            waitingForResponse = new AutoResetEvent(false);
            tsapi = new AvayaTsapiWrapper();
            tsapi.SetEventCallback(new AvayaTsapiWrapper.CallBackFunction(AvayaEventHandler));
            tsapi.SetCSTAUnsolicitedCallback(new AvayaTsapiWrapper.CSTAUnsolicitedCallback(AvayaCSTAUnsolicitedEventHandler));
        }

        public void SendAndWait(AvayaRequest request)
        {
            request.SetTsapiWrapper(tsapi);
            int requestResult = request.SendRequest();
            if (requestResult > 0)
            {
                request.InvokeID = requestResult;
                AvayaRequestsWaitingForResponse.TryAdd(requestResult, request);
                waitingOnInvokeID = requestResult;
                if (!waitingForResponse.WaitOne(REQUEST_TIMEOUT))
                {
                    request.SetFailure("Request was sent, but timed out. A response was not recieved from TSAPI within " + REQUEST_TIMEOUT + " milliseconds.");
                }
            }
            else
            {
                request.SetFailure("Unknown failure.");
            }
        }

        protected void AvayaEventHandler(int invokeID, string message)
        {
            AvayaRequest originalRequest;
            if (AvayaRequestsWaitingForResponse.TryRemove(invokeID, out originalRequest))
            {
                originalRequest.ProcessResponse(message);

                if (waitingOnInvokeID == invokeID)
                {
                    waitingForResponse.Set();
                }
            } 
        }

        protected void AvayaCSTAUnsolicitedEventHandler(CSTAEventInfo eventInfo)
        {
            //Processing the event and add the call to the queue.
            lock (ApplicationCallQueue)
            {
                CallInfo ci = new CallInfo();
                //ci.ANI = eventInfo.ANI;
                //ci.DNIS = eventInfo.DNIS;
                ci.CallID = eventInfo.CallID;
                ci.DeviceID = eventInfo.DeviceID;
                ci.CallingDeviceID = eventInfo.CallingDevice;
                ci.ANI = eventInfo.ANI;
                ci.UCID = eventInfo.UCID;
                ci.EventDateTime = DateTime.Now;
                ci.EventType = eventInfo.EventType;
                //ci.ScriptName = Configuration.MonitoredDevices.First().CallScript.ScriptName;

                switch (eventInfo.EventType)
                {
                    case "CSTA_DELIVERED":
                        // ApplicationCallScript = Configuration.MonitoredDevices.First().CallScript;
                        // ci.ScriptName = ApplicationCallScript.ScriptName;
                        ci.CalledDeviceID = eventInfo.CalledDevice;
                        ci.ANI = eventInfo.ANI;
                        ci.DNIS = eventInfo.DNIS;
                        ci.UUIData = eventInfo.UUIData;
                        ci.UCID = eventInfo.UCID;
                        //Handle the delivered call and put into the queue.
                        UpdateCallQueue(eventInfo, ci);
                        //Determine if this was a conference or transfer

                        //Execute the script
                        // ApplicationCallScript.callController = this.AvayaConnector;

                        //Getting different CalledDeviceID, need to investigate.
                       /* if (ApplicationCallScript != null && (ApplicationCallScript.DeviceExtension == eventInfo.CalledDevice))
                        {
                            ApplicationCallScript.CallID = ci.CallID;
                            foreach (CallCommand cc in ApplicationCallScript.CallCommands)
                            {
                                //Set the call id to the call id that was delivered.
                                cc.CallID = ci.CallID;
                            }

                            scriptThread = new Thread(new ThreadStart(ApplicationCallScript.ExecuteScript));
                            scriptThread.Name = ApplicationCallScript.ScriptName;
                            scriptThread.Start();

                            //callScript.ExecuteScript();
                        }*/
                        break;
                    case "CSTA_ESTABLISHED":
                        ci.Trunk = eventInfo.TrunkGroup;
                        ci.CallingDeviceID = eventInfo.CallingDevice;
                        ci.ANI = eventInfo.CallingDevice;
                        ci.UUIData = eventInfo.UUIData;
                        ci.UCID = eventInfo.UCID;
                        UpdateCallQueue(eventInfo, ci);
                        break;
                    case "CSTA_ORIGINATED":

                        UpdateCallQueue(eventInfo, ci);
                        break;
                    case "CSTA_HELD":
                        /*
                        if (ApplicationCallScript != null)
                        {
                            CallCommand heldCommand = ApplicationCallScript.CallCommands.Find(x => x.RequestSent == true && x.CommandType == CallCommandType.Hold);
                            heldCommand.ResponseReceived = true;
                        }*/
                        UpdateCallQueue(eventInfo, ci);
                        break;
                    case "CSTA_RETRIEVED":
                       /* if (ApplicationCallScript != null)
                        {
                            if (ApplicationCallScript.CallCommands.Exists(x => x.RequestSent == true && x.CommandType == CallCommandType.Resume))
                            {
                                CallCommand resumeCommand = ApplicationCallScript.CallCommands.Find(x => x.RequestSent == true && x.CommandType == CallCommandType.Resume);
                                resumeCommand.ResponseReceived = true;
                            }
                        }*/
                        UpdateCallQueue(eventInfo, ci);
                        break;
                    case "CSTA_CONNECTION_CLEARED":
                      /*  if (ApplicationCallScript != null)
                        {
                            if (ApplicationCallScript.CallCommands.Exists(x => x.RequestSent == true && x.CommandType == CallCommandType.Drop))
                            {
                                CallCommand dropCommand = ApplicationCallScript.CallCommands.Find(x => x.RequestSent == true && x.CommandType == CallCommandType.Drop);
                                dropCommand.ResponseReceived = true;
                            }
                        }*/
                        UpdateCallQueue(eventInfo, ci);
                        break;
                    case "CSTA_CALL_CLEARED":

                        UpdateCallQueue(eventInfo, ci);
                        break;
                    case "CSTA_TRANSFERRED":
                       /* if (ApplicationCallScript != null)
                        {
                            if (ApplicationCallScript.CallCommands.Exists(x => x.RequestSent == true && x.CommandType == CallCommandType.Transfer))
                            {
                                CallCommand transferCommand = ApplicationCallScript.CallCommands.Find(x => x.RequestSent == true && x.CommandType == CallCommandType.Transfer);
                                transferCommand.ResponseReceived = true;
                            }

                            if (ApplicationCallScript.CallCommands.Exists(x => x.RequestSent == false && x.CommandType == CallCommandType.Answer))
                            {
                                AnswerCall answerConf = (AnswerCall)ApplicationCallScript.CallCommands.First(x => x.RequestSent == false && x.CommandType == CallCommandType.Answer);
                                answerConf.ConfCallID = eventInfo.PartyCallID;
                                answerConf.CallID = eventInfo.PartyCallID;
                            }
                        }*/
                        ci.CallID = eventInfo.PartyCallID;
                        ci.Trunk = eventInfo.TrunkGroup;
                        UpdateCallQueue(eventInfo, ci);
                        break;
                    case "CSTA_CONFERENCED":
                       /* if (ApplicationCallScript != null)
                        {
                            if (ApplicationCallScript.CallCommands.Exists(x => x.RequestSent == true && x.CommandType == CallCommandType.Conference))
                            {
                                CallCommand conferenceCommand = ApplicationCallScript.CallCommands.Find(x => x.RequestSent == true && x.CommandType == CallCommandType.Conference);
                                conferenceCommand.ResponseReceived = true;
                            }
                        }
                        * */
                        ci.Trunk = eventInfo.TrunkGroup;

                        UpdateCallQueue(eventInfo, ci);
                        break;
                    case "CSTA_CONFERENCE_CALL_CONF":
                      /*  if (ApplicationCallScript != null)
                        {
                            if (ApplicationCallScript.CallCommands.Exists(x => x.RequestSent == true && x.CommandType == CallCommandType.Conference))
                            {
                                CallCommand confConfirmCommand = ApplicationCallScript.CallCommands.Find(x => x.RequestSent == true && x.CommandType == CallCommandType.Conference);
                                confConfirmCommand.ConfCallID = eventInfo.ConferencedCallID;
                                if (ApplicationCallScript.CallCommands.Exists(x => x.RequestSent == false && x.CommandType == CallCommandType.Answer))
                                {
                                    AnswerCall answerConf = (AnswerCall)ApplicationCallScript.CallCommands.First(x => x.RequestSent == false && x.CommandType == CallCommandType.Answer);
                                    answerConf.ConfCallID = eventInfo.ConferencedCallID;
                                    answerConf.CallID = eventInfo.ConferencedCallID;
                                }
                            }
                        }*/

                        break;
                    default:
                        //TODO: Unhandled event
                        break;
                }
                // this.AppEventLogger.Event(ci.ToString());
                ci = null;
                //this.AppLogger.Info("EventType: " + eventInfo.EventType + " CallID: " + eventInfo.CallID + " CalledDevice: " + eventInfo.CalledDevice + " CallingDevice: " + eventInfo.CallingDevice + "\r\n" + "UUIData: " + eventInfo.UUIData);

            }
            
        }


        private void UpdateCallQueue(AvayaTsapiDLL.CSTAEventInfo eventInfo, CallInfo ci)
        {
            lock (ApplicationCallQueue)
            {
                if (ApplicationCallQueue.Count(x => x.CallID == ci.CallID) >= 1)
                {

                    //Update the event times based on the event type.
                    switch (eventInfo.EventType)
                    {
                        case "CSTA_ESTABLISHED":
                            ApplicationCallQueue.First(x => x.CallID == ci.CallID).EstablishedEventDateTime = DateTime.Now;
                            ApplicationCallQueue.First(x => x.CallID == ci.CallID).UCID = eventInfo.UCID;
                            ApplicationCallQueue.First(x => x.CallID == ci.CallID).UUIData = eventInfo.UUIData;
                            break;
                        case "CSTA_DELIVERED":
                            ApplicationCallQueue.First(x => x.CallID == ci.CallID).RingingEventDateTime = DateTime.Now;
                            ApplicationCallQueue.First(x => x.CallID == ci.CallID).ANI = ci.ANI;
                            break;
                        case "CSTA_CONNECTION_CLEARED":
                            ApplicationCallQueue.First(x => x.CallID == ci.CallID).ReleasedEventDateTime = DateTime.Now;
                            break;
                        case "CSTA_CALL_CLEARED":
                            ApplicationCallQueue.First(x => x.CallID == ci.CallID).AbandonedEventDateTime = DateTime.Now;
                            break;
                    }

                    ApplicationCallQueue.First(x => x.CallID == ci.CallID).CallInformation.Add(ci);

                    if (ApplicationCallQueue.First(x => x.CallID == ci.CallID).CallInformation.Exists(x => x.EventType == "CSTA_CONNECTION_CLEARED"))
                    {
                        
                         ApplicationCallQueue.First(x => x.CallID == ci.CallID).CallState = CallState.Completed;
                            
                        CompletedCalls.Add(ApplicationCallQueue.First(x => x.CallID == ci.CallID));
                        /*
                        if (Configuration.LogCallData)
                        {
                            List<CallInfo> callInfoList = ApplicationCallQueue.First(x => x.CallID == ci.CallID).CallInformation;

                            XmlRootNode root = new XmlRootNode("CallRoot");



                            //maxdupslaff
                            IQSWebService.KeyValue[] callKeyValues = ApplicationCallQueue.First(x => x.CallID == ci.CallID).GetKeyValueInfo();
                            for (int i = 0; i < callKeyValues.Length; i++)
                            {
                                root.AddTag(callKeyValues[i].Name, callKeyValues[i].Value);
                            }
                            root.AddTag("LoggingTime", string.Format("{0:MM/dd/yyyy HH:mm:ss tt}", DateTime.Now));

                            XmlRootNode callEvents = new XmlRootNode("CallEvents");
                            foreach (CallInfo info in callInfoList)
                            {
                                XmlRootNode callEvent = new XmlRootNode("Event");
                                Type type = info.GetType();
                                PropertyInfo[] properties = type.GetProperties();


                                foreach (PropertyInfo property in properties)
                                {
                                    callEvent.AddTag(property.Name, property.GetValue(info, null).ToString());
                                }
                                callEvents.AddNode(callEvent);
                            }

                            root.AddNode(callEvents);




                            CallDataLogger.Info(root.GetXML());
                        }
                         * */
                    }

                }
                else
                {
                    //Setup a new call and put it into the log.
                    Call call = new Call();
                    call.CallID = ci.CallID;
                    call.ApplicationName = "AvayaReflectorApp";
                    call.AppVersion = "1.0";
                    //call.DrillName = Configuration.CallScripts.FirstOrDefault(x => x.ScriptName == ci.ScriptName).DrillName;
                    // call.TestCase = Configuration.CallScripts.FirstOrDefault(x => x.ScriptName == ci.ScriptName).TestCase;
                    call.DeviceID = ci.DeviceID;
                    call.CalledDeviceID = ci.CalledDeviceID;
                    call.UUIData = ci.UUIData;
                   // call.ServerName = ServerName;
                    call.Platform = "Avaya";
                   // call.SiteName = Configuration.SiteName;
                    call.CallInformation.Add(ci);
                    ApplicationCallQueue.Add(call);
                }
            }

        }
    }
}
