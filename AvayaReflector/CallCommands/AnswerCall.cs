using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvayaReflector.CallCommands
{
    [Serializable]
    public class AnswerCall : CallCommand
    {
        [XmlElement(ElementName="AnswerExtension")]
        public string AnswerExtension { get; set; }
        
        public AnswerCall() { }

        public override void Request(AvayaTsapiDLL.AvayaTsapiWrapper avayaConnector)
        {
            bool result;
            if (avayaConnector != null)
            {
                string ext;
                if (this.AnswerExtension == null) { ext = this.Extension; } else { ext = this.AnswerExtension; }
                result = avayaConnector.AnswerCall(ext, this.CallID);
            }

        }
    }
}
