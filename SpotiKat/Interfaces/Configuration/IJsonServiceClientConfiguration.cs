namespace SpotiKat.Interfaces.Configuration {
    public interface IJsonServiceClientConfiguration {
        int MaxNumberOfRetries { get; }
        int SlowDownFactor { get; }
    }
}