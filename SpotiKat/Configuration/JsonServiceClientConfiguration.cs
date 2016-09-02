using SpotiKat.Interfaces.Configuration;

namespace SpotiKat.Configuration {
    public class JsonServiceClientConfiguration : IJsonServiceClientConfiguration {
        public int MaxNumberOfRetries {
            get { return Settings.Default.JsonServiceClientMaxNumberOfRetries; }
        }

        public int SlowDownFactor {
            get { return Settings.Default.JsonServiceSlowDownFactor; }
        }
    }
}