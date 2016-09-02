using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using SpotiKat.Api.ServiceModel.Interfaces;
using SpotiKat.Entities;

namespace SpotiKat.Api.ServiceModel.Response {
    public class GenresResponse : IHasResponseStatus {
        [JsonProperty("genres")]
        public IList<Genre> Genres { get; set; }

        [JsonProperty("responseStatusCode")]
        public HttpStatusCode ResponseStatusCode { get; set; }
    }
}