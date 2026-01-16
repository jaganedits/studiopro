using Microsoft.EntityFrameworkCore;
using Service.Entity;

namespace Service.UnitOfWork.Repositories;

public class GenericEntityRepository<TEntity> : EntityRepositoryBase<DbContext, TEntity> where TEntity : EntityBase, new()
{
    public GenericEntityRepository() : base( null)
    { }
}