namespace SpotiKat.Lastfm.Configuration {
    public class LastfmConfiguration : ILastfmConfiguration {
        public string ApiKey {
            get { return Settings.Default.ApiKey; }
        }

        public string Secret {
            get { return Settings.Default.Secret; }
        }

        public string ApiBaseUrl {
            get { return Settings.Default.ApiBaseUrl; }
        }

        public string ArtistGetSimilarMethodUrlParameter {
            get { return Settings.Default.ArtistGetSimilarMethodUrlParameter; }
        }
    }
}