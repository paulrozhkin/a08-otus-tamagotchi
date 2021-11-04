using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Core.Models;
using Domain.Core.Repositories.Specifications;

namespace Domain.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity FindById(int id);
        ValueTask<TEntity> FindByIdAsync(int id);

        IEnumerable<TEntity> Find(ISpecification<TEntity> specification = null!);
        Task<IEnumerable<TEntity>> FindAsync(ISpecification<TEntity> specification = null!);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        ValueTask AddAsync(TEntity entity);
        ValueTask AddRangeAsync(IEnumerable<TEntity> entities);


        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        bool Contains(ISpecification<TEntity> specification = null!);
        bool Contains(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ContainsAsync(ISpecification<TEntity> specification = null!);
        Task<bool> ContainsAsync(Expression<Func<TEntity, bool>> predicate);


        int Count(ISpecification<TEntity> specification = null!);
        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(ISpecification<TEntity> specification = null!);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    }
}