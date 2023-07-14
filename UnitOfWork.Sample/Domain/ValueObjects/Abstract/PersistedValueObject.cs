namespace UnitOfWork.Sample.Domain.ValueObjects;

public readonly record struct PersistedValueObject<TKey, TValueObject>
    (TValueObject Object, TKey Id) : IValueObject, IEntity<TKey>
    where TValueObject : IValueObject where TKey : notnull
{
    public static implicit operator TValueObject(
        PersistedValueObject<TKey, TValueObject> valueObject) =>
        valueObject.Object;
}

public readonly record struct OwnedValue<TValueKey, TValue, TOwnerKey, TOwner>
    (TValueKey ValueKey, TOwnerKey OwnerKey)
    where TValue : IEntity<TValueKey>
    where TOwner : IEntity<TOwnerKey>
    where TValueKey : notnull
    where TOwnerKey : notnull
{
    public OwnedValue(TValue value, TOwner owner) : this(value.Id, owner.Id)
    {
        Value = value;
        Owner = owner;
    }

    public TValue Value { get; }
    public TOwner Owner { get; }

    public static implicit operator TValue(
        OwnedValue<TValueKey, TValue, TOwnerKey, TOwner> value) =>
        value.Value;

    public static implicit operator TOwner(
        OwnedValue<TValueKey, TValue, TOwnerKey, TOwner> value) =>
        value.Owner;
}
