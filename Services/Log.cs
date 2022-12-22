using bagant.Params;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using System;
using System.Text;

namespace bagant.Services
{
    public class Log
    {
        private static ILog Logger = LogManager.GetLogger(typeof(Log));
        private static readonly string CadenaMsge = "(BAGANT)[id]: " + Parameters.IDENTIFIER + " [Message] -> {0}";
        private static Log LogClass = new Log();

        public static ILog LogVar
        {
            get
            {
                return Logger;
            }
        }

        private Log()
        {
            _init();
        }

        private static void _init()
        {
            var appender = new FileAppender()
            {
                Layout = new PatternLayout("%date (%p) %message%newline"),
                File = Parameters.PATH_LOG + "BAGANT_" + Environment.UserName + "_" + (DateTime.Now.ToString("yyyyMMdd")) + ".log",
                Encoding = Encoding.UTF8,
                AppendToFile = true,
                LockingModel = new FileAppender.MinimalLock()
            };

            appender.ActivateOptions();
            BasicConfigurator.Configure(appender);
        }

        public static void Info(string message)
        {
            LogVar.Info(string.Format(CadenaMsge, message));
        }

        public static void Error(string message)
        {
            LogVar.Error(string.Format(CadenaMsge, message));
        }

        public static void Fatal(string message)
        {
            Logger.Fatal(string.Format(CadenaMsge, message));
        }

        public static void Debug(string message)
        {
            Logger.Debug(string.Format(CadenaMsge, message));
        }

        public static void Warn(string message)
        {
            Logger.Warn(string.Format(CadenaMsge, message));
        }


    }
}
