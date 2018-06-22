using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace MesGamification.Logger.Api.Entities
{
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class EntityBase : IEntityBase
    {
        public EntityBase()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        [JsonProperty(Order = 1)]
        [BsonElement(Order = 0)]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }
        public DateTime CreatedOn => ObjectId.CreationTime;

        [BsonElement("_m", Order = 1)]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        [JsonIgnore]
        public ObjectId ObjectId => ObjectId.Parse(Id ?? (Id = ObjectId.GenerateNewId().ToString()));
    }
}