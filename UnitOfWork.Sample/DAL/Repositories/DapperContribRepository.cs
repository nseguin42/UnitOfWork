namespace UnitOfWork.Sample.DAL.Repositories;

public class
    DapperContribRepository<TDatabase, TEntity> : Dapper.DapperContribRepository<Guid, TEntity>
    where TDatabase : Database<TDatabase> where TEntity : class, IEntity<Guid>, new()
{
    public DapperContribRepository(IDbContext context) : base(context.GetTransaction<TDatabase>)
    {
    }
}
