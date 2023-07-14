using System;
using System.Data;
using System.Threading.Tasks;

namespace UnitOfWork
{
    public abstract class BaseRepository : IDisposable
    {
        private readonly Lazy<IDbTransaction> _transaction;

        private bool _isDisposed;

        protected BaseRepository(Func<IDbTransaction> transactionFactory) : this() =>
            _transaction = new Lazy<IDbTransaction>(transactionFactory);

        private BaseRepository()
        {
        }

        protected IDbConnection Connection => _transaction.Value?.Connection;

        protected IDbTransaction Transaction => _transaction.Value;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (isDisposing)
            {
                if (_transaction.IsValueCreated)
                {
                    _transaction.Value.Dispose();
                }
            }

            _isDisposed = true;
        }
    }

    public abstract class BaseRepository<TKey, T> : BaseRepository, IRepository<TKey, T>
        where T : class
    {
        protected BaseRepository(Func<IDbTransaction> transactionFactory) : base(transactionFactory)
        {
        }

        public abstract T Get(TKey id);

        public abstract void Insert(T entity);
        public abstract void Update(T entity);

        public abstract void Delete(T entity);
    }

    public abstract class BaseAsyncRepository<TKey, T> : BaseRepository<TKey, T>,
        IAsyncRepository<TKey, T> where T : class
    {
        protected BaseAsyncRepository(Func<IDbTransaction> transactionFactory) : base(
            transactionFactory)
        {
        }

        public abstract Task<T> GetAsync(TKey id);
        public abstract Task InsertAsync(T entity);
        public abstract Task UpdateAsync(T entity);
        public abstract Task DeleteAsync(T entity);

        public override T Get(TKey id) => GetAsync(id).GetAwaiter().GetResult();

        public override void Insert(T entity) => InsertAsync(entity).GetAwaiter().GetResult();

        public override void Update(T entity) => UpdateAsync(entity).GetAwaiter().GetResult();

        public override void Delete(T entity) => DeleteAsync(entity).GetAwaiter().GetResult();
    }
}
