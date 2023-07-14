using System.Data;

namespace UnitOfWork
{
    internal readonly struct UnitOfWorkWrapper : IUnitOfWork
    {
        private readonly IDbTransaction _transaction;
        public UnitOfWorkWrapper(IDbTransaction transaction) => _transaction = transaction;
        public void Commit() => _transaction.Commit();

        public void Rollback() => _transaction.Rollback();

        public void Dispose() => _transaction.Dispose();
    }
}
