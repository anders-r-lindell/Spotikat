using Newtonsoft.Json;

namespace SpotiKat.Entities {
    public class Page {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("isDisabled")]
        public bool IsDisabled { get; set; }
        
        [JsonProperty("isCurrent")]
        public bool IsCurrent { get; set; }
    }
}