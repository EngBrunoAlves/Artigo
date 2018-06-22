using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MesGamification.Logger.Api.Entities
{
    public interface IEntityBase
    {
        [BsonId]
        string Id { get; set; }

        [BsonIgnore]
        DateTime CreatedOn { get; }

        DateTime ModifiedOn { get; }

        [BsonIgnore]
        ObjectId ObjectId { get; }
    }
}