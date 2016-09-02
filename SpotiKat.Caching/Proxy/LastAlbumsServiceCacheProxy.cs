using System.Collections.Generic;
using System.Threading.Tasks;
using SpotiKat.Caching.Interfaces;
using SpotiKat.Caching.Interfaces.Configuration;
using SpotiKat.Entities;
using SpotiKat.Services.Interfaces;

namespace SpotiKat.Caching.Proxy {
    public class LastAlbumsServiceCacheProxy : CacheProxyBase, ILastAlbumService {
        private readonly ISpotiKatCacheConfiguration _cacheConfiguration;
        private readonly ILastAlbumService _lastFeedItemService;

        public LastAlbumsServiceCacheProxy(ILastAlbumService lastFeedItemService,
            ISpotiKatCacheConfiguration cacheConfiguration,
            ICacheService cacheService)
            : base(cacheService) {
            _lastFeedItemService = lastFeedItemService;
            _cacheConfiguration = cacheConfiguration;
        }

        public async Task<IList<Album>> GetFeedItemsAlbumsAsync(FeedItemSource source, int page) {
            var cacheKey = GetCacheKey(string.Format("GetAlbums:source={0}&page={1}", source, page));
            var cacheTimeout = _cacheConfiguration.Timeout;
            return
                await
                    CacheService.GetAsync(cacheKey, cacheTimeout, false,
                        () => _lastFeedItemService.GetFeedItemsAlbumsAsync(source, page));
        }

        private string GetCacheKey(string subKey) {
            return Caching.CacheService.CacheKeyPrefix + typeof (ILastAlbumService).FullName + ":" + subKey;
        }
    }
}