using MongoDB.Driver;

namespace SpotiKat.MongoDb.Interfaces {
    public interface IMongoDbFactory {
        IMongoDatabase Get();
    }
}