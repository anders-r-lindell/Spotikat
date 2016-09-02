using System;
using MongoDB.Bson;

namespace SpotiKat.Caching.MongoDb.Interfaces {
    public interface ICachedEntity {
        ObjectId Id { get; set; }
        string Key { get; set; }
        object Entity { get; set; }
        DateTime ExpirationDate { get; set; }
    }
}