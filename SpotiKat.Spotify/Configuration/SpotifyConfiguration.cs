//ncrunch: no coverage start

using SpotiKat.Spotify.Interfaces.Configuration;

namespace SpotiKat.Spotify.Configuration {
    public class SpotifyConfiguration : ISpotifyConfiguration {
        public string ApiBaseUrl {
            get { return Settings.Default.ApiBaseUrl; }
        }

        public string SearchEndpointFormat {
            get { return Settings.Default.SearchEndpointFormat; }
        }
    }
}

//ncrunch: no coverage end