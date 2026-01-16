using Microsoft.EntityFrameworkCore;

namespace Service.Entity;

public class EntityContextBase<TContext> : DbContext, IEntityContext where TContext : DbContext
{
    public EntityContextBase(DbContextOptions<TContext> options) : base(options)
    {
    }
}