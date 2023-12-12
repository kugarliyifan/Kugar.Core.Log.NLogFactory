using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NLog;

namespace Kugar.Core.Log
{
    public class NLoggerProviderForDI : ILoggerProvider
    {
        private NLogFactory _factory = null;

        public NLoggerProviderForDI(NLogFactory factory = null)
        {
            _factory = factory ?? (NLogFactory)LoggerManager.LoggerFactory;
        }

        public void Dispose()
        {

        }

        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            var logger = (NLogger)_factory.GetLogger(categoryName);

            return new NLoggerForDI(logger);
        }

        public class NLoggerForDI : Microsoft.Extensions.Logging.ILogger, IDisposable
        {
            private NLogger _logger = null;

            public NLoggerForDI(NLogger logger)
            {
                _logger = logger;
            }

            public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                var msg = formatter?.Invoke(state, exception) ?? exception?.Message;

                if (!string.IsNullOrEmpty(msg))
                {
                    msg = $"{eventId.Name} {msg}";

                    switch (logLevel)
                    {
                        case Microsoft.Extensions.Logging.LogLevel.Trace:
                            if (_logger.IsTraceEnable) _logger.Trace(msg);
                            break;
                        case Microsoft.Extensions.Logging.LogLevel.Debug:
                            if (_logger.IsDebugEnable) _logger.Debug(msg);
                            break;
                        case Microsoft.Extensions.Logging.LogLevel.Information:
                            if (_logger.IsDebugEnable) _logger.Debug(msg);
                            break;
                        case Microsoft.Extensions.Logging.LogLevel.Warning:
                            if (_logger.IsWarnEnable) _logger.Warn(msg, exception);
                            break;
                        case Microsoft.Extensions.Logging.LogLevel.Error:
                            if (_logger.IsErrorEnable) _logger.Error(msg, error: exception);
                            break;
                        case Microsoft.Extensions.Logging.LogLevel.Critical:
                            if (_logger.IsErrorEnable) _logger.Error(msg, error: exception);
                            break;
                        case Microsoft.Extensions.Logging.LogLevel.None:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
                    }
                }
            }

            public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
            {
                switch (logLevel)
                {
                    case Microsoft.Extensions.Logging.LogLevel.Trace:
                        return _logger.IsTraceEnable;
                    case Microsoft.Extensions.Logging.LogLevel.Debug:
                        return _logger.IsDebugEnable;
                    case Microsoft.Extensions.Logging.LogLevel.Information:
                        return _logger.IsDebugEnable;
                    case Microsoft.Extensions.Logging.LogLevel.Warning:
                        return _logger.IsWarnEnable;
                    case Microsoft.Extensions.Logging.LogLevel.Error:
                        return _logger.IsErrorEnable;
                    case Microsoft.Extensions.Logging.LogLevel.Critical:
                        return _logger.IsErrorEnable;
                    case Microsoft.Extensions.Logging.LogLevel.None:
                        return true;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
                }
            }

            public IDisposable BeginScope<TState>(TState state) => default;

            public void Dispose()
            {
            }
        }
    }

    public static class NlogForIDExtMethod
    {
        public static void AddNLog(this Microsoft.Extensions.Logging.ILoggerFactory factory)
        {
            factory.AddProvider(new NLoggerProviderForDI());
        }

        public static void AddNLog(this Microsoft.Extensions.Logging.ILoggerFactory factory, NLogFactory nlogfactory)
        {
            factory.AddProvider(new NLoggerProviderForDI(nlogfactory));
        }

  
    }
}