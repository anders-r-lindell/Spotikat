using System;

namespace SpotiKat.Interfaces.Logging {
    public interface ILog {
        void Debug(string message);
        void Debug(string message, Exception exception);
        void DebugFormat(string message, params object[] args);
        void DebugFormat(string message, Exception exception, params object[] args);
        void Error(string message);
        void Error(string message, Exception exception);
        void ErrorFormat(string message, params object[] args);
        void ErrorFormat(string message, Exception exception, params object[] args);
    }
}