using System;
using SpotiKat.Lastfm.Configuration;
using SpotiKat.Lastfm.Interfaces;

namespace SpotiKat.Lastfm {
    public class UrlBuilder : IUrlBuilder {
        private readonly ILastfmConfiguration _lastfmConfiguration;

        public UrlBuilder(ILastfmConfiguration lastfmConfiguration) {
            if (lastfmConfiguration == null) {
                throw new ArgumentNullException("lastfmConfiguration");
            }

            _lastfmConfiguration = lastfmConfiguration;
        }

        public string BuildArtistGetSimilatUrl(string artist) {
            if (artist == null) {
                throw new ArgumentNullException("artist");
            }

            var urlParameter = string.Format(_lastfmConfiguration.ArtistGetSimilarMethodUrlParameter, artist);
            return string.Format(GetBaseUrlWithApiKey(), urlParameter);
        }

        private string GetBaseUrlWithApiKey() {
            return string.Format(_lastfmConfiguration.ApiBaseUrl, _lastfmConfiguration.ApiKey, "{0}");
        }
    }
}