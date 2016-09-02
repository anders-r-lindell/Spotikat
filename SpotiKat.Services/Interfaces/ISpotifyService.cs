using System.Threading.Tasks;
using SpotiKat.Entities;

namespace SpotiKat.Services.Interfaces {
    public interface ISpotifyService {
        Task<Album> FindArtistAlbumAsync(string artistName, string albumName);
    }
}