namespace UnitOfWork.Sample.DAL.Databases;

public abstract class MasterDataStore : Database<MasterDataStore>
{
    public MasterDataStore(ISettingsService settingsService) : base(settingsService)
    {
    }
}
