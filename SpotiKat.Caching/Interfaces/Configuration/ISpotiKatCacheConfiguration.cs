using System;

namespace SpotiKat.Caching.Interfaces.Configuration {
    public interface ISpotiKatCacheConfiguration {
        TimeSpan Timeout { get; }
    }
}