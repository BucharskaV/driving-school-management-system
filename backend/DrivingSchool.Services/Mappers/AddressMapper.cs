using DrivingSchool.Domain.Models;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Mappers;

public class AddressMapper
{
    public static AddressDto MapToDto(Address address)
    {
        return new AddressDto(
            address.Id,
            address.City,
            address.District,
            address.Street,
            address.HouseNumber);
    }
}