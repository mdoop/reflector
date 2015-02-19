using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AvayaReflector.Configuration
{
    public class LoggingConifg
    {
        public LoggingConifg() { }

        [XmlAttribute("loggingEnabled")]
        public bool LoggingEnabled { get; set; }

        [XmlAttribute("filename")]
        public string Filename { get; set; }
    }
}
