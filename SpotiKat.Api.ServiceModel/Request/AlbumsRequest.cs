namespace SpotiKat.Api.ServiceModel.Request {
    public class AlbumsRequest {
        public string Genre { get; set; }
        public int Page { get; set; }
        public FeedItemSource Source { get; set; }
    }
}