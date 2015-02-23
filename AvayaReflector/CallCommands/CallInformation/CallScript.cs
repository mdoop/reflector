using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AvayaReflector.CallCommands;
using System.Threading;

namespace AvayaReflector.CallInformation
{   
    [Serializable]
    public class CallScript
    {

        #region Public Properties
            [XmlElement(ElementName = "ScriptName")]
            public string ScriptName { get; set; }
            [XmlElement(ElementName = "DrillName")]
            public string DrillName { get; set; }
            [XmlElement(ElementName = "TestCase")]
            public string TestCase { get; set; }
            [XmlElement(ElementName = "Extension")]
            public string DeviceExtension { get; set; }
            [XmlIgnore()]
            public long CallID { get; set; }
            [XmlIgnore]
            public long CTCallID { get; set; }
            [XmlArray(ElementName="CallCommands")]
            [XmlArrayItem(ElementName="CallCommand")]
            public List<CallCommand> CallCommands { get; set; }
            [XmlIgnore()]
            public AvayaTsapiDLL.AvayaTsapiWrapper callController { get; set; }
            [XmlIgnore()]
            public Thread ScriptThread { get; set; }
        #endregion

        #region Private Fields
            
        #endregion

        public CallScript()
        {
            CallCommands = new List<CallCommand>();
        }

        public void ExecuteScript()
        {
            ScriptThread = new Thread(new ThreadStart(ProcessScript));
            ScriptThread.Start();
        }

        public void StopExecutingScript()
        {
            if (ScriptThread.ThreadState != ThreadState.Aborted || ScriptThread.ThreadState != ThreadState.Stopped)
            {
                try
                {
                    ScriptThread.Abort();
                }
                catch (ThreadAbortException abortException)
                {
                    //log the error.
                    
                }
            }
        }

        private void ProcessScript()
        {
            foreach (CallCommand cc in CallCommands)
            {
                //Set the device extension.
                cc.Extension = this.DeviceExtension;
                //Check if there is a delay associated with the call command and sleep for Delay * 1000ms
                if (cc.Delay > 0)
                {
                    //Wait Delay
                    System.Threading.Thread.Sleep((cc.Delay * 1000));
                }
                //Execute the request.
                try
                {
                    cc.Request(this.callController);
                    cc.RequestSent = true;
                }
                catch (Exception ex)
                {
                    int x = 0;
                }
                
            }

            //Reset the requested commands  when the script is finished.
            foreach (CallCommand cc in CallCommands)
            {
                cc.RequestSent = false;
                cc.ResponseReceived = false;
            }

            if (ScriptThread.ThreadState != ThreadState.Aborted || ScriptThread.ThreadState != ThreadState.Stopped)
            {
                ScriptThread.Abort();
            }
        }


    }
}
