using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace POC.Core.Logging
{
    public class Flogger
    {
        #region private members

        private static readonly ILogger _perfLogger;
        private static readonly ILogger _usageLogger;
        private static readonly ILogger _errorLogger;
        private static readonly ILogger _diagnosticLogger;

        #endregion


        #region ctor

        static Flogger()
        {
            _perfLogger = new LoggerConfiguration()
                .WriteTo.File("d:\\temp\\loggingPoc\\perf.txt")
                .CreateLogger();

            _usageLogger = new LoggerConfiguration()
                .WriteTo.File("d:\\temp\\loggingPoc\\usage.txt")
                .CreateLogger();


            _errorLogger = new LoggerConfiguration()
                .WriteTo.File("d:\\temp\\loggingPoc\\error.txt")
                .CreateLogger();


            _diagnosticLogger = new LoggerConfiguration()
                .WriteTo.File("d:\\temp\\loggingPoc\\diagnostic.txt")
                .CreateLogger();

        }

        #endregion

        public static void WritePerf(LogDetail infoToLog)
        {
            _perfLogger.Write(LogEventLevel.Information, "{@LogDetail}", infoToLog);
        }

        public static void WriteUsage(LogDetail infoToLog)
        {
            _usageLogger.Write(LogEventLevel.Information, "{@LogDetail}", infoToLog);
        }
        public static void WriteError(LogDetail infoToLog)
        {
            _errorLogger.Write(LogEventLevel.Information, "{@LogDetail}", infoToLog);
        }
        public static void WriteWriteDiagnostic(LogDetail infoToLog)
        {
            var writeDiagnostics = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableDiagnostics"]);
            if (!writeDiagnostics)
            {
                return;
            }

            _diagnosticLogger.Write(LogEventLevel.Information, "{@LogDetail}", infoToLog);
        }
    }
}
