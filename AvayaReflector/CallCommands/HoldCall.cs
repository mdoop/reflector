using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvayaReflector.CallCommands
{
    public class HoldCall : CallCommand
    {
        public String HoldExtension { get; set; }

        public HoldCall() { }

        public HoldCall(String extension)
        {
            this.HoldExtension = extension;
        }
        public override void Request(AvayaTsapiDLL.AvayaTsapiWrapper avayaConnector)
        {
            bool result;
            if (avayaConnector != null)
            {
                result = avayaConnector.HoldCall(this.Extension, this.CallID);
            }
           
        }
    }
}
