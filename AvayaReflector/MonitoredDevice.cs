using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvayaReflector
{
    [Serializable]
    public class MonitoredDevice
    {
        public MonitoredDevice(string extension, string agentID, string agentPassword)
        {
            this.Extension = extension;
            this.AgentID = agentID;
            this.AgentPassword = agentPassword;
        }

        public MonitoredDevice(int extension, string agentID, string agentPassword)
            : this(extension.ToString(), agentID, agentPassword) {}

        public MonitoredDevice(int extension, int agentID, string agentPassword)
            : this(extension.ToString(), agentID.ToString(), agentPassword) { }

        public MonitoredDevice(int extension, int agentID, int agentPassword)
            : this(extension.ToString(), agentID.ToString(), agentPassword.ToString()) { }

        public MonitoredDevice(string extension, int agentID, string agentPassword)
            : this(extension, agentID.ToString(), agentPassword) { }

        public MonitoredDevice(string extension, int agentID, int agentPassword)
            : this(extension, agentID.ToString(), agentPassword.ToString()) { }

        public MonitoredDevice(string extension, string agentID, int agentPassword)
            : this(extension, agentID, agentPassword.ToString()) { }

        public MonitoredDevice() { }

        [XmlElement("Extension")]
        public string Extension { get; set; }

        [XmlElement("AgentID")]
        public string AgentID { get; set; }

        [XmlElement("AgentPassword")]
        public string AgentPassword { get; set; }

    }
}
