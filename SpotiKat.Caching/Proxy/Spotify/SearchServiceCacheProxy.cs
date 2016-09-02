using System.Threading.Tasks;
using SpotiKat.Caching.Interfaces;
using SpotiKat.Caching.Interfaces.Configuration;
using SpotiKat.Spotify.Entities;
using SpotiKat.Spotify.Interfaces;

namespace SpotiKat.Caching.Proxy.Spotify {
    public class SearchServiceCacheProxy : CacheProxyBase, ISearchService {
        private readonly ISpotifyCacheConfiguration _cacheConfiguration;
        private readonly ISearchService _searchService;

        public SearchServiceCacheProxy(ISearchService searchService,
            ISpotifyCacheConfiguration cacheConfiguration,
            ICacheService cacheService) : base(cacheService) {
            _searchService = searchService;
            _cacheConfiguration = cacheConfiguration;
        }

        public async Task<AlbumSearchResult> AlbumSearchAsync(string artist, string album) {
            var cacheKey = GetCacheKey(string.Format("AlbumSearch:artist={0}&album={1}", artist, album));
            var cacheTimeout = _cacheConfiguration.Timeout;
            return
                await
                    CacheService.GetAsync(cacheKey, cacheTimeout, true,
                        () => _searchService.AlbumSearchAsync(artist, album));
        }

        private string GetCacheKey(string subKey) {
            return Caching.CacheService.CacheKeyPrefix + typeof (ISearchService).FullName + ":" + subKey;
        }
    }
}