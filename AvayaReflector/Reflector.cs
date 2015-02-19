using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Xml.Serialization;
using System.IO;
using AvayaReflector.Configuration;
using log4net.Appender;

namespace AvayaReflector
{
    public class Reflector
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Reflector()
        {
            ReflectorConfig x = new ReflectorConfig();
            x.LoginID = "123";
            x.ReflectorLogFile.Filename = "logfile.t";
            x.ReflectorLogFile.LoggingEnabled = true;
            x.CallDataLogFile.LoggingEnabled = false;
            x.CallDataLogFile.Filename = "file";
            Serialize(x);
            
        }

        public void Serialize(ReflectorConfig list)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ReflectorConfig));
            using ( TextWriter writer = new StreamWriter( @"c:\users\maxd\documents\visual studio 2013\Projects\AvayaReflectorApp_2.0\AvayaReflector\Test.xml") )
            {
                serializer.Serialize(writer, list);
                string x = writer.ToString();
                int s = 0;
            } 


        }

    }
}
