using Newtonsoft.Json;

namespace SpotiKat.Entities {
    public class Genre {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("source")]
        public FeedItemSource Source { get; set; }

        [JsonProperty("isLastAlbumRoute")]
        public bool IsLastAlbumRoute { get; set; }
    }
}