using System.Runtime.Serialization;

namespace SpotiKat.Lastfm.Entities {
    [DataContract]
    public class Artist {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}