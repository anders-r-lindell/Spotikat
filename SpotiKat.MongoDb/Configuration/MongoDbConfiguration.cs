using SpotiKat.MongoDb.Interfaces.Configuration;

namespace SpotiKat.MongoDb.Configuration {
    public class MongoDbConfiguration : IMongoDbConfiguration {
        public string Url {
            get { return Settings.Default.Url; }
        }

        public string DatabaseName {
            get { return Settings.Default.DatabaseName; }
        }
    }
}