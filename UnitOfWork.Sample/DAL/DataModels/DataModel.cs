using System.Collections.Concurrent;
using System.Reflection;
using UnitOfWork.Sample.Domain.ValueObjects;

namespace UnitOfWork.Sample.DAL.DataModels;

public abstract record DataModel<TKey, TEntity> : IEntity<TKey>
    where TKey : notnull where TEntity : IEntity<TKey>
{
    private static readonly Lazy<ConcurrentQueue<PropertyInfo>> RequiredProperties
        = new(() => RequiredPropertyCache<TEntity>.RequiredProperties.Value);

    protected DataModel() : this(default!)
    {
    }

    public TKey Id { get; set; } = default!;

    public virtual bool IsValid() => GetRequiredDependencyProperties().All(p => p != null);

    public abstract TEntity ToEntity();

    public abstract void CloneFromEntity(TEntity entity);

    public bool TryToEntity(out TEntity? entity)
    {
        if (IsValid())
        {
            entity = ToEntity();
            return true;
        }

        entity = default;
        return false;
    }

    protected virtual IEnumerable<object?> GetRequiredDependencyProperties() =>
        RequiredProperties.Value.Select(p => p.GetValue(this));
}

public abstract record DataModel<TEntity> : DataModel<Guid, TEntity> where TEntity : IEntity;

public abstract record ValueDataModel<TValueObject> : ValueDataModel<Guid, TValueObject>
    where TValueObject : IValueObject;
