using System;
using SpotiKat.Exceptions;

namespace SpotiKat.Sbwr.Exceptions {
    public class SbwrServiceException : SpotiKatApplicationException {
        public SbwrServiceException(string message) : base(message) {}

        public SbwrServiceException(string message, Exception innerException)
            : base(message, innerException) {}
    }
}