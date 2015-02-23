using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvayaReflector.CallCommands
{
    [Serializable]
    public class DropCall : CallCommand
    {
        [XmlElement(ElementName = "DropExtension")]
        public string DropExtension { get; set; }
        public override void Request(AvayaTsapiDLL.AvayaTsapiWrapper avayaController)
        {
            if (avayaController != null)
            {
                if (this.DropExtension == null) { this.DropExtension = this.Extension; }
                bool result = avayaController.DropCall(this.DropExtension, this.CallID);
                if (result)
                {
                    //Log the request has been successfully sent.

                }
                else
                {
                    //Log the request was not successful.

                }
            }
            else
            {
                //Log error.
            }
        }
    }
}
