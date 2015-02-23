using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvayaTsapiDLL;
using System.Reflection;

namespace AvayaReflector.AvayaRequests
{
    public class OpenStreamRequest : AvayaRequest
    {
        private string server;
        private string login;
        private string password;

        public OpenStreamRequest(AvayaTsapiWrapper tsapi, string server, string login, string password)
            : base(() => tsapi.OpenAESStreamConnection(server, login, password), "OPEN STREAM", "ACS_OPEN_STREAM_CONF")
        {
            this.server = server;
            this.login = login;
            this.password = password;
        }

        public OpenStreamRequest(string server, string login, string password)
            : base(null, "OPEN STREAM", "ACS_OPEN_STREAM_CONF")
        {
            this.server = server;
            this.login = login;
            this.password = password;
        }

        public override void SetTsapiWrapper(AvayaTsapiWrapper tsapi)
        {
            tsapiMethod = () => tsapi.OpenAESStreamConnection(server, login, password);
        }

        protected override string GetRequestInfo()
        {
            return "Server = {" + server + "}, Login = {" + login + "}, Password = {" + password + "}";
        }
    }
}
