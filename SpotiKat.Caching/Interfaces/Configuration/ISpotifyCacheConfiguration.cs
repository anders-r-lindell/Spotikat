using System;

namespace SpotiKat.Caching.Interfaces.Configuration {
    public interface ISpotifyCacheConfiguration {
        TimeSpan Timeout { get; }
    }
}