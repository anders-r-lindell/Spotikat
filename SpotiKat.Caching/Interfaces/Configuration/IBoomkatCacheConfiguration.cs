using System;

namespace SpotiKat.Caching.Interfaces.Configuration {
    public interface IBoomkatCacheConfiguration {
        TimeSpan Timeout { get; }
    }
}