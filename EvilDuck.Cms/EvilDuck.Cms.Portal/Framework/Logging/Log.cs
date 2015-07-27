using Microsoft.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvilDuck.Cms.Portal.Framework.Logging
{
    public class Log : ILog
    {

        private ILoggerFactory _factory;
        private ILogger _logger;

        public Log(ILoggerFactory loggerFactory)
        {
            _factory = loggerFactory;
        }

        public void LogError(Func<string> func)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(func());
            }
        }

        public void LogWarn(Func<string> func)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(func());
            }
        }

        public void LogInfo(Func<string> func)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(func());
            }
        }

        public void Init(string name)
        {
            _logger = _factory.CreateLogger(name);
        }

    }
}
