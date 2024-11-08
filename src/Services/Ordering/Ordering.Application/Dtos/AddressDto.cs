namespace Ordering.Application.Dtos;

public record AddressDto(
    string FirstName,
    string LastName,
    string EmailAddress,
    string AddressLine,
    string Country,
    string State,
    string ZipCode);

public static class AddressDtoExtensions
{
    public static Address ToEntity(this AddressDto addressDto)
    {
        return Address.From(
            addressDto.FirstName,
            addressDto.LastName,
            addressDto.EmailAddress,
            addressDto.AddressLine,
            addressDto.Country,
            addressDto.State,
            addressDto.ZipCode);
    }
}
