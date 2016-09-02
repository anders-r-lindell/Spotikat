namespace SpotiKat.Boomkat.Interfaces.Configuration {
    public interface IBoomkatConfiguration {
        string FeedItemUrlFormat { get; }
        string FeedItemByGenreUrlFormat { get; }
        string WebClientUserAgent { get; }
    }
}