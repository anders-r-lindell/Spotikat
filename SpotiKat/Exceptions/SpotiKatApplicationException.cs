using System;

namespace SpotiKat.Exceptions {
    public class SpotiKatApplicationException : Exception {
        public SpotiKatApplicationException(string message) : base(message) {}

        public SpotiKatApplicationException(string message, Exception innerException) : base(message, innerException) {}
    }
}