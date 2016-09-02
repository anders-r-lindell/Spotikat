using System.Threading.Tasks;
using SpotiKat.Entities;

namespace SpotiKat.Services.Interfaces {
    public interface ISpotifyAlbumService {
        Task<Album> FindAlbumAsync(string artistName, string albumName);
    }
}