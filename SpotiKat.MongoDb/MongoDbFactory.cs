using MongoDB.Driver;
using SpotiKat.MongoDb.Interfaces;
using SpotiKat.MongoDb.Interfaces.Configuration;

namespace SpotiKat.MongoDb {
    public class MongoDbFactory : IMongoDbFactory {
        private readonly IMongoDbConfiguration _mongoDbConfiguration;

        public MongoDbFactory(IMongoDbConfiguration mongoDbConfiguration) {
            _mongoDbConfiguration = mongoDbConfiguration;
        }

        public IMongoDatabase Get() {
            var client = new MongoClient(_mongoDbConfiguration.Url);
            return client.GetDatabase(_mongoDbConfiguration.DatabaseName);
        }
    }
}