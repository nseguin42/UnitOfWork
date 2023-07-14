using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace UnitOfWork.Sample.DAL.Databases;

public abstract class Database<TDatabase> : UnitOfWork.Database<TDatabase>
    where TDatabase : Database<TDatabase>
{
    private readonly string _connectionString;

    protected Database(ISettingsService settingsService) =>
        _connectionString = settingsService.GetConnectionString(GetType().Name);

    public override DbConnection CreateConnection() => new SqlConnection(_connectionString);
}
