using Microsoft.EntityFrameworkCore;
using Service.Entity;

namespace Service.Models;
public partial class DbContextHelper : EntityContextBase<DbContextHelper>
{

    public DbContextHelper(DbContextOptions<DbContextHelper> options) : base(options)
    {
    }
}