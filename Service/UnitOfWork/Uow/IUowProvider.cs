namespace Service.UnitOfWork.Uow;

public interface IUowProvider
{
    IUnitOfWork CreateUnitOfWork(bool trackChanges = true, bool enableLogging = false);
    // IUnitOfWork CreateUnitOfWork<TEntityContext>(bool trackChanges = true, bool enableLogging = false) where TEntityContext : DbContext;
    Task<IUnitOfWork> CreateUnitOfWork(bool withTransaction, string TransactionName,bool trackChanges = true, bool enableLogging = false);
    Task CommitTransaction(string transactionName);
    Task RollbackTransaction(string transactionName);
}