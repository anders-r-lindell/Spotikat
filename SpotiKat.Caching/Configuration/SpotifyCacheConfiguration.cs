using System;
using SpotiKat.Caching.Interfaces.Configuration;

//ncrunch: no coverage start

namespace SpotiKat.Caching.Configuration {
    public class SpotifyCacheConfiguration : ISpotifyCacheConfiguration {
        public TimeSpan Timeout {
            get { return Settings.Default.SpotifyTimeout; }
        }
    }
}

//ncrunch: no coverage end	