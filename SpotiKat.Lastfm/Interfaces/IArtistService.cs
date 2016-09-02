using SpotiKat.Lastfm.Entities;

namespace SpotiKat.Lastfm.Interfaces {
    public interface IArtistService {
        SimilarArtistsResponse GetSimilarArtists(string artist);
    }
}