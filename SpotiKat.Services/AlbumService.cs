using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpotiKat.Boomkat.Interfaces;
using SpotiKat.Entities;
using SpotiKat.NewRelic.Interfaces;
using SpotiKat.Sbwr.Interfaces;
using SpotiKat.Services.Comparer;
using SpotiKat.Services.Entities;
using SpotiKat.Services.Interfaces;
using SpotiKat.Spotify.Exceptions;

namespace SpotiKat.Services {
    public class AlbumService : IAlbumService {
        private readonly IBoomkatFeedItemService _boomkatBoomkatFeedItemService;
        private readonly INewRelicTransactionManager _newRelicTransactionManager;
        private readonly ISbwrFeedItemService _sbwrFeedItemService;
        private readonly ISpotifyService _spotifyService;

        public AlbumService(IBoomkatFeedItemService boomkatBoomkatFeedItemService,
            ISpotifyService spotifyService, ISbwrFeedItemService sbwrFeedItemService,
            INewRelicTransactionManager newRelicTransactionManager) {
            _boomkatBoomkatFeedItemService = boomkatBoomkatFeedItemService;
            _spotifyService = spotifyService;
            _sbwrFeedItemService = sbwrFeedItemService;
            _newRelicTransactionManager = newRelicTransactionManager;
        }

        public async Task<Albums> GetAlbumsByGenreAsync(FeedItemSource source, string genre, int page) {
            var albums = await GetAlbumsAsync(source, genre, page);
            // Pre cache next page
            Task.Run(() => GetAlbumsAsync(source, genre, page + 1));
            return albums;
        }

        private async Task<Albums> GetAlbumsAsync(FeedItemSource source, string genre, int page) {
            FeedItemsResult feedItemsResult;

            if (source == FeedItemSource.Boomkat) {
                feedItemsResult = (genre != "0")
                    ? await _boomkatBoomkatFeedItemService.GetFeedItemsByGenreAsync(genre, page)
                    : await _boomkatBoomkatFeedItemService.GetFeedItemsAsync(page);
            }
            else {
                feedItemsResult = await _sbwrFeedItemService.GetFeedItemsByGenreAsync(genre, page);
            }

            var albums = new Albums {
                Items = new List<Album>(),
                Pages = feedItemsResult.Pages.Select(
                    x => (x != "...")
                        ? new Page { Text = x, Value = int.Parse(x), IsCurrent = int.Parse(x) == page}
                        : new Page {Text = x, Value = 0, IsDisabled = true}).ToList()
            };

            if (page > 1) {
                albums.Pages.Insert(0, new Page {Text = "‹ previous", Value = page - 1});
            }

            if (albums.Pages[albums.Pages.Count - 1].Text == "..." || albums.Pages[albums.Pages.Count - 1].Value != page) {
                albums.Pages.Add(new Page {Text = "next ›", Value = page + 1});
            }

            foreach (var feedItem in feedItemsResult.Items) {
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

                album.ImageUrl = album.ImageUrl;
                if (!albums.Items.Contains(album, new AlbumComparer())) {
                    albums.Items.Add(album);
                }
            }

            return albums;
        }
    }
}