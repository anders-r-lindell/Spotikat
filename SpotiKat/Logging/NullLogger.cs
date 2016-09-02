using System;
using SpotiKat.Interfaces.Logging;

namespace SpotiKat.Logging {
    public class NullLogger : ILog {
        public bool IsDebugEnabled {
            get { return false; }
        }

        public void Debug(string message, Exception exception) {}
        public void Debug(string message) {}
        public void DebugFormat(string message, params object[] args) {}
        public void DebugFormat(string message, Exception exception, params object[] args) {}
        public void Error(string message, Exception exception) {}
        public void Error(string message) {}
        public void ErrorFormat(string message, params object[] args) {}
        public void ErrorFormat(string message, Exception exception, params object[] args) {}
    }
}