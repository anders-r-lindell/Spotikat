using System;
using MongoDB.Bson;
using SpotiKat.Caching.MongoDb.Interfaces;

namespace SpotiKat.Caching.MongoDb {
    public class CachedEntity : ICachedEntity {
        public ObjectId Id { get; set; }
        public string Key { get; set; }
        public object Entity { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}