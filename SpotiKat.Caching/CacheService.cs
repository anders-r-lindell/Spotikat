using System;
using System.Collections;
using System.Threading.Tasks;
using SpotiKat.Caching.Interfaces;
using SpotiKat.Interfaces.Caching;

namespace SpotiKat.Caching {
    public class CacheService : ICacheService {
        public static readonly string CacheKeyPrefix = "SpotiKat:";
        private readonly ICache _cache;

        public CacheService(ICache cache) {
            _cache = cache;
        }

        public T Get<T>(string cacheKey, TimeSpan cacheTimeout, bool cacheNullAndEmptyCollection,
            Func<T> delegateFunction) where T : class {
            var obj = _cache.Get(cacheKey);

            if (obj == null) {
                obj = delegateFunction() ?? (object) (new CachedNullValue());
                if (IsCachable(obj, cacheNullAndEmptyCollection)) {
                    _cache.Add(cacheKey, obj, DateTime.Now.Add(cacheTimeout));
                }
            }

            return obj is CachedNullValue ? null : (T) obj;
        }

        public void Remove(string cacheKey) {
            _cache.Remove(cacheKey);
        }

        public async Task<T> GetAsync<T>(string cacheKey, TimeSpan cacheTimeout, bool cacheNullAndEmptyCollection,
            Func<Task<T>> delegateFunction) where T : class {
            var obj = await _cache.GetAsync(cacheKey);
            var foundInCache = obj != null;
            if (obj == null) {
                obj = await delegateFunction() ?? (object) (new CachedNullValue());
                if (IsCachable(obj, cacheNullAndEmptyCollection)) {
                    await _cache.AddAsync(cacheKey, obj, DateTime.Now.Add(cacheTimeout));
                }
            }

            if (obj is CachedNullValue) {
                return null;
            }

            return (T) obj;
        }

        public async Task RemoveAsync(string cacheKey) {
            await _cache.RemoveAsync(cacheKey);
        }

        private bool IsCachable(object obj, bool cacheNullAndEmptyCollection) {
            if (cacheNullAndEmptyCollection) {
                return true;
            }
            if (obj is CachedNullValue) {
                return false;
            }
            var collection = obj as ICollection;
            if (collection != null && collection.Count == 0) {
                return false;
            }
            return true;
        }
    }
}