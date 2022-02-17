using System;
using Domain.Core.Models;

namespace Domain.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        int Complete();
    }
}
