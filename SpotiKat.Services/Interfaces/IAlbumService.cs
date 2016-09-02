using System.Threading.Tasks;
using SpotiKat.Services.Entities;

namespace SpotiKat.Services.Interfaces {
    public interface IAlbumService {
        Task<Albums> GetAlbumsByGenreAsync(FeedItemSource source, string genre, int page);
    }
}