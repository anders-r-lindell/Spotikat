using System;
using SpotiKat.Caching.Interfaces.Configuration;

//ncrunch: no coverage start

namespace SpotiKat.Caching.Configuration {
    public class BoomkatCacheConfiguration : IBoomkatCacheConfiguration {
        public TimeSpan Timeout {
            get { return Settings.Default.BoomkatTimeout; }
        }
    }
}

//ncrunch: no coverage end