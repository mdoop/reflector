using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AvayaReflector.Configuration
{
    [XmlType("MonitoredDevices")]
    public class MonitoredDevices
    {
        public MonitoredDevices()
        {
            Items = new List<MonitoredDevice>();
        }

        [XmlAttribute("loginAgentsOnStartup")]
        public bool LoginAgents { get; set; }

        [XmlElement("MonitoredDevice")]
        public List<MonitoredDevice> Items { get; set; }
    }
}
