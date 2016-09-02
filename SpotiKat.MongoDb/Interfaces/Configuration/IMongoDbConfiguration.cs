namespace SpotiKat.MongoDb.Interfaces.Configuration {
    public interface IMongoDbConfiguration {
        string Url { get; }
        string DatabaseName { get; }
    }
}