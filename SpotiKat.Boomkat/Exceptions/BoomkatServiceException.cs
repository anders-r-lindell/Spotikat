using System;
using SpotiKat.Exceptions;

namespace SpotiKat.Boomkat.Exceptions {
    public class BoomkatServiceException : SpotiKatApplicationException {
        public BoomkatServiceException(string message) : base(message) {}

        public BoomkatServiceException(string message, Exception innerException) : base(message, innerException) {}
    }
}