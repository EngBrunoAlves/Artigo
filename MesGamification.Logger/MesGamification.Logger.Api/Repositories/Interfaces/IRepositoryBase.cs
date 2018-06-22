using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MesGamification.Logger.Api.Entities;
using MongoDB.Driver;

namespace MesGamification.Logger.Api.Repositories.Interfaces
{
    public interface IRepositoryBase<T> where T : IEntityBase
    {
        #region MongoSpecific
        IMongoCollection<T> Collection { get; }
        FilterDefinitionBuilder<T> Filter { get; }
        ProjectionDefinitionBuilder<T> Project { get; }
        UpdateDefinitionBuilder<T> UpdateDefinitionBuilder { get; }
        #endregion MongoSpecific

        #region CRUD

        #region Insert
        void Insert(T entity);
        Task InsertAsync(T entity);
        void Insert(IEnumerable<T> entities);
        Task InsertAsync(IEnumerable<T> entities);
        #endregion Insert

        #region Update
        bool Update(string id, params UpdateDefinition<T>[] updates);
        bool Update(T entity, params UpdateDefinition<T>[] updates);
        bool Update(FilterDefinition<T> filter, params UpdateDefinition<T>[] updates);
        bool Update(Expression<Func<T, bool>> filter, params UpdateDefinition<T>[] updates);
        bool Update<TField>(T entity, Expression<Func<T, TField>> field, TField value);
        bool Update<TField>(FilterDefinition<T> filter, Expression<Func<T, TField>> field, TField value);
        Task<bool> UpdateAsync<TField>(T entity, Expression<Func<T, TField>> field, TField value);
        Task<bool> UpdateAsync(string id, params UpdateDefinition<T>[] updates);
        Task<bool> UpdateAsync(T entity, params UpdateDefinition<T>[] updates);
        Task<bool> UpdateAsync<TField>(FilterDefinition<T> filter, Expression<Func<T, TField>> field, TField value);
        Task<bool> UpdateAsync(FilterDefinition<T> filter, params UpdateDefinition<T>[] updates);
        Task<bool> UpdateAsync(Expression<Func<T, bool>> filter, params UpdateDefinition<T>[] updates);
        #endregion Update

        #region Replace
        bool Replace(T entity);
        Task<bool> ReplaceAsync(T entity);
        void Replace(IEnumerable<T> entities);
        #endregion Replace

        #region Delete
        bool Delete(string id);
        bool Delete(T entity);
        bool Delete(Expression<Func<T, bool>> filter);
        bool DeleteAll();
        Task<bool> DeleteAsync(string id);
        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteAsync(Expression<Func<T, bool>> filter);
        Task<bool> DeleteAllAsync();
        #endregion Delete

        #region Get
        T Get(string id);
        #endregion Get

        #region Find
        IEnumerable<T> Find(Expression<Func<T, bool>> filter);
        IEnumerable<T> Find(Expression<Func<T, bool>> filter, int pageIndex, int size);
        IEnumerable<T> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size);
        IEnumerable<T> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending);
        #endregion Find

        #region FindAll
        IEnumerable<T> FindAll();
        IEnumerable<T> FindAll(int pageIndex, int size);
        IEnumerable<T> FindAll(Expression<Func<T, object>> order, int pageIndex, int size);
        IEnumerable<T> FindAll(Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending);
        #endregion FindAll

        #region First
        T First();
        T First(Expression<Func<T, bool>> filter);
        T First(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order);
        T First(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, bool isDescending);
        #endregion First

        #region Last
        T Last();
        T Last(Expression<Func<T, bool>> filter);
        T Last(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order);
        T Last(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, bool isDescending);
        #endregion Last

        #endregion CRUD

        #region Utils
        bool Any(Expression<Func<T, bool>> filter);
        long Count(Expression<Func<T, bool>> filter);
        Task<long> CountAsync(Expression<Func<T, bool>> filter);
        long Count();
        Task<long> CountAsync();
        #endregion Utils
    }
}