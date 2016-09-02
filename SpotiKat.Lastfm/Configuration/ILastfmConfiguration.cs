namespace SpotiKat.Lastfm.Configuration {
    public interface ILastfmConfiguration {
        string ApiKey { get; }
        string Secret { get; }
        string ApiBaseUrl { get; }
        string ArtistGetSimilarMethodUrlParameter { get; }
    }
}