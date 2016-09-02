using SpotiKat.Boomkat.Interfaces;
using SpotiKat.Boomkat.Interfaces.Configuration;

namespace SpotiKat.Boomkat {
    public class UrlBuilder : IUrlBuilder {
        private readonly IBoomkatConfiguration _boomkatConfiguration;

        public UrlBuilder(IBoomkatConfiguration boomkatConfiguration) {
            _boomkatConfiguration = boomkatConfiguration;
        }

        public string BuildFeedItemUrl(int page) {
            return string.Format(_boomkatConfiguration.FeedItemUrlFormat, page);
        }

        public string BuildFeedItemByGenreUrl(string genre, int page) {
            return string.Format(_boomkatConfiguration.FeedItemByGenreUrlFormat, page, genre);
        }
    }
}