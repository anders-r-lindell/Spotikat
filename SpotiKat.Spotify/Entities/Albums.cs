using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpotiKat.Spotify.Entities {
    public class Albums {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("items")]
        public IList<Album> Items { get; set; }
    }
}