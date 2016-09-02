using System;
using SpotiKat.Interfaces.Logging;

namespace SpotiKat.Logging {
    public class NullLogFactory : ILogFactory {
        public ILog GetLogger(Type type) {
            return new NullLogger();
        }
    }
}