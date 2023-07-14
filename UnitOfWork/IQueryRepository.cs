using System.Threading.Tasks;

namespace UnitOfWork
{
    public interface IQueryRepository<in TKey, out T>
    {
        T Get(TKey id);
    }

    public interface IAsyncQueryRepository<in TKey, T>
    {
        Task<T> GetAsync(TKey id);
    }
}
