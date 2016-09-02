using SpotiKat.Entities;

namespace SpotiKat.MongoDb {
    public class BsonClassMap {
        public void Register() {
            MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Album>();
            /*MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Album>()
                .SetDiscriminator(typeof (Album).FullName + ", " + typeof (Album).Assembly.FullName.Split(',')[0]);*/
        }
    }
}