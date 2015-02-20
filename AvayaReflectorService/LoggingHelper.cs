using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace AvayaReflectorService
{
    public static class LoggingHelper
    {   
        public enum ConfigItem
        {
            LOGLEVEL,
            APPENDER,
            LOGGER
        }

        public static void UpdateLog4NetConfigFile(string configFileName, ConfigItem nodeToChange, string identifier, string newValue)
        {
            // Load the config file.
            XmlDocument doc = new XmlDocument();
            doc.Load(configFileName);

            // Create an XPath navigator for the document.
            XPathNavigator nav = doc.CreateNavigator();

            try
            {
                XPathExpression expr;

                switch (nodeToChange)
                {
                    default:

                    case ConfigItem.APPENDER:
                        expr = nav.Compile("/log4net/appender");
                        break;

                    case ConfigItem.LOGLEVEL:
                        expr = nav.Compile("/log4net/logger");
                        break;
                }

                // Locate the node(s) defined by the XPath expression.
                XPathNodeIterator iterator = nav.Select(expr);

                // Iterate on the node set
                while (iterator.MoveNext())
                {
                    XPathNavigator nav2 = iterator.Current.Clone();

                    switch (nodeToChange)
                    {
                        default:
                        case ConfigItem.APPENDER:
                            string appenderName = nav2.GetAttribute( "name", nav.NamespaceURI);
                            if (appenderName.Equals(identifier))
                            {
                                iterator = nav2.Select("param");

                                while (iterator.MoveNext())
                                {
                                    nav2 = iterator.Current.Clone();
                                    string paramName = nav2.GetAttribute("name", nav.NamespaceURI);
                                    if (paramName.Equals("File"))
                                    {
                                        SetAttribute(nav2, String.Empty, "value", nav.NamespaceURI, newValue);
                                        break;
                                    }
                                }
                            }
                            break;
                        
                        case ConfigItem.LOGLEVEL:
                             string loggerName = nav2.GetAttribute( "name", nav.NamespaceURI);
                             if (loggerName.Equals(identifier))
                             {
                                 iterator = nav2.Select("level");
                                 iterator.MoveNext();
                                 nav2 = iterator.Current.Clone();
                                 SetAttribute(nav2, String.Empty, "value", nav.NamespaceURI, newValue);
                             }
                            break;
                    }
                }


                // Save the modified config file.
                doc.Save(configFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void SetAttribute(System.Xml.XPath.XPathNavigator navigator, String prefix, String localName, String namespaceURI, String value)
        {
            if (navigator.CanEdit == true)
            {
                // Check if given localName exist
                if (navigator.MoveToAttribute(localName, namespaceURI))
                {
                    // Exist, so set current attribute with new value.
                    navigator.SetValue(value);
                    // Move navigator back to beginning of node
                    navigator.MoveToParent();
                }
                else
                {
                    // Does not exist, create the new attribute
                    navigator.CreateAttribute(prefix, localName, namespaceURI, value);
                }
            }
        }
    }
    
}
