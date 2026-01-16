using Microsoft.EntityFrameworkCore;

namespace Service.UnitOfWork.Repositories;

public interface IRepositoryInjection
{
    IRepositoryInjection SetContext(DbContext context);
}