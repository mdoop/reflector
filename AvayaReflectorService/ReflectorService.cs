using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using AvayaReflector;
using log4net;
using log4net.Config;
using log4net.Appender;

namespace AvayaReflectorService
{
    public partial class ReflectorService : ServiceBase
    {
        public ReflectorService()
        {
            XmlConfigurator.Configure();
            foreach (IAppender a in LogManager.GetLogger("Reflector").Logger.Repository.GetAppenders())
            {
                if (a is FileAppender)
                {
                    FileAppender fa = (FileAppender)a;
                    // Programmatically set this to the desired location here
                    string logFileLocation = @"C:\TEMP\AvayaReflectorServiceLogHAHAhAH.log";

                    // Uncomment the lines below if you want to retain the base file name
                    // and change the folder name...
                    //FileInfo fileInfo = new FileInfo(fa.File);
                    //logFileLocation = string.Format(@"C:\MySpecialFolder\{0}", fileInfo.Name);

                    fa.File = logFileLocation;
                    fa.ActivateOptions();
                    break;
                }
            }

            //private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            new Reflector();
        }

        public void OnDebug(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStop()
        {
        }
    }
}
