namespace SpotiKat.Spotify.Interfaces.Configuration {
    public interface ISpotifyConfiguration {
        string ApiBaseUrl { get; }
        string SearchEndpointFormat { get; }
    }
}