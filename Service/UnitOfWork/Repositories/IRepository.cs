using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Service.UnitOfWork.Repositories;

 public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        TEntity Get(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> GetNoTracking(Expression<Func<TEntity, bool>> filter = null);
        Task<IEnumerable<TEntity>> GetNoTrackingAsync(Expression<Func<TEntity, bool>> filter = null);
        Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        IEnumerable<TEntity> GetAllActive(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        Task<IEnumerable<TEntity>> GetAllActiveAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        IEnumerable<TEntity> GetPage(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        Task<IEnumerable<TEntity>> GetPageAsync(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        IEnumerable<TEntity> GetPageActive(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        Task<IEnumerable<TEntity>> GetPageActiveAsync(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        IEnumerable<TEntity> QueryActive(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        Task<IEnumerable<TEntity>> QueryActiveAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        IEnumerable<TEntity> QueryPage(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        Task<IEnumerable<TEntity>> QueryPageAsync(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        IQueryable<TEntity> Filters(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        IEnumerable<TEntity> QueryPageActive(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        Task<IEnumerable<TEntity>> QueryPageActiveAsync(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        void Load(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        Task LoadAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        void LoadActive(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        Task LoadActiveAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        void Add(TEntity entity);
        void AddRange(List<TEntity> entity);
        TEntity Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(List<TEntity> entity);
        //void Remove(int id);
        bool Any(Expression<Func<TEntity, bool>> filter = null);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null);
        int Count(Expression<Func<TEntity, bool>> filter = null);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null);
        void SetUnchanged(TEntity entitieit);
        IQueryable<TEntity> RawQuery(string sql);
        bool Contains(Expression<Func<TEntity, bool>> predicate);
        TEntity Single(params object[] keys);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, bool trackChanges = true);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate, bool trackChanges = true);
        Task<bool> ContainsAsync(Expression<Func<TEntity, bool>> predicate, bool trackChanges = true);
        Task<TEntity> SingleAsync(params object[] keys);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true);
        Task<TEntity> IncludeSingleOrDefaultAsync([NotNull]string[] navigationPaths, Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true);
        Task<TEntity> IncludeFirstOrDefaultAsync([NotNull]string[] navigationPaths, Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true);
        Task<TEntity> IncludeSingleOrDefaultAsync([NotNull] string navigationPath, Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true);
        Task<TEntity> IncludeFirstOrDefaultAsync([NotNull] string navigationPath, Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true);
        TEntity IncludeSingleOrDefault([NotNull] string[] navigationPaths, Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true);
        TEntity IncludeFirstOrDefault([NotNull] string[] navigationPaths, Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true);
        TEntity IncludeSingleOrDefault([NotNull] string navigationPath, Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true);
        TEntity IncludeFirstOrDefault([NotNull] string navigationPath, Expression<Func<TEntity, bool>> predicate = null, bool trackChanges = true);

        void Remove<TChild>(TChild childEntity);
        void UpdateRange(List<TEntity> entity);
        void RemoveRange<TChild>(IEnumerable<TChild> childEntitys);
        void Detach(TEntity entity);
    }