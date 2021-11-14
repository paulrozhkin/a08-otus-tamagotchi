using Domain.Core.Models;

namespace Domain.Core.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        int Complete();
    }
}
