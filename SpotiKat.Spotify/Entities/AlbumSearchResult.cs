using Newtonsoft.Json;

namespace SpotiKat.Spotify.Entities {
    public class AlbumSearchResult {
        [JsonProperty("albums")]
        public Albums Albums { get; set; }
    }
}