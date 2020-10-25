using LibraryApp.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Core
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbSet<TEntity> _dbSet;

        protected readonly ApplicationDbContext _applicationDbContext;

        public Repository(ApplicationDbContext context)
        {
            _applicationDbContext = context;
            _dbSet = context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            this._dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this._dbSet.ToList();
        }

        public TEntity GetById(long id)
        {
            return this._dbSet.Find(id);
        }

        public void Remove(TEntity entity)
        {
            this._dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            this._dbSet.Attach(entity);
            this._applicationDbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual IQueryable<TEntity> Queryable() => _dbSet;
    }
}
