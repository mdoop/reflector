using AvayaTsapiDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AvayaReflector.AvayaRequests
{
    public class AgentLoginRequest : AvayaRequest
    {
        private string agentID;
        private string agentPassword;
        private string deviceExtension;
        private AvayaTsapiDLL.WorkMode workMode;

        public AgentLoginRequest(string agentID, string agentPassword, string deviceExtension, AvayaTsapiDLL.WorkMode workMode)
            : base(null, "AGENT LOGIN", "CSTA_SET_AGENT_STATE_CONF")
        {
            this.agentID = agentID;
            this.agentPassword = agentPassword;
            this.deviceExtension = deviceExtension;
            this.workMode = workMode;
        }

        public override void SetTsapiWrapper(AvayaTsapiDLL.AvayaTsapiWrapper tsapi)
        {
            tsapiMethod = () => tsapi.AgentLogin(agentID, agentPassword, deviceExtension, workMode, 0);
        }

        protected override string GetRequestInfo()
        {
            string workModeString = "";
            switch (workMode)
            {
                case WorkMode.WM_MANUAL_IN:
                    workModeString = "MANUAL_IN";
                    break;

                case WorkMode.WM_AUX_WORK:
                    workModeString = "AUX_WORK";
                    break;

                case WorkMode.WM_AFTCAL_WK:
                    workModeString = "AFTCAL_WK";
                    break;

                case WorkMode.WM_AUTO_IN:
                    workModeString = "AUTO_IN";
                    break;

                case WorkMode.WM_NONE:
                    workModeString = "WM_NONE";
                    break;
                
                default:
                    break;
            }
            return "AgentID = {" + agentID + "}, AgentPassword = {" + agentPassword + "}, DeviceExtension = {" + deviceExtension + "}, Workmode = {" + workModeString + "}";
;
        }

    }
}
