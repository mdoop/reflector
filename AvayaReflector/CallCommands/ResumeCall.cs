using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvayaReflector.CallCommands
{
    [Serializable]
    public class ResumeCall : CallCommand
    {
        public ResumeCall() { }

        public String ResumeExtension { get; set; }


        public override void Request(AvayaTsapiDLL.AvayaTsapiWrapper avayaController)
        {
            if (avayaController != null)
            {
                bool result = avayaController.ResumeCall(this.Extension, this.CallID);
            }
        }
    }
}
