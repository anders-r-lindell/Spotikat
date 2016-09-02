using System;
using SpotiKat.Interfaces.Logging;

namespace SpotiKat.Web.Logging
{
    public class NLogFactory : ILogFactory
    {
        public ILog GetLogger(Type type) {
            return new NLogAdapter(type);
        }
    }
}