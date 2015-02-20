using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AvayaReflector.Configuration
{
    [Serializable]
    public class ReflectorConfig
    {
        public ReflectorConfig() 
        {
            MonitoredDevices = new MonitoredDevices();
            MonitoredDevices.Items.Add(new MonitoredDevice(1234, 1234, 123));
            MonitoredDevices.Items.Add(new MonitoredDevice(1234, 1234, 123));
            MonitoredDevices.Items.Add(new MonitoredDevice(1234, 1234, 123));
            MonitoredDevices.Items.Add(new MonitoredDevice(1234, 1234, 123));
            MonitoredDevices.Items.Add(new MonitoredDevice(1234, 1234, 123));
            CallScript = new CallScript(true, "testScript123");
        }

        [XmlElement("CallScript")]
        public CallScript CallScript { get; set; }

        [XmlElement("MonitoredDevices")]
        public MonitoredDevices MonitoredDevices { get; set; }

        [XmlIgnore]
        public string ServerName { get; set; }

        [XmlIgnore]
        public string StreamLogin { get; set; }

        [XmlIgnore]
        public string StreamPassword { get; set; }

 
    }
}
