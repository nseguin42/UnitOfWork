using System;
using System.Collections.Generic;

namespace UnitOfWork.Extensions
{
    public static class RepositoryExtensions
    {
        public static bool InsertOrUpdate<TKey, TEntity>(
            this IRepository<TKey, TEntity> repo,
            TEntity entity) where TEntity : IEntity<TKey>
        {
            var entityFromDb = repo.Get(entity.Id);
            if (entityFromDb is null)
            {
                repo.Insert(entity);
                return true;
            }

            repo.Update(entity);
            return false;
        }

        public static void InsertOrUpdate<TKey, TEntity>(
            this IRepository<TKey, TEntity> repo,
            IEnumerable<TEntity> entities) where TEntity : IEntity<TKey>
        {
            foreach (var entity in entities)
            {
                InsertOrUpdate(repo, entity);
            }
        }

        public static TEntity GetOrInsert<TKey, TEntity>(
            this IRepository<TKey, TEntity> repo,
            TEntity entity) where TEntity : IEntity<TKey>
        {
            var entityFromDb = repo.Get(entity.Id);
            if (entityFromDb is null)
            {
                repo.Insert(entity);
                return entity;
            }

            return entityFromDb;
        }

        public static TEntity GetOrInsert<TKey, TEntity>(
            this IRepository<TKey, TEntity> repo,
            TKey id,
            Func<TKey, TEntity> entityFactory) where TEntity : IEntity<TKey>
        {
            var entity = entityFactory(id);
            return GetOrInsert(repo, entity);
        }

        public static IEnumerable<TEntity> GetOrInsert<TKey, TEntity>(
            this IRepository<TKey, TEntity> repo,
            IEnumerable<TEntity> entities) where TEntity : IEntity<TKey>
        {
            var results = new List<TEntity>();
            foreach (var entity in entities)
            {
                var result = GetOrInsert(repo, entity);
                results.Add(result);
            }

            return results;
        }
    }
}
