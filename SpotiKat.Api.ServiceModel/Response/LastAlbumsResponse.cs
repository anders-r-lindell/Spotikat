using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using SpotiKat.Api.ServiceModel.Interfaces;
using SpotiKat.Entities;

namespace SpotiKat.Api.ServiceModel.Response {
    public class LastAlbumsResponse : IHasResponseStatus {
        [JsonProperty("info")]
        public LastAlbumsResponseInfo Info { get; set; }

        [JsonProperty("albums")]
        public IList<Album> Albums { get; set; }

        [JsonProperty("responseStatusCode")]
        public HttpStatusCode ResponseStatusCode { get; set; }
    }
}