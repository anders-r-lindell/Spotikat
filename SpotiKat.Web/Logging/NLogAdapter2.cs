using System;
using NLog;
using SpotiKat.Interfaces.Logging;

namespace SpotiKat.Web.Logging {
    public class NLogAdapter : ILog {
        private readonly Logger _logger;

        public NLogAdapter(Type type) {
            _logger = LogManager.GetLogger(type.FullName);
        }

        public void Debug(string message) {
            try {
                _logger.Debug(message);
            }
            catch {
                // Not much to do...
            }
        }

        public void Debug(string message, Exception exception) {
            try {
                _logger.Debug(exception, message);
            }
            catch {
                // Not much to do...
            }
        }

        public void DebugFormat(string message, params object[] args) {
            try {
                _logger.Debug(message, args);
            }
            catch {
                // Not much to do...
            }
        }

        public void DebugFormat(string message, Exception exception, params object[] args) {
            try
            {
                _logger.Debug(exception, message, args);
            }
            catch
            {
                // Not much to do...
            }
        }

        public void Error(string message) {
            try {
                _logger.Error(message);
            }
            catch {
                // Not much to do...
            }
        }

        public void Error(string message, Exception exception) {
            try {
                _logger.Error(exception, message);
            }
            catch {
                // Not much to do...
            }
        }

        public void ErrorFormat(string message, params object[] args) {
            try {
                _logger.Error(message, args);
            }
            catch {
                // Not much to do...
            }
        }

        public void ErrorFormat(string message, Exception exception, params object[] args) {
            try
            {
                _logger.Error(exception, message, args);
            }
            catch
            {
                // Not much to do...
            }
        }
    }
}