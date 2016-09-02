using System.Threading.Tasks;
using SpotiKat.Entities;

namespace SpotiKat.Sbwr.Interfaces {
    public interface ISbwrFeedItemService {
        Task<FeedItemsResult> GetFeedItemsByGenreAsync(string genre, int page);
    }
}