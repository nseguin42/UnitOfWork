namespace UnitOfWork.Sample.Domain.ValueObjects;

public record Address(
    string Street,
    string City,
    string PostalCode,
    string StateCode,
    string? Unit = null,
    string? ExtraDetail = null) : BaseValueObject
{
    public IEnumerable<string> GetAddressLines()
    {
        if (!string.IsNullOrWhiteSpace(Unit))
        {
            yield return $"{Street} {Unit}";
        }
        else
        {
            yield return Street;
        }

        yield return $"{City}, {StateCode} {PostalCode}";
        if (!string.IsNullOrWhiteSpace(ExtraDetail))
        {
            yield return ExtraDetail;
        }
    }
}
