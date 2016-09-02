using Newtonsoft.Json;

namespace SpotiKat.Entities {
    public class Album {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("artist")]
        public string Artist { get; set; }
    }
}