using UnitOfWork.Sample.Domain.ValueObjects;

namespace UnitOfWork.Sample.DAL.DataModels;

public record AddressDataModel : ValueDataModel<Guid, Address>
{
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string StateCode { get; set; } = null!;
    public string? Unit { get; set; }
    public string? ExtraDetail { get; set; }

    public AddressDataModel()
    {
    }

    public AddressDataModel(Address address)
    {
        Street = address.Street;
        City = address.City;
        PostalCode = address.PostalCode;
        StateCode = address.StateCode;
        Unit = address.Unit;
        ExtraDetail = address.ExtraDetail;
    }

    public override Address ToValueObject()
    {
        return new(Street, City, PostalCode, StateCode, Unit, ExtraDetail);
    }

    public override void CloneFromEntity(Address entity)
    {
        Street = entity.Street;
        City = entity.City;
        PostalCode = entity.PostalCode;
        StateCode = entity.StateCode;
        Unit = entity.Unit;
        ExtraDetail = entity.ExtraDetail;
    }
}
