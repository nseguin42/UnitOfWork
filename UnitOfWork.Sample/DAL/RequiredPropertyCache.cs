using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace UnitOfWork.Sample.DAL;

internal static class RequiredPropertyCache<TEntity>
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly Lazy<ConcurrentQueue<PropertyInfo>> RequiredProperties = new(
        () => new ConcurrentQueue<PropertyInfo>(GetRequiredPropertiesByReflection()));

    private static IEnumerable<PropertyInfo> GetRequiredPropertiesByReflection() =>
        typeof(TEntity).GetProperties()
            .Where(p => p.GetCustomAttribute<RequiredAttribute>() != null);
}
