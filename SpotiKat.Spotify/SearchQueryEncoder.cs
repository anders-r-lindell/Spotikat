using SpotiKat.Spotify.Interfaces;

namespace SpotiKat.Spotify {
    public class SearchQueryEncoder : ISearchQueryEncoder {
        public string Encode(string searchQuery) {
            return searchQuery.Replace(" ", "+");
        }
    }
}