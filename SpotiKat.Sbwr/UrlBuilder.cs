using SpotiKat.Sbwr.interfaces.Configuration;
using SpotiKat.Sbwr.Interfaces;

namespace SpotiKat.Sbwr {
    public class UrlBuilder : IUrlBuilder {
        private readonly ISbwrConfiguration _sbwrConfiguration;

        public UrlBuilder(ISbwrConfiguration sbwrConfiguration) {
            _sbwrConfiguration = sbwrConfiguration;
        }

        public string BuildFeedItemByGenreUrl(string genre, int page) {
            return string.Format(_sbwrConfiguration.FeedItemByGenreUrlFormat, genre, page);
        }
    }
}