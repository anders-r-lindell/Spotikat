using System;
using System.Threading.Tasks;
using SpotiKat.Interfaces.Logging;
using SpotiKat.Interfaces.Net.Http;
using SpotiKat.Spotify.Entities;
using SpotiKat.Spotify.Exceptions;
using SpotiKat.Spotify.Interfaces;

namespace SpotiKat.Spotify {
    public class SearchService : ISearchService {
        private const string AlbumSearchErrorMessageFormat =
            "Failed to execute album search for artist '{0}', album '{1}': {2}";

        private readonly ILogFactory _logFactory;
        private readonly IJsonServiceClient _serviceClient;
        private readonly IUrlBuilder _urlBuilder;

        public SearchService(IUrlBuilder urlBuilder,
            IJsonServiceClient serviceClient,
            ILogFactory logFactory) {
            _urlBuilder = urlBuilder;
            _serviceClient = serviceClient;
            _logFactory = logFactory;
        }

        public async Task<AlbumSearchResult> AlbumSearchAsync(string artist, string album) {
            try {
                return await _serviceClient.GetAsync<AlbumSearchResult>(_urlBuilder.BuildAlbumSearchUrl(artist, album));
            }
            catch (Exception ex) {
                _logFactory.GetLogger(typeof (SearchService))
                    .ErrorFormat(AlbumSearchErrorMessageFormat, artist, album, ex.Message);
                throw new SpotifyServiceException(
                    string.Format(AlbumSearchErrorMessageFormat, artist, album, ex.Message), ex);
            }
        }
    }
}