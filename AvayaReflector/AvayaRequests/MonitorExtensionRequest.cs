using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvayaReflector.AvayaRequests
{
    public class MonitorExtensionRequest : AvayaRequest
    {
        private string deviceExtension;

        public MonitorExtensionRequest(string deviceExtension)
            : base(null, "MONITOR EXTENSION", "CSTA_MONITOR_CONF")
        {
            this.deviceExtension = deviceExtension;
        }

        public override void SetTsapiWrapper(AvayaTsapiDLL.AvayaTsapiWrapper tsapi)
        {
            tsapiMethod = () => tsapi.MonitorExtension(deviceExtension);
        }

        protected override string GetRequestInfo()
        {
            return "DeviceExtension = {" + deviceExtension + "}";
        }
    }
}
