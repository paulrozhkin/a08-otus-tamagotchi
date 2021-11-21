using System;
using System.Collections;
using System.Linq;
using Domain.Core.Models;
using Domain.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.Repositories
{
    public class UnitOfWork<T> : IUnitOfWork where T : DbContext
    {
        private readonly T _context;
        private Hashtable _repositories;

        public UnitOfWork(T context)
        {
            _context = context;
        }

        public int Complete()
        {
            var entries = _context.ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && e.State is EntityState.Added or EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTimeOffset.Now.UtcDateTime;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTimeOffset.Now.UtcDateTime;
                }
            }

            return _context.SaveChanges();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            _repositories ??= new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                        .MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<TEntity>) _repositories[type];
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}