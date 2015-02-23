using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AvayaReflector.CallInformation
{
    [Serializable]
    public class MonitoredDevice
    {
        [XmlElement(ElementName="Extension")]
        public string Extension { get; set; }
        [XmlElement(ElementName = "AgentID")]
        public string AgentID { get; set; }
        [XmlElement(ElementName = "AgentPassword")]
        public string AgentPassword { get; set; }
        [XmlElement(ElementName = "CallScriptName")]
        public string CallScriptName { get; set; }
        public CallScript CallScript { get; set; }
        
    }
}
