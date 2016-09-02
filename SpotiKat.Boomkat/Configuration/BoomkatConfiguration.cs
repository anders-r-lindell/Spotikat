//ncrunch: no coverage start

using SpotiKat.Boomkat.Interfaces.Configuration;

namespace SpotiKat.Boomkat.Configuration {
    public class BoomkatConfiguration : IBoomkatConfiguration {
        public string FeedItemUrlFormat {
            get { return Settings.Default.FeedItemUrlFormat; }
        }

        public string FeedItemByGenreUrlFormat {
            get { return Settings.Default.FeedItemByGenreUrlFormat; }
        }

        public string WebClientUserAgent {
            get { return Settings.Default.WebClientUserAgent; }
        }
    }
}

//ncrunch: no coverage end