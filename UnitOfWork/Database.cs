using System.Data;

namespace UnitOfWork
{
    public abstract class Database<TDatabase> where TDatabase : Database<TDatabase>
    {
        public abstract IDbConnection CreateConnection();
    }
}
