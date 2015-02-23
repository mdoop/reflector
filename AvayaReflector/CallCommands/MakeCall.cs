using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvayaReflector.CallCommands
{
    public class MakeCall : CallCommand
    {
        public string DeviceExtension { get; set; }
        public string DestinationDeviceExtesion { get; set; }


        public override void Request(AvayaTsapiDLL.AvayaTsapiWrapper avayaController)
        {
            if (avayaController != null)
            {

                bool result = avayaController.MakeCall(this.DeviceExtension, this.DestinationDeviceExtesion);
                if (!result)
                {
                    throw new Exception("Make Call Failed.");
                }
            }
        }
    }
}
