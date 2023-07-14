namespace UnitOfWork
{
    public interface IRepository<in TKey, T> : ICommandRepository<T>, IQueryRepository<TKey, T>
    {
    }

    public interface IAsyncRepository<in TKey, T> : IAsyncCommandRepository<T>,
        IAsyncQueryRepository<TKey, T>
    {
    }
}
