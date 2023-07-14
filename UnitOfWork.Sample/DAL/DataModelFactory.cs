using UnitOfWork.Sample.DAL.DataModels;

namespace UnitOfWork.Sample.DAL;

public static class DataModelFactory
{
    public static TDataModel Create<TEntity, TDataModel>(TEntity entity)
        where TEntity : IEntity<Guid> where TDataModel : DataModel<Guid, TEntity>, new()
    {
        var dataModel = new TDataModel();
        dataModel.CloneFromEntity(entity);
        return dataModel;
    }

    public static TDataModel Create<TKey, TEntity, TDataModel>(TEntity entity)
        where TKey : notnull
        where TEntity : IEntity<TKey>
        where TDataModel : DataModel<TKey, TEntity>, new()
    {
        var dataModel = new TDataModel();
        dataModel.CloneFromEntity(entity);
        return dataModel;
    }
}
