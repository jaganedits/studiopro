using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Service.Entity;

namespace Service.UnitOfWork.Repositories;

public abstract class EntityRepositoryBase<TContext, TEntity> : RepositoryBase<TContext>, IRepository<TEntity> where TContext : DbContext where TEntity : EntityBase, new()
    {
        private readonly short activeStatus = 1;

        public TContext context { get; private set; }
        //private readonly OrderBy<TEntity> DefaultOrderBy = new OrderBy<TEntity>(qry => qry.OrderByDescending(e => e.ModifiedOn));

        protected EntityRepositoryBase(TContext _context) : base(_context)
        {
            context = _context;
        }

        protected DbSet<TEntity> DbSet => Context.Set<TEntity>();

        public virtual IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes);
            return result.ToList();
        }

        public virtual IEnumerable<TEntity> GetNoTracking(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = DbSet;
            var res = query.AsNoTracking();
            return res.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetNoTrackingAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Cache Master Table 
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes);
            // return await result.Cacheable().ToListAsync();
            return await result.ToListAsync();
        }

        public virtual void Load(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes);
            result.Load();
        }

        public virtual async Task LoadAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes);
            await result.LoadAsync();
        }

        public virtual IEnumerable<TEntity> GetPage(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            //if (orderBy == null) orderBy = DefaultOrderBy.Expression;

            var result = QueryDb(null, orderBy, includes);
            return result.Skip(startRow).Take(pageLength).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetPageAsync(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            //if (orderBy == null) orderBy = DefaultOrderBy.Expression;

            var result = QueryDb(null, orderBy, includes);
            return await result.Skip(startRow).Take(pageLength).ToListAsync();
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = DbSet;

            return query.SingleOrDefault(filter);
        }

        public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = DbSet;

            return query.SingleOrDefaultAsync(filter);
        }

        public virtual IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes);
            return result.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes);
            return await result.ToListAsync();
        }

        public virtual void Load(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes);
            result.Load();
        }

        public virtual async Task LoadAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes);
            await result.LoadAsync();
        }

        public virtual IEnumerable<TEntity> QueryPage(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            //if (orderBy == null) orderBy = DefaultOrderBy.Expression;

            var result = QueryDb(filter, orderBy, includes);
            return result.Skip(startRow).Take(pageLength).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> QueryPageAsync(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            //if (orderBy == null) orderBy = DefaultOrderBy.Expression;

            var result = QueryDb(filter, orderBy, includes);
            return await result.Skip(startRow).Take(pageLength).ToListAsync();
        }

        public virtual void Add(TEntity entity)
        {
            if (entity == null) throw new InvalidOperationException("Unable to add a null entity to the repository.");
            DbSet.Add(entity);
        }

        public virtual void AddRange(List<TEntity> entity)
        {
            if (entity == null) throw new InvalidOperationException("Unable to add a null entity to the repository.");
            DbSet.AddRange(entity);
        }

        public virtual void UpdateRange(List<TEntity> entity)
        {
            if (entity == null) throw new InvalidOperationException("Unable to add a null entity to the repository.");

            DbSet.UpdateRange(entity);
        }

        public virtual TEntity Update(TEntity entity)
        {
            //DbSet.Attach(entity);
            //Context.Entry(entity).State = EntityState.Modified;
            //return Context.Entry(entity).Entity;
            return DbSet.Update(entity).Entity;
        }

        public virtual void Remove(TEntity entity)
        {
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Deleted;
            DbSet.Remove(entity);
        }

        public virtual void Remove<TChild>(TChild childEntity)
        {
            Context.Entry(childEntity).State = EntityState.Deleted;
        }

        public virtual void RemoveRange<TChild>(IEnumerable<TChild> childEntitys)
        {
            Context.Entry(childEntitys).State = EntityState.Deleted;
        }

        public virtual void RemoveRange(List<TEntity> entity)
        {
            DbSet.RemoveRange(entity);
        }


        //public virtual void Remove(TEntity entity)
        //{
        //    this.Remove(entity);
        //}

        public virtual bool Any(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Any();
        }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.AnyAsync();
        }

        public virtual int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.CountAsync();
        }

        protected IQueryable<TEntity> QueryDb(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null, bool onlyActive = false)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                if (onlyActive == true)
                    query = query.Where(filter).Where(x => x.Status == activeStatus);
                else
                    query = query.Where(filter);
            }
            else
            {
                if (onlyActive == true)
                    query = query.Where(x => x.Status == activeStatus);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.ElementType.Name.ToLower().Contains("view") ? query.AsNoTracking() : query;
            //return query.AsNoTracking();
        }

        public void SetUnchanged(TEntity entity)
        {
            base.Context.Entry<TEntity>(entity).State = EntityState.Unchanged;
        }

        public virtual IEnumerable<TEntity> GetAllActive(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes, true);
            return result.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllActiveAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes, true);
            return await result.ToListAsync();
        }

        public virtual IEnumerable<TEntity> GetPageActive(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            //if (orderBy == null) orderBy = DefaultOrderBy.Expression;

            var result = QueryDb(null, orderBy, includes, true);
            return result.Skip(startRow).Take(pageLength).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetPageActiveAsync(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            //if (orderBy == null) orderBy = DefaultOrderBy.Expression;

            var result = QueryDb(null, orderBy, includes, true);
            return await result.Skip(startRow).Take(pageLength).ToListAsync();
        }

        //public virtual TEntity GetActive(long id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        //{
        //    IQueryable<TEntity> query = DbSet;

        //    if (includes != null)
        //    {
        //        query = includes(query);
        //    }

        //    return query.SingleOrDefault(x => x.ID == id && x.IsDeleted == false);
        //}

        //public virtual async Task<TEntity> GetActiveAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        //{
        //    IQueryable<TEntity> query = DbSet;

        //    if (includes != null)
        //    {
        //        query = includes(query);
        //    }

        //    return await query.SingleOrDefaultAsync(x => x.ID == id && x.IsDeleted == false);
        //}

        public virtual IEnumerable<TEntity> QueryActive(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes, true);
            return result.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> QueryActiveAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes, true);
            return await result.ToListAsync();
        }

        public virtual IEnumerable<TEntity> QueryPageActive(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            // if (orderBy == null) orderBy = DefaultOrderBy.Expression;

            var result = QueryDb(filter, orderBy, includes, true);
            return result.Skip(startRow).Take(pageLength).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> QueryPageActiveAsync(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            //if (orderBy == null) orderBy = DefaultOrderBy.Expression;

            var result = QueryDb(filter, orderBy, includes, true);
            return await result.Skip(startRow).Take(pageLength).ToListAsync();
        }

        public virtual void LoadActive(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes, true);
            result.Load();
        }

        public virtual async Task LoadActiveAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes, true);
            await result.LoadAsync();
        }

        public virtual IQueryable<TEntity> Filters(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(filter, null, includes, true);
            return result;
        }
        public IQueryable<TEntity> RawQuery(string sql)
        {
            return DbSet.FromSqlRaw(sql);
        }

        public bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        public TEntity Single(params object[] keys)
        {
            return DbSet.Find(keys);
        }
        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate, bool trackChanges = true)
        {
            if (!trackChanges)
            {
                return DbSet.AsNoTracking().FirstOrDefault(predicate);
            }
            return DbSet.SingleOrDefault(predicate);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, bool trackChanges = true)
        {
            if (!trackChanges)
            {
                return DbSet.AsNoTracking().FirstOrDefault(predicate);
            }
            return DbSet.FirstOrDefault(predicate);
        }

        public async Task<bool> ContainsAsync(Expression<Func<TEntity, bool>> predicate, bool trackChanges = true)
        {
            if (!trackChanges)
            {
                return await DbSet.AsNoTracking().AnyAsync(predicate);
            }
            return await DbSet.AnyAsync(predicate);
        }

        public async Task<TEntity> SingleAsync(params object[] keys)
        {
            return await DbSet.FindAsync(keys);
        }
        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true)
        {
            if (!trackChanges)
            {
                if (predicate != null)
                {
                    return await DbSet.AsNoTracking().SingleOrDefaultAsync(predicate);
                }
                return await DbSet.AsNoTracking().SingleOrDefaultAsync();
            }

            if (predicate != null)
            {
                return await DbSet.SingleOrDefaultAsync(predicate);
            }
            return await DbSet.SingleOrDefaultAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true)
        {
            if (!trackChanges)
            {
                if (predicate != null)
                {
                    return await DbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
                }
                return await DbSet.AsNoTracking().FirstOrDefaultAsync();
            }

            if (predicate != null)
            {
                return await DbSet.FirstOrDefaultAsync(predicate);
            }
            return await DbSet.FirstOrDefaultAsync();
        }

        public async Task<TEntity> IncludeSingleOrDefaultAsync([NotNull] string[] navigationPaths, Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true)
        {
            IQueryable<TEntity> query = DbSet;

            foreach (var path in navigationPaths)
            {
                query = query.Include(path);
            }
            if (!trackChanges)
            {
                if (predicate != null)
                {
                    return await query.AsNoTracking().SingleOrDefaultAsync(predicate);
                }
                return await query.AsNoTracking().SingleOrDefaultAsync();
            }
            if (predicate != null)
            {
                return await query.SingleOrDefaultAsync(predicate);
            }
            return await query.SingleOrDefaultAsync();
        }
        public async Task<TEntity> IncludeFirstOrDefaultAsync([NotNull] string[] navigationPaths, Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true)
        {
            IQueryable<TEntity> query = DbSet;

            foreach (var path in navigationPaths)
            {
                query = query.Include(path);
            }

            if (!trackChanges)
            {
                if (predicate != null)
                {
                    return await query.AsNoTracking().FirstOrDefaultAsync(predicate);
                }
                return await query.AsNoTracking().FirstOrDefaultAsync();
            }

            if (predicate != null)
            {
                return await query.FirstOrDefaultAsync(predicate);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<TEntity> IncludeSingleOrDefaultAsync([NotNull] string navigationPath, Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true)
        {
            string[] navPaths = { navigationPath };
            return await IncludeSingleOrDefaultAsync(navPaths, predicate, trackChanges);
        }

        public async Task<TEntity> IncludeFirstOrDefaultAsync([NotNull] string navigationPath, Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true)
        {
            string[] navPaths = { navigationPath };
            return await IncludeFirstOrDefaultAsync(navPaths, predicate, trackChanges);
        }

        public TEntity IncludeSingleOrDefault([NotNull] string[] navigationPaths, Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true)
        {
            IQueryable<TEntity> query = DbSet;

            foreach (var path in navigationPaths)
            {
                query = query.Include(path);
            }

            if (!trackChanges)
            {
                if (predicate != null)
                {
                    return query.AsNoTracking().SingleOrDefault(predicate);
                }
                return query.AsNoTracking().SingleOrDefault();
            }

            if (predicate != null)
            {
                return query.SingleOrDefault(predicate);
            }
            return query.SingleOrDefault();
        }

        public TEntity IncludeFirstOrDefault([NotNull] string[] navigationPaths, Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true)
        {
            IQueryable<TEntity> query = DbSet;

            foreach (var path in navigationPaths)
            {
                query = query.Include(path);
            }

            if (!trackChanges)
            {
                if (predicate != null)
                {
                    return query.AsNoTracking().FirstOrDefault(predicate);
                }
                return query.AsNoTracking().FirstOrDefault();
            }

            if (predicate != null)
            {
                return query.FirstOrDefault(predicate);
            }
            return query.FirstOrDefault();
        }

        public TEntity IncludeSingleOrDefault([NotNull] string navigationPath, Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true)
        {
            string[] navPaths = { navigationPath };
            return IncludeSingleOrDefault(navPaths, predicate, trackChanges);
        }

        public TEntity IncludeFirstOrDefault([NotNull] string navigationPath, Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true)
        {
            string[] navPaths = { navigationPath };
            return IncludeFirstOrDefault(navPaths, predicate, trackChanges);
        }

        public void Detach(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Detached;
        }

    }