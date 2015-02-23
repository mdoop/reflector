using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AvayaReflector.AvayaRequests;

namespace AvayaReflector
{
    public abstract class AvayaRequest
    {
        protected string successfullResponseMessage;
        public int InvokeID;
        protected Func<int> tsapiMethod;
        protected string requestName;
        protected string reasonMessage;
        protected bool requestSucess;
        private AvayaRequestResult _result;

        public AvayaRequest(Func<int> tsapiMethod, string requestName, string successfullResponseMessage)
        {
            this.tsapiMethod = tsapiMethod;
            this.requestName = requestName;
            this.successfullResponseMessage = successfullResponseMessage;
        }


        public string GetName()
        {
            return requestName;
        }

        protected abstract string GetRequestInfo();

        public void ProcessResponse(string message)
        {
            reasonMessage = message;

            if (message == successfullResponseMessage)
            {
                requestSucess = true;
            }
            else
            {
                requestSucess = false;
            }
        }

        public abstract void SetTsapiWrapper(AvayaTsapiDLL.AvayaTsapiWrapper tsapi);

        public int SendRequest()
        {
            return tsapiMethod();
        }

        public void SetFailure(string reason)
        {
            reasonMessage = reason;
            requestSucess = false;
        }

        public AvayaRequestResult Result
        {
            get
            {
                if (_result == null)
                {
                    string successOrFailure = requestSucess ? "SUCCESS" : "FAILURE";
                    string resultString = requestName.ToUpper() + ": " + successOrFailure + ". " + GetRequestInfo() + ". Tsapi Message: " + reasonMessage;
                    _result = new AvayaRequestResult(requestSucess, resultString);
                }

                return _result;
            }
        }
    }
}
