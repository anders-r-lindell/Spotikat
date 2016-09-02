using Newtonsoft.Json;

namespace SpotiKat.Api.ServiceModel.Response {
    public class LastAlbumsResponseInfo {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("source")]
        public FeedItemSource Source { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}