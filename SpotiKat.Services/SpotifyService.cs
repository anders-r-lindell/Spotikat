using System.Threading.Tasks;
using SpotiKat.Entities;
using SpotiKat.Services.Interfaces;

namespace SpotiKat.Services {
    public class SpotifyService : ISpotifyService {
        private readonly ISpotifyAlbumService _spotifyAlbumService;

        public SpotifyService(ISpotifyAlbumService spotifyAlbumService) {
            _spotifyAlbumService = spotifyAlbumService;
        }

        public async Task<Album> FindArtistAlbumAsync(string artistName, string albumName) {
            return await _spotifyAlbumService.FindAlbumAsync(artistName, albumName);
        }
    }
}