using Microsoft.EntityFrameworkCore;

namespace Service.UnitOfWork.Uow;

public class UnitOfWork : UnitOfWorkBase<DbContext>, IUnitOfWork
{
    public UnitOfWork(DbContext context, IServiceProvider provider) : base(context, provider)
    { }
}