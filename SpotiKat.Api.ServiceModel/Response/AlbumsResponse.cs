using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using SpotiKat.Api.ServiceModel.Interfaces;
using SpotiKat.Entities;

namespace SpotiKat.Api.ServiceModel.Response {
    public class AlbumsResponse : IHasResponseStatus {
        [JsonProperty("info")]
        public AlbumsResponseInfo Info { get; set; }

        [JsonProperty("albums")]
        public IList<Album> Albums { get; set; }

        [JsonProperty("pages")]
        public IList<Page> Pages { get; set; }

        [JsonProperty("responseStatusCode")]
        public HttpStatusCode ResponseStatusCode { get; set; }
    }
}