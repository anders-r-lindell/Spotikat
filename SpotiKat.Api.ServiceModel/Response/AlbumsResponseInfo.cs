using Newtonsoft.Json;

namespace SpotiKat.Api.ServiceModel.Response {
    public class AlbumsResponseInfo {
        [JsonProperty("genre")]
        public string Genre { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("source")]
        public FeedItemSource Source { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}