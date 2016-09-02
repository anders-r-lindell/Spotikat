﻿using Newtonsoft.Json;

namespace SpotiKat.Spotify.Entities {
    public class Image {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}