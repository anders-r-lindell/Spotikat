using SpotiKat.Spotify.Interfaces;
using SpotiKat.Spotify.Interfaces.Configuration;

namespace SpotiKat.Spotify {
    public class UrlBuilder : IUrlBuilder {
        private readonly ISearchQueryEncoder _searchQueryEncoder;
        private readonly ISpotifyConfiguration _spotifyConfiguration;

        public UrlBuilder(ISpotifyConfiguration spotifyConfiguration, ISearchQueryEncoder searchQueryEncoder) {
            _spotifyConfiguration = spotifyConfiguration;
            _searchQueryEncoder = searchQueryEncoder;
        }

        public string BuildAlbumSearchUrl(string artist, string album) {
            var albumSearchEndpoint = string.Format(_spotifyConfiguration.SearchEndpointFormat,
                _searchQueryEncoder.Encode(string.Format("artist:{0} AND album:{1}", artist, album)),
                "album");
            return string.Format("{0}{1}", _spotifyConfiguration.ApiBaseUrl, albumSearchEndpoint);
        }
    }
}