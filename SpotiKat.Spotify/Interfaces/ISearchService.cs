using System.Threading.Tasks;
using SpotiKat.Spotify.Entities;

namespace SpotiKat.Spotify.Interfaces {
    public interface ISearchService {
        Task<AlbumSearchResult> AlbumSearchAsync(string artist, string album);
    }
}