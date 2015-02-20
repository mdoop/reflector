using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AvayaReflectorService.Configuration
{
    public static class ReflectorConfigFactory
    {
        public static T GetConfig<T>(string configFileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlReader reader = XmlReader.Create(configFileName);
            T configObj = (T)serializer.Deserialize(reader);
            reader.Close();

            return configObj;
        }

    }
}
