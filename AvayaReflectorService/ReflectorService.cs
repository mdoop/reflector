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
using AvayaReflectorService.Configuration;
using AvayaReflector.Configuration;
using System.IO;
using System.Xml.XPath;
using System.Xml;

namespace AvayaReflectorService
{
    public partial class ReflectorService : ServiceBase
    {
        private const string LOG4NET_CONFIG_FILE = "log4net.config";
        private const string REFLECTOR_LOG = "Reflector"; // these values need to match the logger names defined in log4net.xml
        private const string CALLDATA_LOG = "CallData";
        private const string REFLECTOR_APPENDER = "ReflectorAppender"; // these values need to match the appender names defined in log4net.xml
        private const string CALLDATA_APPENDER = "CallDataAppender";

        private ReflectorServiceConfig config;
        private ILog reflectorLog;
        private ILog callLog;


        public ReflectorService()
        {
            config = ReflectorConfigFactory.GetConfig<ReflectorServiceConfig>("ServiceSettings.xml");
            InitLogging();
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ReflectorConfig reflectorConfig = ReflectorConfigFactory.GetConfig<ReflectorConfig>("ReflectorConfig.xml");
            new Reflector(SetServerSettings(reflectorConfig), reflectorLog, callLog);
        }

        public void OnDebug(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStop()
        {

        }

        private ReflectorConfig SetServerSettings(ReflectorConfig reflectorConfig)
        {
            reflectorConfig.ServerName = config.ServerName;
            reflectorConfig.StreamLogin = config.LoginID;
            reflectorConfig.StreamPassword = config.Password;

            return reflectorConfig;
        }


        // We use this method to load custom file names for our loggers. 
        // These custom filenames are specified in "ServiceSettings.xml", under
        // "ReflectorLogFile" and "CallDataLogFile".
        //
        // The goal is to have the user modify ONE file for the ReflectorService,
        // so rather than having them change ServiceSettings.xml AND log4net.xml,
        // we use this method to help elimnate that extra step. They just need to 
        // change "ServiceSettings.xml", and NOT log4net.xml.
        private void InitLogging()
        {
            string reflectorLogLevel = config.ReflectorLogFile.LoggingEnabled ? "INFO" : "OFF";
            string callLogLevel = config.CallDataLogFile.LoggingEnabled ? "INFO" : "OFF";

            LoggingHelper.UpdateLog4NetConfigFile(LOG4NET_CONFIG_FILE, LoggingHelper.ConfigItem.LOGLEVEL, REFLECTOR_LOG, reflectorLogLevel);
            LoggingHelper.UpdateLog4NetConfigFile(LOG4NET_CONFIG_FILE, LoggingHelper.ConfigItem.LOGLEVEL, CALLDATA_LOG, callLogLevel);
            LoggingHelper.UpdateLog4NetConfigFile(LOG4NET_CONFIG_FILE, LoggingHelper.ConfigItem.APPENDER, CALLDATA_APPENDER, config.CallDataLogFile.Filename);
            LoggingHelper.UpdateLog4NetConfigFile(LOG4NET_CONFIG_FILE, LoggingHelper.ConfigItem.APPENDER, REFLECTOR_APPENDER, config.ReflectorLogFile.Filename);

            XmlConfigurator.Configure();

            reflectorLog = LogManager.GetLogger(REFLECTOR_LOG);
            callLog = LogManager.GetLogger(CALLDATA_LOG);
        }
    }

}
