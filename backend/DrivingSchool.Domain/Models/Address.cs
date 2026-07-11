namespace DrivingSchool.Domain.Models;

public class Address
{
    public int Id { get; private init; }
    public string City { get; set; }
    public string District { get; set; }
    public string Street { get; set; }
    public int HouseNumber { get; set; }
    
    private Address(){}
    public Address(string city, string district, string street, int houseNumber)
    {
        City = city;
        District = district;
        Street = street;
        HouseNumber = houseNumber;
    }
}