using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Core.Models;
using Domain.Core.Repositories;
using Domain.Core.Repositories.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public async ValueTask AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async ValueTask AddRangeAsync(IEnumerable<TEntity> entities)
        { 
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }

        public bool Contains(ISpecification<TEntity> specification = null)
        {
            return Count(specification) > 0;
        }

        public bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return Count(predicate) > 0;
        }

        public async Task<bool> ContainsAsync(ISpecification<TEntity> specification = null)
        {
            return await CountAsync(specification) > 0;
        }

        public async Task<bool> ContainsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await CountAsync(predicate) > 0;
        }

        public int Count(ISpecification<TEntity> specification = null)
        {
            return ApplySpecification(specification).Count();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).Count();
        }

        public Task<int> CountAsync(ISpecification<TEntity> specification = null)
        {
            return ApplySpecification(specification).CountAsync();
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).CountAsync();
        }

        public IEnumerable<TEntity> Find(ISpecification<TEntity> specification = null)
        {
            return ApplySpecification(specification);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(ISpecification<TEntity> specification = null)
        {
            var results = await ApplySpecification(specification).ToListAsync();
            return results;
        }


        public TEntity FindById(Guid id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public ValueTask<TEntity> FindByIdAsync(Guid id)
        {
            return _context.Set<TEntity>().FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(_context.Set<TEntity>().AsQueryable(), spec);
        }
    }
}