using System.Collections.Generic;

namespace UnitOfWork.Extensions
{
    public static class CommandRepositoryExtensions
    {
        public static void Insert<TKey, TEntity>(
            this ICommandRepository<TEntity> repo,
            IEnumerable<TEntity> entities) where TEntity : IEntity<TKey>
        {
            foreach (var entity in entities)
            {
                repo.Insert(entity);
            }
        }

        public static void Update<TKey, TEntity>(
            this ICommandRepository<TEntity> repo,
            IEnumerable<TEntity> entities) where TEntity : IEntity<TKey>
        {
            foreach (var entity in entities)
            {
                repo.Update(entity);
            }
        }

        public static void Delete<TKey, TEntity>(
            this ICommandRepository<TEntity> repo,
            IEnumerable<TEntity> entities) where TEntity : IEntity<TKey>
        {
            foreach (var entity in entities)
            {
                repo.Delete(entity);
            }
        }
    }
}
