using System.Collections.Generic;
using System.Threading.Tasks;
using SpotiKat.Entities;

namespace SpotiKat.Services.Interfaces {
    public interface ILastAlbumService {
        Task<IList<Album>> GetFeedItemsAlbumsAsync(FeedItemSource source, int page);
    }
}