using System.Collections.ObjectModel;
using UnitOfWork.Sample.Domain.ValueObjects;

namespace UnitOfWork.Sample.Domain.Models;

public class Person : BaseDomainModel
{
    private readonly List<Address>? _addresses;

    public Person(
        string firstName,
        string lastName,
        Address? primaryAddress = null,
        IEnumerable<Address>? addresses = null)
    {
        FirstName = firstName;
        LastName = lastName;
        PrimaryAddress = primaryAddress;
        _addresses = addresses?.ToList();
    }

    public string FirstName { get; }
    public string LastName { get; }

    public Address? PrimaryAddress { get; }

    public IReadOnlyCollection<Address> Addresses =>
        _addresses?.AsReadOnly() ?? ReadOnlyCollection<Address>.Empty;
}
