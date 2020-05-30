using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kugar.Core.ExtMethod;
using Kugar.Core.Log;
using Newtonsoft.Json;

namespace Kugar.Core
{
    public class NLogger : LoggerBase
    {
        private NLog.Logger _logger = null;
        private object[] emptyArgs = new object[0];

        public NLogger(NLog.Logger logger)
        {
            _logger = logger;
        }


        protected override void DebugInternal(string message, KeyValuePair<string, object>[] extData = null)
        {
            _logger?.Debug(buildString(null, message, extData));
        }

        protected override void TraceInternal(string message, KeyValuePair<string, object>[] extData = null)
        {
            _logger?.Trace(buildString(null, message, extData));
        }

        protected override void WarnInternal(string message, Exception error, KeyValuePair<string, object>[] extData = null)
        {
            _logger?.Warn(buildString(null, message, extData));
        }

        protected override void ErrorInternal(string message, Exception error, KeyValuePair<string, object>[] extData = null)
        {
            _logger?.Error(error, buildString(error, message, extData));
        }

        private string buildString(Exception error, string message, KeyValuePair<string, object>[] extData)
        {
            message = message ?? "";

            var msg = $"message:{message},error={(error == null ? "no error" : JsonConvert.SerializeObject(error))},extData={extData?.Select(x => $"Key={x.Key}|Value={JsonConvert.SerializeObject(x.Value)}")?.JoinToString('\n') ?? "null"}";

            return msg;

        }

        internal NLog.Logger SourceLogger => _logger;
    }


    //public class NLogger : ILogger
    //{
    //    private NLog.Logger _logger = null;

    //    private bool _isTraceEnable = true;

    //    private bool _isWarnEnable = true;

    //    private bool _isErrorEnable = true;

    //    private bool _isDebugEnable = true;

    //    public NLogger(NLog.Logger logger)
    //    {
    //        _logger = logger;
    //    }

    //    #region "Debug"

    //    public void Debug(string message)
    //    {
    //        this.Debug(message, null, null);
    //    }

    //    public void Debug(string message, params object[] args)
    //    {
    //        this.Debug(message, null, args);
    //    }

    //    public void Debug(string message, IFormatProvider formatProvider, params object[] args)
    //    {
    //        if (_isDebugEnable && _logger != null)
    //        {
    //            _logger.Debug(string.Format(formatProvider, message, args));
    //        }
    //    }

    //    #endregion

    //    #region "Trace"

    //    public void Trace(string message)
    //    {
    //        this.Trace(message, null, null);
    //    }

    //    public void Trace(string message, params object[] args)
    //    {
    //        this.Trace(message, null, args);
    //    }

    //    public void Trace(string message, IFormatProvider formatProvider, params object[] args)
    //    {
    //        if (_isTraceEnable && _logger != null)
    //        {
    //            _logger.Trace(string.Format(formatProvider, message, args));
    //        }
    //    }

    //    #endregion

    //    #region "Warn"

    //    public void Warn(string message)
    //    {
    //        this.Warn(message, null, null);
    //    }

    //    public void Warn(string message, params object[] args)
    //    {
    //        this.Warn(message, null, args);
    //    }

    //    public void Warn(string message, IFormatProvider formatProvider, params object[] args)
    //    {
    //        if (_isWarnEnable && _logger != null)
    //        {
    //            _logger.Warn(string.Format(formatProvider, message, args));
    //        }
    //    }
    //    #endregion

    //    #region "Error"

    //    public void Error(string message)
    //    {
    //        this.Error(message, null, null);
    //    }

    //    public void Error(string message, params object[] args)
    //    {
    //        this.Error(message, null, args);
    //    }

    //    public void Error(string message, IFormatProvider formatProvider, params object[] args)
    //    {
    //        if (_isErrorEnable && _logger != null)
    //        {
    //            _logger.Error(string.Format(formatProvider, message, args));
    //        }
    //    }
    //    #endregion


    //    public bool IsDebugEnable
    //    {
    //        get { return _isDebugEnable; }
    //        set { _isDebugEnable = value; }
    //    }

    //    public bool IsTraceEnable
    //    {
    //        get { return _isTraceEnable; }
    //        set { _isTraceEnable = value; }
    //    }

    //    public bool IsWarnEnable
    //    {
    //        get { return _isWarnEnable; }
    //        set { _isWarnEnable = value; }
    //    }

    //    public bool IsErrorEnable
    //    {
    //        get { return _isErrorEnable; }
    //        set { _isErrorEnable = value; }
    //    }
    //}
}