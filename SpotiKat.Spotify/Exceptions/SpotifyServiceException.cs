using System;
using SpotiKat.Exceptions;

namespace SpotiKat.Spotify.Exceptions {
    public class SpotifyServiceException : SpotiKatApplicationException {
        public SpotifyServiceException(string message) : base(message) {}

        public SpotifyServiceException(string message, Exception innerException) : base(message, innerException) {}
    }
}