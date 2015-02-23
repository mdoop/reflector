using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AvayaReflector.CallInformation;


namespace AvayaReflector.CallCommands
{
    [Serializable]
    [XmlInclude(typeof(AnswerCall)), XmlInclude(typeof(HoldCall)), XmlInclude(typeof(ResumeCall)), XmlInclude(typeof(DropCall)), XmlInclude(typeof(TransferCall)), XmlInclude(typeof(ConferenceCall))]
    public abstract class CallCommand
    {
        
        public abstract void Request(AvayaTsapiDLL.AvayaTsapiWrapper avayaController);
        [XmlElement(ElementName="CallCommandType")]
        public CallCommandType CommandType;
        [XmlElement(ElementName="Delay")]
        public int Delay { get; set; }
        public string Extension { get; set; }
        [XmlIgnore()]
        public int CallID
        {
            get;
            set;
        }
        [XmlIgnore()]
        public int ConfCallID { get; set; }
        [XmlIgnore()]
        public bool RequestSent { get; set; }
        [XmlIgnore()]
        public bool ResponseReceived { get; set; }

    }
}
