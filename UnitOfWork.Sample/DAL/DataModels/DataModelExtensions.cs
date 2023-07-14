using UnitOfWork.Sample.Domain.ValueObjects;

namespace UnitOfWork.Sample.DAL.DataModels;

public static class DataModelExtensions
{
    public static IEnumerable<TEntity> ToEntity<TDataModel, TKey, TEntity>(
        this IEnumerable<TDataModel> dataModels)
        where TDataModel : DataModel<TKey, TEntity>
        where TKey : notnull
        where TEntity : class, IEntity<TKey>
    {
        return dataModels.Select(dataModel => dataModel.ToEntity());
    }

    public static IEnumerable<TEntity> ToEntity<TDataModel, TEntity>(
        this IEnumerable<TDataModel> dataModels)
        where TDataModel : DataModel<TEntity> where TEntity : class, IEntity
    {
        return dataModels.Select(dataModel => dataModel.ToEntity());
    }

    public static IEnumerable<TValueObject> ToValueObject<TDataModel, TKey, TValueObject>(
        this IEnumerable<TDataModel> dataModels)
        where TDataModel : ValueDataModel<TKey, TValueObject>
        where TKey : notnull
        where TValueObject : IValueObject
    {
        return dataModels.Select(dataModel => dataModel.ToValueObject());
    }

    public static IEnumerable<TValueObject> ToValueObject<TDataModel, TValueObject>(
        this IEnumerable<TDataModel> dataModels)
        where TDataModel : ValueDataModel<TValueObject> where TValueObject : IValueObject
    {
        return dataModels.Select(dataModel => dataModel.ToValueObject());
    }
}
