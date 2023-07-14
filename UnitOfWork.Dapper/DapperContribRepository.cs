using System;
using System.Data;
using Dapper.Contrib.Extensions;

namespace UnitOfWork.Dapper
{
    public class DapperContribRepository<TKey, T> : BaseRepository<TKey, T> where T : class, new()
    {
        public DapperContribRepository(Func<IDbTransaction> transactionFactory) : base(
            transactionFactory)
        {
        }

        public override T Get(TKey id) => Connection.Get<T>(id, Transaction);

        public override void Insert(T entity) => Connection.Insert(entity, Transaction);

        public override void Update(T entity) => Connection.Update(entity, Transaction);

        public override void Delete(T entity) => Connection.Delete(entity, Transaction);
    }
}
