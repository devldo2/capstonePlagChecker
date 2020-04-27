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
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private readonly IDbContext context;
        public Repository(IDbContext context)
        {
            this.context = context;
        }
        public TEntity Add(TEntity entity)
        {
            context.Database.Set<TEntity>().Add(entity);
            return entity;
        }
        public void Delete(TEntity entity)
        {
            var existsEntity = GetById(entity.Id);
            if (existsEntity != null)
            {
                context.Database.Set<TEntity>().Remove(existsEntity);
            }
        }
        public void Edit(TEntity entity)
        {
            context.Database.Entry(entity).State = EntityState.Modified;
        }
        public IQueryable<TEntity> GetAll(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var result = this.context.Database.Set<TEntity>().AsQueryable();

            if (include != null)
                result = include(result);

            return result;
        }
        public TEntity GetById(int id)
        {
            return this.context.Database.Set<TEntity>().Find(id);
        }
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            context.Database.Set<TEntity>().RemoveRange(entities);
        }
        public void Save()
        {
            context.Database.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await context.Database.SaveChangesAsync();
        }
    }
}
