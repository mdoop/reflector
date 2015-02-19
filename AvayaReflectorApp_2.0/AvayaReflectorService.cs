using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AvayaReflector.Service
{
    public partial class AvayaReflectorService : ServiceBase
    {
        public AvayaReflectorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            new AvayaReflector();
        }

        protected override void OnStop()
        {

        }
    }
}
