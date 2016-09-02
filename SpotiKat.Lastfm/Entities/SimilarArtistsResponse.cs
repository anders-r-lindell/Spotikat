using System.Runtime.Serialization;

namespace SpotiKat.Lastfm.Entities {
    [DataContract]
    public class SimilarArtistsResponse {
        [DataMember(Name = "similarartists")]
        public SimilarArtists SimilarArtists { get; set; }
    }
}