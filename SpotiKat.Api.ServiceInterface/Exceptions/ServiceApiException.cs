using System;
using System.Net;
using SpotiKat.Exceptions;

namespace SpotiKat.Api.ServiceInterface.Exceptions {
    public class ServiceApiException : SpotiKatApplicationException {
        public const string InvalidValueErrorMessageFormat = "Request has invalid value for parameter {0}: {1}";

        public ServiceApiException(string message) : base(message) {}

        public ServiceApiException(HttpStatusCode statusCode, string message) : base(message) {
            StatusCode = statusCode;
        }

        public ServiceApiException(HttpStatusCode statusCode, string message, Exception innerException)
            : base(message, innerException) {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; set; }
    }
}