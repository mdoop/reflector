using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AvayaReflector.Configuration
{
    public class WebServiceConfig
    {
        public WebServiceConfig() { }

        [XmlAttribute("enabled")]
        public bool Enabled { get; set; }

        [XmlElement("Timeout")]
        public int Timeout { get; set; }

        [XmlElement("RetryInterval")]
        public int RetryInterval { get; set; }

        [XmlElement("MaxAttempts")]
        public int MaxAttempts { get; set; }

        [XmlElement("Url")]
        public string Url { get; set; }

    }
}
