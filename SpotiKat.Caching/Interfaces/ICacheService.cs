using System;
using System.Threading.Tasks;

namespace SpotiKat.Caching.Interfaces {
    public interface ICacheService {
        T Get<T>(string cacheKey, TimeSpan cacheTimeout, bool cacheNullAndEmptyCollection, Func<T> delegateFunction)
            where T : class;

        void Remove(string cacheKey);

        Task<T> GetAsync<T>(string cacheKey, TimeSpan cacheTimeout, bool cacheNullAndEmptyCollection,
            Func<Task<T>> delegateFunction) where T : class;

        Task RemoveAsync(string cacheKey);
    }
}