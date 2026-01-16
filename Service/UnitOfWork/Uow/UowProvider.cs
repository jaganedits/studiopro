using Microsoft.EntityFrameworkCore;
using Service.Entity;

namespace Service.UnitOfWork.Uow;

 public class UowProvider : IUowProvider
    {
        private string _transactionName = string.Empty;
        private readonly DbContext _context;
        private UnitOfWork uow;

        public UowProvider()
        { }

        public UowProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _context = (DbContext)_serviceProvider.GetService(typeof(IEntityContext));
            uow = new UnitOfWork(_context, _serviceProvider);
        }

        private readonly IServiceProvider _serviceProvider;

        public IUnitOfWork CreateUnitOfWork(bool trackChanges = true, bool enableLogging = false)
        {
            if (!trackChanges)
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return GetUnitOfWork();
        }

        public IUnitOfWork CreateUnitOfWork<TEntityContext>(bool trackChanges = true, bool enableLogging = false) where TEntityContext : DbContext
        {
            if (!trackChanges)
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return GetUnitOfWork();
        }

        public async Task<IUnitOfWork> CreateUnitOfWork(bool withTransaction, string TransactionName, bool trackChanges = true, bool enableLogging = false)
        {
            if (withTransaction)
            {
                if (_context.Database.CurrentTransaction == null)
                {
                    await _context.Database.BeginTransactionAsync();
                    if (string.IsNullOrEmpty(TransactionName))
                        throw new Exception("Please provide Transaction name.");

                    if (string.IsNullOrEmpty(this._transactionName))
                        this._transactionName = TransactionName;
                }
            }

            if (!trackChanges)
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return GetUnitOfWork();
        }

        public async Task CommitTransaction(string transactionName)
        {
            if (_context.Database.CurrentTransaction != null)
            {
                if (transactionName.Equals(transactionName))
                {
                    await _context.Database.CommitTransactionAsync();
                    this._transactionName = null;
                }
            }
        }

        public async Task RollbackTransaction(string transactionName)
        {
            if (_context.Database.CurrentTransaction != null)
            {
                if (transactionName.Equals(transactionName))
                {
                    await _context.Database.RollbackTransactionAsync();
                    this._transactionName = null;
                }
            }
        }

        private UnitOfWork GetUnitOfWork()
        {
            if (uow == null)
                uow = new UnitOfWork(_context, _serviceProvider);

            return uow;
        }

    }