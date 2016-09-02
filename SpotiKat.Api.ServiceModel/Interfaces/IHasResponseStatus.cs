using System.Net;

namespace SpotiKat.Api.ServiceModel.Interfaces {
    public interface IHasResponseStatus {
        HttpStatusCode ResponseStatusCode { get; set; }
    }
}