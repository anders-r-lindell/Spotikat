using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SpotiKat.Lastfm.Entities {
    [DataContract]
    public class SimilarArtists {
        [DataMember(Name = "artist")]
        public IList<Artist> Artists { get; set; }
    }
}