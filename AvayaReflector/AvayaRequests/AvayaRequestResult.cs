using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvayaReflector.AvayaRequests
{
    public class AvayaRequestResult
    {
        protected string resultMessage;
        public bool Success; 
        
        public AvayaRequestResult(bool success, string resultMessage)
        {
            Success = success;
            this.resultMessage = resultMessage;
        }

        public string GetResultMessage()
        {
            return resultMessage;
        }
    }
}
