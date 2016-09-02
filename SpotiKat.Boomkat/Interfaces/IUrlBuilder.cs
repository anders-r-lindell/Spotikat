namespace SpotiKat.Boomkat.Interfaces {
    public interface IUrlBuilder {
        string BuildFeedItemUrl(int page);
        string BuildFeedItemByGenreUrl(string genre, int page);
    }
}