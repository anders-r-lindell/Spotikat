using System.Threading.Tasks;
using SpotiKat.Boomkat.Interfaces;
using SpotiKat.Caching.Interfaces;
using SpotiKat.Caching.Interfaces.Configuration;
using SpotiKat.Entities;

namespace SpotiKat.Caching.Proxy.Boomkat {
    public class BoomkatFeedItemServiceCacheProxy : CacheProxyBase, IBoomkatFeedItemService {
        private readonly IBoomkatFeedItemService _boomkatFeedItemService;
        private readonly IBoomkatCacheConfiguration _cacheConfiguration;

        public BoomkatFeedItemServiceCacheProxy(IBoomkatFeedItemService boomkatFeedItemService,
            IBoomkatCacheConfiguration cacheConfiguration,
            ICacheService cacheService)
            : base(cacheService) {
            _boomkatFeedItemService = boomkatFeedItemService;
            _cacheConfiguration = cacheConfiguration;
        }

        public async Task<FeedItemsResult> GetFeedItemsAsync(int page) {
            var cacheKey = GetCacheKey(string.Format("GetFeedItems:page={0}", page));
            var cacheTimeout = _cacheConfiguration.Timeout;
            return
                await
                    CacheService.GetAsync(cacheKey, cacheTimeout, true,
                        () => _boomkatFeedItemService.GetFeedItemsAsync(page));
        }

        public async Task<FeedItemsResult> GetFeedItemsByGenreAsync(string genre, int page) {
            var cacheKey = GetCacheKey(string.Format("GetFeedItemsByGenre:genre={0}&page={1}", genre, page));
            var cacheTimeout = _cacheConfiguration.Timeout;
            return await CacheService.GetAsync(cacheKey, cacheTimeout, true,
                () => _boomkatFeedItemService.GetFeedItemsByGenreAsync(genre, page));
        }

        private string GetCacheKey(string subKey) {
            return Caching.CacheService.CacheKeyPrefix + typeof (IBoomkatFeedItemService).FullName + ":" + subKey;
        }
    }
}