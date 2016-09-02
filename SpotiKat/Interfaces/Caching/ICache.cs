using System;
using System.Threading.Tasks;

namespace SpotiKat.Interfaces.Caching {
    public interface ICache {
        object Get(string cacheKey);

        void Add(string cacheKey,
            object obj,
            DateTime absoluteExpiration);

        void Remove(string cacheKey);
        Task<object> GetAsync(string cacheKey);

        Task AddAsync(string cacheKey,
            object obj,
            DateTime absoluteExpiration);

        Task RemoveAsync(string cacheKey);
    }
}