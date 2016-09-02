using System.Collections.Generic;
using SpotiKat.Sbwr.interfaces.Configuration;

namespace SpotiKat.Sbwr.Configuration {
    public class SbwrConfiguration : ISbwrConfiguration {
        public string FeedItemByGenreUrlFormat {
            get { return Settings.Default.FeedItemByGenreUrlFormat; }
        }

        public string WebClientUserAgent {
            get { return Settings.Default.WebClientUserAgent; }
        }

        public IList<string> Genres {
            get { return Settings.Default.Genres.Split(';'); }
        }
    }
}