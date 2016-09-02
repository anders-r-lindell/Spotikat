using System.Collections.Generic;

namespace SpotiKat.Sbwr.interfaces.Configuration {
    public interface ISbwrConfiguration {
        string FeedItemByGenreUrlFormat { get; }
        string WebClientUserAgent { get; }
        IList<string> Genres { get; }
    }
}