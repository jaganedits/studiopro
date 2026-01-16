using Microsoft.Extensions.DependencyInjection.Extensions;
using Service.Entity;
using Service.UnitOfWork.Repositories;
using Service.UnitOfWork.Uow;

namespace Service.UnitOfWork.Startup;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddDataAccess<TEntityContext>(this IServiceCollection services) where TEntityContext : EntityContextBase<TEntityContext>
    {
        RegisterDataAccess<TEntityContext>(services);
        return services;
    }

    private static void RegisterDataAccess<TEntityContext>(IServiceCollection services) where TEntityContext : EntityContextBase<TEntityContext>
    {
        services.TryAddScoped<IUowProvider, UowProvider>();
        services.TryAddTransient<IEntityContext, TEntityContext>();
        services.TryAddTransient(typeof(IRepository<>), typeof(GenericEntityRepository<>));

    }
}