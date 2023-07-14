using System.Threading.Tasks;

namespace UnitOfWork
{
    public interface ICommandRepository<in T>
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }

    public interface IAsyncCommandRepository<in T>
    {
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
