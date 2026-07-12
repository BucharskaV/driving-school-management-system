namespace DrivingSchool.Services.DTOs;

public record AddressDto(
     int Id,
     string City,
     string District,
     string Street,
     int HouseNumber
);