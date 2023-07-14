using System.Collections.Concurrent;
using System.Data;
using Microsoft.Data.SqlClient;

namespace UnitOfWork.Sample.DAL;

public class DbContext : IDbContext
{
    private readonly Lazy<WorkQueue<IDbTransaction>> _transactions;
    private readonly ConcurrentDictionary<Type, IDbConnection> _connections = new();
    private bool _isDisposed;

    public DbContext(ISettingsService settingsService)
    {
        SettingsService = settingsService;
        _transactions = new Lazy<WorkQueue<IDbTransaction>>(
            () => new WorkQueue<IDbTransaction>(Connections.Select(c => c.BeginTransaction())));
    }

    protected ISettingsService SettingsService { get; }

    protected IEnumerable<IDbConnection> Connections => _connections.Values;
    protected IReadOnlyCollection<IDbTransaction> Transactions => _transactions.Value;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IDbConnection GetConnection<TDatabase>() where TDatabase : Database<TDatabase> =>
        _connections.GetOrAdd(typeof(TDatabase), CreateConnection<TDatabase>());

    public IDbTransaction GetTransaction<TDatabase>() where TDatabase : Database<TDatabase>
    {
        var index = _connections.Keys.ToList().IndexOf(typeof(TDatabase));
        return ((IEnumerable<IDbTransaction>)_transactions.Value).ElementAt(index);
    }

    public void Commit() => _transactions.Value.Commit();

    public void Rollback() => _transactions.Value.Rollback();

    protected virtual IDbConnection CreateConnection<TDatabase>() =>
        new SqlConnection(SettingsService.GetConnectionString(typeof(TDatabase).Name));

    protected virtual void Dispose(bool isDisposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (isDisposing)
        {
            _transactions.Value.Dispose();
            foreach (var connection in _connections.Values)
            {
                connection.Dispose();
            }
        }

        _isDisposed = true;
    }
}
