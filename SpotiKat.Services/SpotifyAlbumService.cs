using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpotiKat.Entities;
using SpotiKat.Services.Interfaces;
using SpotiKat.Spotify.Interfaces;
using SpotifyAlbum = SpotiKat.Spotify.Entities.Album;

namespace SpotiKat.Services {
    public class SpotifyAlbumService : ISpotifyAlbumService {
        private readonly ISearchService _searchService;

        public SpotifyAlbumService(ISearchService searchService) {
            _searchService = searchService;
        }

        public async Task<Album> FindAlbumAsync(string artistName, string albumName) {
            var albumSearchResult = await _searchService.AlbumSearchAsync(artistName, albumName);

            if (albumSearchResult == null || albumSearchResult.Albums == null ||
                albumSearchResult.Albums.Total == 0) {
                return null;
            }

            if (albumSearchResult.Albums.Total == 1) {
                return CreateAlbum(artistName, albumName, albumSearchResult.Albums.Items[0]);
            }

            return GetBestAlbumMatch(artistName, albumName, albumSearchResult.Albums.Items);
        }

        private Album GetBestAlbumMatch(string artistName, string albumName, IList<SpotifyAlbum> albums) {
            var bestMatchAlbum = TryGetAlbumWithExactMatch(albumName, albums);
            if (bestMatchAlbum != null) {
                return CreateAlbum(artistName, albumName, bestMatchAlbum);
            }

            bestMatchAlbum = TryGetAlbumWithindexOfMatch(albumName, albums);
            if (bestMatchAlbum != null) {
                return CreateAlbum(artistName, albumName, bestMatchAlbum);
            }

            bestMatchAlbum = GetAlbumWithMostAvailableMarkets(albums);
            return CreateAlbum(artistName, albumName, bestMatchAlbum);
        }

        private SpotifyAlbum TryGetAlbumWithExactMatch(string albumName, IList<SpotifyAlbum> albums) {
            var albumTypeMatches =
                albums.Where(x => x.Type == "album" && IsExactAlbumNameMatch(x.Name, albumName)).ToList();
            if (albumTypeMatches.Any()) {
                return GetAlbumWithMostAvailableMarkets(albumTypeMatches);
            }

            var otherTypesMatches =
                albums.Where(x => x.Type != "album" && IsExactAlbumNameMatch(x.Name, albumName)).ToList();
            if (otherTypesMatches.Any()) {
                return GetAlbumWithMostAvailableMarkets(otherTypesMatches);
            }

            return null;
        }

        private SpotifyAlbum TryGetAlbumWithindexOfMatch(string albumName, IList<SpotifyAlbum> albums) {
            var albumTypeMatches =
                albums.Where(x => x.Type == "album" && IsIndexOfAlbumNameMatch(x.Name, albumName)).ToList();
            if (albumTypeMatches.Any()) {
                return GetAlbumWithMostAvailableMarkets(albumTypeMatches);
            }

            var otherTypesMatches =
                albums.Where(x => x.Type != "album" && IsIndexOfAlbumNameMatch(x.Name, albumName)).ToList();
            if (otherTypesMatches.Any()) {
                return GetAlbumWithMostAvailableMarkets(otherTypesMatches);
            }

            return null;
        }

        private SpotifyAlbum GetAlbumWithMostAvailableMarkets(IList<SpotifyAlbum> albums) {
            var albumWithMostAvailableMarkets = albums[0];

            for (var i = 1; i < albums.Count; i++) {
                if (albums[i].AvailableMarkets.Count() > albumWithMostAvailableMarkets.AvailableMarkets.Count()) {
                    albumWithMostAvailableMarkets = albums[i];
                }
            }

            return albumWithMostAvailableMarkets;
        }

        private bool IsExactAlbumNameMatch(string albumName1, string albumName2) {
            return albumName1.Equals(albumName2, StringComparison.InvariantCultureIgnoreCase);
        }

        private bool IsIndexOfAlbumNameMatch(string albumName1, string albumName2) {
            if (albumName1.Length <= 5 || albumName2.Length <= 5) {
                return false;
            }
            return albumName1.IndexOf(albumName2, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                   albumName2.IndexOf(albumName1, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        private Album CreateAlbum(string artistName, string albumName, SpotifyAlbum album) {
            return new Album {
                Artist = artistName,
                Href = album.Uri,
                Name = albumName,
                ImageUrl = GetAlbumImageUrl(album)
            };
        }

        private string GetAlbumImageUrl(SpotifyAlbum album) {
            var image = album.Images.FirstOrDefault(x => x.Width == 640 || x.Height == 640) ??
                        album.Images.FirstOrDefault(x => x.Width == 300 || x.Height == 300);
            return (image != null) ? image.Url : "/assets/img/album.png";
        }
    }
}