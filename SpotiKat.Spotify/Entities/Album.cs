using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpotiKat.Spotify.Entities {
    public class Album {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("album_type")]
        public string Type { get; set; }

        [JsonProperty("available_markets")]
        public IList<string> AvailableMarkets { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("images")]
        public IList<Image> Images { get; set; }
    }
}