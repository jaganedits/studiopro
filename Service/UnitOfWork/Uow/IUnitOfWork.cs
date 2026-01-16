using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.UnitOfWork.Repositories;

namespace Service.UnitOfWork.Uow;

public interface IUnitOfWork : IDisposable
{
    int SaveChanges();
    Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    IRepository<TEntity> GetRepository<TEntity>();
    TRepository GetCustomRepository<TRepository>();

    DbContext GetContext<TRepository>();

    void OnCompleteTransaction(Action<ChangeTracker> func);

    bool IsContextBeginTransaction();
}