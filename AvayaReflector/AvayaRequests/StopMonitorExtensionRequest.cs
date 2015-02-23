using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvayaReflector.AvayaRequests
{
    public class StopMonitorExtensionRequest : AvayaRequest
    {
         private string deviceExtension;

         public StopMonitorExtensionRequest(string deviceExtension)
             : base(null, "STOP MONITORING EXTENSION", "CSTA_MONITOR_STOP_CONF")
        {
            this.deviceExtension = deviceExtension;
        }

        public override void SetTsapiWrapper(AvayaTsapiDLL.AvayaTsapiWrapper tsapi)
        {
            tsapiMethod = () => tsapi.StopMonitorExtension(deviceExtension);
        }

        protected override string GetRequestInfo()
        {
            return "DeviceExtension = {" + deviceExtension + "}";
        }
    }
}
