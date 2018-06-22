using System;
using System.Reflection;
using MesGamification.Logger.Api.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MesGamification.Logger.Api.Repositories.Context
{
    public class MesGamificationContext
    {
        private readonly MesGamificationOptions _options;
        public MesGamificationContext(IOptions<MesGamificationOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentException(nameof(options));
        }

        private IMongoDatabase GetDatabase()
        {
            var client = new MongoClient(_options.ConnectionString) ?? throw new ArgumentException("Connection string error");
            return client.GetDatabase(_options.Database);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            var database = GetDatabase();
            var collectionName = GetCollectionName<T>();
            return database.GetCollection<T>(collectionName);
        }


        #region Collectionname
        private string GetCollectionName<T>()
        {
            string collectionName = typeof(T).GetTypeInfo().BaseType.Equals(typeof(object)) ?
                GetCollectionNameFromInterface<T>() : GetCollectionNameFromType<T>();

            if (string.IsNullOrEmpty(collectionName))
                collectionName = typeof(T).Name;
            return collectionName.ToLowerInvariant();
        }

        private string GetCollectionNameFromInterface<T>() =>
            CustomAttributeExtensions.GetCustomAttribute<CollectionNameAttribute>(typeof(T).GetTypeInfo().Assembly)?.Name ?? typeof(T).Name;

        private string GetCollectionNameFromType<T>()
        {
            Type entitytype = typeof(T);
            string collectionname;

            var att = CustomAttributeExtensions.GetCustomAttribute<CollectionNameAttribute>(typeof(T).GetTypeInfo().Assembly);
            if (att != null)
                collectionname = att.Name;
            else
                collectionname = entitytype.Name;

            return collectionname;
        }
        #endregion Collectionname
    }
}