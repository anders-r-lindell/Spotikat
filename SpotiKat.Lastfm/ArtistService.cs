using System;
using ServiceStack.Logging;
using SpotiKat.Interfaces;
using SpotiKat.Lastfm.Entities;
using SpotiKat.Lastfm.Exceptions;
using SpotiKat.Lastfm.Interfaces;

namespace SpotiKat.Lastfm {
    public class ArtistService : IArtistService {
        private const string GetSimilarArtistsErrorMessageFormat =
            "Failed to execute get similar artists for artist '{0}': {1}";

        private readonly ILogFactory _logFactory;
        private readonly IRestServiceClient _serviceClient;
        private readonly IUrlBuilder _urlBuilder;

        public ArtistService(IUrlBuilder urlBuilder,
            IRestServiceClient serviceClient,
            ILogFactory logFactory) {
            _urlBuilder = urlBuilder;
            _serviceClient = serviceClient;
            _logFactory = logFactory;
        }

        public SimilarArtistsResponse GetSimilarArtists(string artist) {
            try {
                var similarArtistsResponse =
                    _serviceClient.Get<SimilarArtistsResponse>(_urlBuilder.BuildArtistGetSimilatUrl(artist));
                if (similarArtistsResponse.SimilarArtists == null) {
                    similarArtistsResponse.SimilarArtists = new SimilarArtists {
                        Artists = new Artist[] {}
                    };
                }
                if (similarArtistsResponse.SimilarArtists.Artists == null) {
                    similarArtistsResponse.SimilarArtists.Artists = new Artist[] {};
                }
                return similarArtistsResponse;
            }
            catch (Exception ex) {
                _logFactory.GetLogger(typeof (ArtistService))
                    .ErrorFormat(GetSimilarArtistsErrorMessageFormat, artist, ex.Message);
                throw new LastfmServiceException(
                    string.Format(GetSimilarArtistsErrorMessageFormat, artist, ex.Message), ex);
            }
        }
    }
}