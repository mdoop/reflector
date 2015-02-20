using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AvayaReflectorService.Configuration
{
    [Serializable]
    public class ReflectorServiceConfig
    {   
        public ReflectorServiceConfig()
        {
            ServerName = "Hello server!";
            Password = "123pass";
            CallDataLogFile = new LoggingConifg();
            ReflectorLogFile = new LoggingConifg();
            WebService = new WebServiceConfig();
            WebService.Enabled = true;
            WebService.MaxAttempts = 100;
            WebService.RetryInterval = 100;
            WebService.Timeout = 100;
            WebService.Url = "hello.com";
        }

        [XmlElement("ServerName")]
        public string ServerName { get; set; }

        [XmlElement("LoginID")]
        public string LoginID { get; set; }

        [XmlElement("Password")]
        public string Password { get; set; }

        [XmlElement("ReflectorLogFile")]
        public LoggingConifg ReflectorLogFile { get; set; }

        [XmlElement("CallDataLogFile")]
        public LoggingConifg CallDataLogFile { get; set; }

        [XmlElement("WebService")]
        public WebServiceConfig WebService { get; set; }
       
    }
}
