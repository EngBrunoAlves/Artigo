using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MesGamification.Logger.Api.Entities;
using MesGamification.Logger.Api.Repositories.Interfaces;
using MesGamification.Logger.Api.Repositories.Context;

namespace MesGamification.Logger.Api.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : IEntityBase
    {
        #region MongoSpecific

        public RepositoryBase(MesGamificationContext context)
        {
            Collection = context.GetCollection<T>();
        }

        public IMongoCollection<T> Collection { get; private set; }

        public FilterDefinitionBuilder<T> Filter => Builders<T>.Filter;

        public ProjectionDefinitionBuilder<T> Project => Builders<T>.Projection;

        public UpdateDefinitionBuilder<T> Updater => Builders<T>.Update;

        public UpdateDefinitionBuilder<T> UpdateDefinitionBuilder => throw new NotImplementedException();

        private IFindFluent<T, T> Query(Expression<Func<T, bool>> filter) => Collection.Find(filter);

        private IFindFluent<T, T> Query() => Collection.Find(Filter.Empty);

        #endregion MongoSpecific

        #region CRUD

        #region Insert
        public virtual void Insert(T entity)
        {
            Retry(() =>
           {
               Collection.InsertOne(entity);
               return true;
           });
        }
        public virtual Task InsertAsync(T entity)
        {
            return Retry(() =>
            {
                return Collection.InsertOneAsync(entity);
            });
        }
        public virtual void Insert(IEnumerable<T> entities)
        {
            Retry(() =>
            {
                Collection.InsertMany(entities);
                return true;
            });
        }
        public virtual Task InsertAsync(IEnumerable<T> entities)
        {
            return Retry(() =>
            {
                return Collection.InsertManyAsync(entities);
            });
        }
        #endregion Insert

        #region Update
        public virtual bool Update(string id, params UpdateDefinition<T>[] updates)
        {
            return Update(Filter.Eq(i => i.Id, id), updates);
        }
        public virtual bool Update(T entity, params UpdateDefinition<T>[] updates)
        {
            return Update(entity.Id, updates);
        }
        public virtual bool Update(FilterDefinition<T> filter, params UpdateDefinition<T>[] updates)
        {
            return Retry(() =>
            {
                var update = Updater.Combine(updates).CurrentDate(i => i.ModifiedOn);
                return Collection.UpdateMany(filter, update.CurrentDate(i => i.ModifiedOn)).IsAcknowledged;
            });
        }
        public virtual bool Update(Expression<Func<T, bool>> filter, params UpdateDefinition<T>[] updates)
        {
            return Retry(() =>
            {
                var update = Updater.Combine(updates).CurrentDate(i => i.ModifiedOn);
                return Collection.UpdateMany(filter, update).IsAcknowledged;
            });
        }
        public virtual bool Update<TField>(T entity, Expression<Func<T, TField>> field, TField value)
        {
            return Update(entity, Updater.Set(field, value));
        }
        public virtual bool Update<TField>(FilterDefinition<T> filter, Expression<Func<T, TField>> field, TField value)
        {
            return Update(filter, Updater.Set(field, value));
        }

        public virtual Task<bool> UpdateAsync<TField>(T entity, Expression<Func<T, TField>> field, TField value)
        {
            return Task.Run(() =>
               {
                   return Update(entity, Updater.Set(field, value));
               });
        }
        public virtual Task<bool> UpdateAsync(string id, params UpdateDefinition<T>[] updates)
        {
            return Task.Run(() =>
           {
               return Update(Filter.Eq(i => i.Id, id), updates);
           });
        }
        public virtual Task<bool> UpdateAsync(T entity, params UpdateDefinition<T>[] updates)
        {
            return Task.Run(() =>
            {
                return Update(entity.Id, updates);
            });
        }
        public virtual Task<bool> UpdateAsync<TField>(FilterDefinition<T> filter, Expression<Func<T, TField>> field, TField value)
        {
            return Task.Run(() =>
            {
                return Update(filter, Updater.Set(field, value));
            });
        }
        public virtual Task<bool> UpdateAsync(FilterDefinition<T> filter, params UpdateDefinition<T>[] updates)
        {
            return Retry(() =>
            {
                return Task.Run(() =>
                {
                    return Update(filter, updates);
                });
            });
        }
        public virtual Task<bool> UpdateAsync(Expression<Func<T, bool>> filter, params UpdateDefinition<T>[] updates)
        {
            return Retry(() =>
           {
               return Task.Run(() =>
               {
                   return Update(filter, updates);
               });
           });
        }
        #endregion Update

        #region Replace
        public virtual bool Replace(T entity)
        {
            return Retry(() =>
               {
                   return Collection.ReplaceOne(i => i.Id == entity.Id, entity).IsAcknowledged;
               });
        }
        public virtual Task<bool> ReplaceAsync(T entity)
        {
            return Retry(() =>
            {
                return Task.Run(() =>
                {
                    return Replace(entity);
                });
            });
        }
        public virtual void Replace(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                Replace(entity);
            }
        }
        #endregion Replace

        #region Delete
        public virtual bool Delete(string id)
        {
            return Retry(() =>
            {
                return Collection.DeleteOne(i => i.Id == id).IsAcknowledged;
            });
        }
        public bool Delete(T entity)
        {
            return Delete(entity.Id);
        }
        public virtual bool Delete(Expression<Func<T, bool>> filter)
        {
            return Retry(() =>
            {
                return Collection.DeleteMany(filter).IsAcknowledged;
            });
        }
        public virtual bool DeleteAll()
        {
            return Retry(() =>
            {
                return Collection.DeleteMany(Filter.Empty).IsAcknowledged;
            });
        }

        public virtual Task<bool> DeleteAsync(string id)
        {
            return Retry(() =>
            {
                return Task.Run(() =>
                {
                    return Delete(id);
                });
            });
        }
        public Task<bool> DeleteAsync(T entity)
        {
            return Task.Run(() =>
            {
                return Delete(entity);
            });
        }
        public virtual Task<bool> DeleteAsync(Expression<Func<T, bool>> filter)
        {
            return Retry(() =>
            {
                return Task.Run(() =>
                {
                    return Delete(filter);
                });
            });
        }
        public virtual Task<bool> DeleteAllAsync()
        {
            return Retry(() =>
                        {
                            return Task.Run(() =>
                            {
                                return DeleteAll();
                            });
                        });
        }
        #endregion Delete

        #region Get
        public virtual T Get(string id)
        {
            return Retry(() =>
            {
                return Find(i => i.Id == id).FirstOrDefault();
            });
        }
        #endregion Get

        #region Find
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> filter)
        {
            return Query(filter).ToEnumerable();
        }
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> filter, int pageIndex, int size)
        {
            return Find(filter, i => i.Id, pageIndex, size);
        }
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size)
        {
            return Find(filter, order, pageIndex, size, true);
        }
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending)
        {
            return Retry(() =>
            {
                var query = Query(filter).Skip(pageIndex * size).Limit(size);
                return (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToEnumerable();
            });
        }
        #endregion Find

        #region FindAll
        public virtual IEnumerable<T> FindAll()
        {
            return Retry(() =>
            {
                return Query().ToEnumerable();
            });
        }
        public virtual IEnumerable<T> FindAll(int pageIndex, int size)
        {
            return FindAll(i => i.Id, pageIndex, size);
        }
        public virtual IEnumerable<T> FindAll(Expression<Func<T, object>> order, int pageIndex, int size)
        {
            return FindAll(order, pageIndex, size, true);
        }
        public virtual IEnumerable<T> FindAll(Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending)
        {
            return Retry(() =>
            {
                var query = Query().Skip(pageIndex * size).Limit(size);
                return (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToEnumerable();
            });
        }
        #endregion FindAll

        #region First
        public virtual T First()
        {
            return FindAll(i => i.Id, 0, 1, false).FirstOrDefault();
        }
        public virtual T First(Expression<Func<T, bool>> filter)
        {
            return First(filter, i => i.Id);
        }
        public virtual T First(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order)
        {
            return First(filter, order, false);
        }
        public virtual T First(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, bool isDescending)
        {
            return Find(filter, order, 0, 1, isDescending).FirstOrDefault();
        }
        #endregion First

        #region Last
        public virtual T Last()
        {
            return FindAll(i => i.Id, 0, 1, true).FirstOrDefault();
        }
        public virtual T Last(Expression<Func<T, bool>> filter)
        {
            return Last(filter, i => i.Id);
        }
        public virtual T Last(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order)
        {
            return Last(filter, order, false);
        }
        public virtual T Last(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, bool isDescending)
        {
            return First(filter, order, !isDescending);
        }
        #endregion Last

        #endregion CRUD

        #region Utils
        public virtual bool Any(Expression<Func<T, bool>> filter)
        {
            return Retry(() =>
          {
              return First(filter) != null;
          });
        }
        public virtual long Count(Expression<Func<T, bool>> filter)
        {
            return Retry(() =>
            {
                return Collection.Count(filter);
            });
        }
        public virtual Task<long> CountAsync(Expression<Func<T, bool>> filter)
        {
            return Retry(() =>
            {
                return Collection.CountAsync(filter);
            });
        }
        public virtual long Count()
        {
            return Retry(() =>
            {
                return Collection.Count(Filter.Empty);
            });
        }
        public virtual Task<long> CountAsync()
        {
            return Retry(() =>
            {
                return Collection.CountAsync(Filter.Empty);
            });
        }
        #endregion Utils

        #region RetryPolicy
        protected virtual TResult Retry<TResult>(Func<TResult> action) =>
            RetryPolicy
            .Handle<MongoConnectionException>(i => i.InnerException.GetType() == typeof(IOException) || i.InnerException.GetType() == typeof(SocketException))
            .Retry(3)
            .Execute(action);
        #endregion RetryPolicy
    }
}