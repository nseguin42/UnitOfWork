using System;

namespace UnitOfWork.Extensions
{
    public static class QueryRepositoryExtensions
    {
        public static T GetOrThrow<TKey, T>(this IQueryRepository<TKey, T> repo, TKey id)
        {
            var entity = repo.Get(id);
            if (entity is null)
            {
                throw new InvalidOperationException(
                    $"Entity of type {typeof(T).Name} with id {id} does not exist.");
            }

            return entity;
        }
    }
}
