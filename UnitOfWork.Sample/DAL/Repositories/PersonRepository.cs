using Dapper;
using UnitOfWork.Sample.DAL.Databases;
using UnitOfWork.Sample.DAL.DataModels;

namespace UnitOfWork.Sample.DAL.Repositories;

public interface IPersonRepository : IRepository<Guid, PersonDataModel>
{
    IEnumerable<PersonDataModel> GetFirst10();
}

public class PersonRepository : DapperContribRepository<MasterDataStore, PersonDataModel>, IPersonRepository
{
    public PersonRepository(IDbContext context) : base(context)
    {
    }

    public IEnumerable<PersonDataModel> GetFirst10()
    {
        const string sql = @"SELECT TOP 10 p.* FROM Person p";
        var result = Connection.Query<PersonDataModel>(sql, transaction: Transaction);
        return result;
    }
}
