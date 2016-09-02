using System;
using SpotiKat.Exceptions;

namespace SpotiKat.Lastfm.Exceptions {
    public class LastfmServiceException : SpotiKatApplicationException {
        public LastfmServiceException(string message) : base(message) {
        }

        public LastfmServiceException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}