﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvayaTsapiDLL;
using System.Reflection;

namespace AvayaReflector.AvayaRequests
{
    public class CloseStreamRequest : AvayaRequest
    {
        private string server;
        private string login;
        private string password;

        public CloseStreamRequest(string server, string login, string password)
            : base(null, "CLOSE STREAM", "ACS_CLOSE_STREAM_CONF")
        {
            this.server = server;
            this.login = login;
            this.password = password;
        }

        public override void SetTsapiWrapper(AvayaTsapiWrapper tsapi)
        {
            tsapiMethod = () => tsapi.CloseAESStreamConnection(server, login, password);
        }

        protected override string GetRequestInfo()
        {
            return "Server = {" + server + "}, Login = {" + login + "}, Password = {" + password + "}";
        }
    }
}
