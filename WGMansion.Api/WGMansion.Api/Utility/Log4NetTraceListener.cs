using log4net;
using System.Diagnostics;

namespace WGMansion.Api.Utility
{
    public class Log4NetTraceListener : TraceListener
    {
        ILog _logger = LogManager.GetLogger(typeof(Log4NetTraceListener));

        public override void Write(string? message)
        {
            _logger.Info(message);
        }

        public override void WriteLine(string message)
        {
            _logger.Info(message);
        }
    }
}
