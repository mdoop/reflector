using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AvayaReflector
{
    public class CallScript
    {
        public CallScript(bool enabled, string scriptName)
        {
            this.Enabled = enabled;
            this.ScriptName = scriptName;
        }

        public CallScript() { }

        [XmlAttribute("enabled")]
        public bool Enabled { get; set; }

        [XmlAttribute("scriptName")]
        public string ScriptName { get; set; }
    }
}
