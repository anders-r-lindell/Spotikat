using SpotiKat.Caching.Interfaces;

namespace SpotiKat.Caching.Proxy {
    public abstract class CacheProxyBase {
        protected readonly ICacheService CacheService;

        protected CacheProxyBase(ICacheService cacheService) {
            CacheService = cacheService;
        }
    }
}