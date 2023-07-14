namespace UnitOfWork.Sample.Domain.ValueObjects;

public static class ValueObjectExtensions
{
    public static PersistedValueObject<TKey, TValueObject> WithPersistenceId<TKey, TValueObject>(
        this TValueObject valueObject,
        TKey id) where TKey : notnull where TValueObject : IValueObject
    {
        if (valueObject is PersistedValueObject<TKey, TValueObject> persistedValueObject)
        {
            return persistedValueObject;
        }

        return new PersistedValueObject<TKey, TValueObject>(valueObject, id);
    }
}