using AntiPlagiatusServer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AntiPlagiatusServer.Data
{
    public interface IRepository<T> where T : EntityBase
    {
        /// <summary>
        /// Get query for all entities
        /// </summary>
        /// <returns>Query for all entities</returns>
        IQueryable<T> GetAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        /// <summary>
        /// Add entity to data base
        /// </summary>
        /// <param name="entity">Entity for add</param>
        /// <returns>Saved entity</returns>
        T Add(T entity);

        /// <summary>
        /// Delete entity from data base
        /// </summary>
        /// <param name="entity">Entity for delete</param>
        void Delete(T entity);

        /// <summary>
        /// Edit entity in data base
        /// </summary>
        /// <param name="entity">Entity to edit</param>
        void Edit(T entity);

        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <returns>Entity by id</returns>
        T GetById(int id);

        /// <summary>
        /// Update entity in data base
        /// </summary>
        /// <param name="entity">Entity for update</param>
        void Save();

        /// <summary>
        /// Update entity async in data base
        /// </summary>
        /// <param name="entity">Entity for update</param>
        Task SaveAsync();
        /// <summary>
        /// Remove range of entities from data base
        /// </summary>
        /// <param name="entities">IEnumerable collection of entities</param>
        void RemoveRange(IEnumerable<T> entities);

        //IQueryable<T> Include(params Expression<Func<T, object>>[] includes);
    }
}
