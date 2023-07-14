using System.Data;

namespace UnitOfWork.Sample.DAL;

public interface IDbContext : IUnitOfWork
{
    IDbConnection GetConnection<TDatabase>() where TDatabase : Database<TDatabase>;
    IDbTransaction GetTransaction<TDatabase>() where TDatabase : Database<TDatabase>;
}
