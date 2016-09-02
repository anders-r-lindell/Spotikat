using System.Threading.Tasks;
using SpotiKat.Entities;

namespace SpotiKat.Boomkat.Interfaces {
    public interface IBoomkatFeedItemService {
        Task<FeedItemsResult> GetFeedItemsAsync(int page);
        Task<FeedItemsResult> GetFeedItemsByGenreAsync(string genre, int page);
    }
}