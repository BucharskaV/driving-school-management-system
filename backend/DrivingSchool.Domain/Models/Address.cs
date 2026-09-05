namespace DrivingSchool.Domain.Models;

public class Address
{
    public int Id { get; private init; }
    private string _city;
    public string City
    {
        get => _city;
        set
        {
            if(String.IsNullOrEmpty(value))
                throw new ArgumentNullException("City cannot be empty");
            if (value.Length > 50)
            {
                throw new ArgumentException("City name cannot be longer than 50 characters");
            }
            _city = value;
        }
    }
    
    private string _district;
    public string District
    {
        get => _district;
        set
        {
            if(String.IsNullOrEmpty(value))
                throw new ArgumentException("District cannot be empty");
            if (value.Length > 50)
            {
                throw new ArgumentException("District name cannot be longer than 50 characters");
            }
            _district = value;
        }
    }
    
    private string _street;
    public string Street
    {
        get => _street;
        set
        {
            if(String.IsNullOrEmpty(value))
                throw new ArgumentException("Street cannot be empty");
            if (value.Length > 50)
            {
                throw new ArgumentException("Street name cannot be longer than 50 characters");
            }
            _street = value;
        }
    }
    
    private int _houseNumber;
    public int HouseNumber
    {
        get => _houseNumber;
        set
        {
            if(value <= 0) throw new ArgumentOutOfRangeException("House number must be greater than 0.");
            _houseNumber = value;
        }
    }
    
    private Address(){}
    public Address(string city, string district, string street, int houseNumber)
    {
        City = city;
        District = district;
        Street = street;
        HouseNumber = houseNumber;
    }
}