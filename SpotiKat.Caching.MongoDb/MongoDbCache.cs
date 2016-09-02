using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using SpotiKat.Interfaces.Caching;
using SpotiKat.MongoDb.Interfaces;

namespace SpotiKat.Caching.MongoDb {
    public class MongoDbCache : ICache {
        private const string MongoDbCollectionName = "cachedentities_v2";
        private readonly IMongoDbFactory _mongoDbFactory;

        public MongoDbCache(IMongoDbFactory mongoDbFactory) {
            _mongoDbFactory = mongoDbFactory;
        }

        public object Get(string cacheKey) {
            throw new NotImplementedException("use GetAsync for this implementation");
        }

        public void Add(string cacheKey, object obj, DateTime absoluteExpiration) {
            throw new NotImplementedException("use AddAsync for this implementation");
        }

        public void Remove(string cacheKey) {
            throw new NotImplementedException("use RemoveAsync for this implementation");
        }

        public async Task<object> GetAsync(string cacheKey) {
            var cachedEntity = await GetCollection().Find(entity => entity.Key == cacheKey).FirstOrDefaultAsync();

            if (cachedEntity != null && cachedEntity.ExpirationDate > DateTime.Now) {
                return cachedEntity.Entity;
            }

            return null;
        }

        public async Task AddAsync(string cacheKey,
            object obj,
            DateTime absoluteExpiration) {
            await RemoveAsync(cacheKey);
            var cachedEntity = new CachedEntity {Key = cacheKey, Entity = obj, ExpirationDate = absoluteExpiration};
            await GetCollection().InsertOneAsync(cachedEntity);
        }

        public async Task RemoveAsync(string cacheKey) {
            await GetCollection().FindOneAndDeleteAsync(entity => entity.Key == cacheKey);
        }

        private IMongoCollection<CachedEntity> GetCollection() {
            return _mongoDbFactory.Get().GetCollection<CachedEntity>(MongoDbCollectionName);
        }
    }
}