using System;
using SpotiKat.Caching.Interfaces.Configuration;

namespace SpotiKat.Caching.Configuration {
    internal class SpotiKatCacheConfiguration : ISpotiKatCacheConfiguration {
        public TimeSpan Timeout {
            get { return Settings.Default.SpotiKatTimeout; }
        }
    }
}