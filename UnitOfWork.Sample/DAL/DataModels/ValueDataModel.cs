using UnitOfWork.Sample.Domain.ValueObjects;

namespace UnitOfWork.Sample.DAL.DataModels;

public abstract record
    ValueDataModel<TKey, TValueObject> : DataModel<TKey, PersistedValueObject<TKey, TValueObject>>
    where TKey : notnull where TValueObject : IValueObject
{
    public override PersistedValueObject<TKey, TValueObject> ToEntity() => new(ToValueObject(), Id);

    public override void CloneFromEntity(PersistedValueObject<TKey, TValueObject> entity)
    {
        CloneFromEntity(entity.Object);
        Id = entity.Id;
    }

    public abstract TValueObject ToValueObject();

    public abstract void CloneFromEntity(TValueObject entity);
}
