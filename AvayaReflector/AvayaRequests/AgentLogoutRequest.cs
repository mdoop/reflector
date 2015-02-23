using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AvayaReflector.AvayaRequests
{
    public class AgentLogoutRequest : AvayaRequest
    {
        private string agentID;
        private string agentPassword;
        private string deviceExtension;
        private AvayaTsapiDLL.WorkMode workMode;

        public AgentLogoutRequest(string agentID, string agentPassword, string deviceExtension)
            : base(null, "AGENT LOGOUT", "CSTA_SET_AGENT_STATE_CONF")
        {
            this.agentID = agentID;
            this.agentPassword = agentPassword;
            this.deviceExtension = deviceExtension;
        }

        public override void SetTsapiWrapper(AvayaTsapiDLL.AvayaTsapiWrapper tsapi)
        {
            tsapiMethod = () => tsapi.AgentLogOut(agentID, agentPassword, deviceExtension);
        }

        protected override string GetRequestInfo()
        {
            return "AgentID = {" + agentID + "}, AgentPassword = = {" + agentPassword + "}, DeviceExtension = {" + deviceExtension + "}";
        }

    }
}
