using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Kugar.Core.Log
{
    public class NLogFactory : ILoggerFactory
    {
        private string _path = "";

        public NLogFactory(string logsFolderPath = "logs")
        {

            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (string.IsNullOrWhiteSpace(logsFolderPath))
            {
                logsFolderPath = assemblyFolder;
            }

            if (File.Exists("NLog.config"))
            {
                NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(assemblyFolder + "\\NLog.config", true);
            }
            else
            {
                var config = new LoggingConfiguration();

                var fileTarget = new FileTarget();
                config.AddTarget("default", fileTarget);

                if (string.IsNullOrWhiteSpace(logsFolderPath))
                {
                    logsFolderPath = "logs";
                }

                fileTarget.FileName = "${basedir}/" + logsFolderPath + "/log_${shortdate}.log";
                fileTarget.Layout = @"${longdate} ${level} -- ${message}";
                fileTarget.Encoding = Encoding.UTF8;

                var rule2 = new LoggingRule("default", NLog.LogLevel.Trace, fileTarget);
                config.LoggingRules.Add(rule2);

                LogManager.Configuration = config;
            }

            Default = GetLogger("default");
        }

        public ILogger GetLogger(string loggerName)
        {
            var l = NLog.LogManager.GetLogger(loggerName);

            if (l != null)
            {
                return new NLogger(l);
            }
            else
            {
                return null;
            }
        }

        public ILogger Default { get; }
    }
}