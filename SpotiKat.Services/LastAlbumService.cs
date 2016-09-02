using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpotiKat.Boomkat.Interfaces;
using SpotiKat.Entities;
using SpotiKat.NewRelic.Interfaces;
using SpotiKat.Sbwr.interfaces.Configuration;
using SpotiKat.Sbwr.Interfaces;
using SpotiKat.Services.Interfaces;
using SpotiKat.Spotify.Exceptions;

namespace SpotiKat.Services {
    public class LastAlbumService : ILastAlbumService {
        private const int NumberOfAlbums = 50;
        private const int BoomkatMaxFeedPageCount = 20;
        private const int SbwrMaxFeedPageCount = 75;
        private readonly IBoomkatFeedItemService _boomkatFeedItemService;
        private readonly INewRelicTransactionManager _newRelicTransactionManager;
        private readonly ISbwrConfiguration _sbwrConfiguration;
        private readonly ISbwrFeedItemService _sbwrFeedItemService;
        private readonly ISpotifyService _spotifyService;

        public LastAlbumService(IBoomkatFeedItemService boomkatFeedItemService,
            ISbwrFeedItemService sbwrFeedItemService, ISpotifyService spotifyService,
            ISbwrConfiguration sbwrConfiguration, INewRelicTransactionManager newRelicTransactionManager) {
            _boomkatFeedItemService = boomkatFeedItemService;
            _spotifyService = spotifyService;
            _sbwrConfiguration = sbwrConfiguration;
            _newRelicTransactionManager = newRelicTransactionManager;
            _sbwrFeedItemService = sbwrFeedItemService;
        }

        public async Task<IList<Album>> GetFeedItemsAlbumsAsync(FeedItemSource source, int page) {
            if (source == FeedItemSource.Boomkat) {
                return await GetBoomkatAlbumsAsync();
            }

            return await GetSbwrAlbumsAsync();
        }

        private async Task<IList<Album>> GetSbwrAlbumsAsync() {
            var albums = new List<Album>();

            var feedPage = 1;
            while (feedPage < SbwrMaxFeedPageCount && albums.Count < NumberOfAlbums) {
                foreach (var genre in _sbwrConfiguration.Genres) {
                    var feedItems = await _sbwrFeedItemService.GetFeedItemsByGenreAsync(genre, feedPage);
                    await GetFeedItemsAlbumsAsync(feedItems, albums);
                    if (albums.Count == NumberOfAlbums) {
                        break;
                    }
                    feedPage++;
                }
            }

            return albums;
        }

        private async Task<IList<Album>> GetBoomkatAlbumsAsync() {
            var albums = new List<Album>();

            var feedPage = 1;
            while (feedPage < BoomkatMaxFeedPageCount && albums.Count < NumberOfAlbums) {
                var feedItems = await _boomkatFeedItemService.GetFeedItemsAsync(feedPage);
                await GetFeedItemsAlbumsAsync(feedItems, albums);
                if (albums.Count == NumberOfAlbums) {
                    break;
                }
                feedPage++;
            }

            return albums;
        }

        private async Task GetFeedItemsAlbumsAsync(FeedItemsResult feedItems, IList<Album> albums) {
            foreach (var feedItem in feedItems.Items) {
                var album = (Album) null;

                try {
                    album = await _spotifyService.FindArtistAlbumAsync(feedItem.Artist, feedItem.Album);
                }
                catch (SpotifyServiceException ssex) {
                    _newRelicTransactionManager.NoticeError(ssex);
                }

                if (album == null) {
                    continue;
                }
                if (
                    albums.FirstOrDefault(
                        a => a.Href.Equals(album.Href, StringComparison.InvariantCultureIgnoreCase)) != null) {
                    continue;
                }

                album.ImageUrl = album.ImageUrl;
                albums.Add(album);

                if (albums.Count == NumberOfAlbums) {
                    break;
                }
            }
        }
    }
}