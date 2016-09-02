using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using SpotiKat.Interfaces.Caching;

//ncrunch: no coverage start

namespace SpotiKat.Caching {
    public class HttpRuntimeCache : ICache {
        public object Get(string cacheKey) {
            return HttpRuntime.Cache.Get(cacheKey);
        }

        public void Add(string cacheKey,
            object obj,
            DateTime absoluteExpiration) {
            HttpRuntime.Cache.Add(cacheKey, obj, null, absoluteExpiration,
                Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        public void Remove(string cacheKey) {
            HttpRuntime.Cache.Remove(cacheKey);
        }

        public Task<object> GetAsync(string cacheKey) {
            var task = Task.Run(() => Get(cacheKey));
            task.Wait();
            return task;
        }

        public Task AddAsync(string cacheKey, object obj, DateTime absoluteExpiration) {
            var task = Task.Run(() => Add(cacheKey, obj, absoluteExpiration));
            task.Wait();
            return task;
        }

        public Task RemoveAsync(string cacheKey) {
            var task = Task.Run(() => Remove(cacheKey));
            task.Wait();
            return task;
        }
    }
}

//ncrunch: no coverage end