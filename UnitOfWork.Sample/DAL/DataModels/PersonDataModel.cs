using System.ComponentModel.DataAnnotations;
using UnitOfWork.Sample.Domain.Models;
using UnitOfWork.Sample.Domain.ValueObjects;

namespace UnitOfWork.Sample.DAL.DataModels;

public record PersonDataModel : DataModel<Person>
{
    public PersonDataModel()
    {
    }

    public PersonDataModel(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    [Required]
    public AddressDataModel? PrimaryAddress { get; set; }

    public List<AddressDataModel>? Addresses { get; set; }

    public override Person ToEntity() =>
        new(
            FirstName,
            LastName,
            PrimaryAddress?.ToValueObject(),
            Addresses?.Select(a => a.ToValueObject()));

    public override void CloneFromEntity(Person entity)
    {
        Id = entity.Id;
        FirstName = entity.FirstName;
        LastName = entity.LastName;
        PrimaryAddress = new AddressDataModel(entity.PrimaryAddress);
        Addresses = entity.Addresses?.Select(a => new AddressDataModel(a)).ToList();
    }
}
