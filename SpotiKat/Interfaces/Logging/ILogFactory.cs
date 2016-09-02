using System;

namespace SpotiKat.Interfaces.Logging {
    public interface ILogFactory {
        ILog GetLogger(Type type);
    }
}