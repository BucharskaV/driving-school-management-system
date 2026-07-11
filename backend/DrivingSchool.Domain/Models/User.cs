using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DrivingSchool.Domain.Models;

public class User
{
    public int Id { get; private init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Pesel { get; init; }
    public string? Email { get; set; }
    public string PhoneNumber { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    protected User(){}
    public User(string firstName, string lastName, string pesel, string phoneNumber, string? email = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Pesel = pesel; 
        PhoneNumber = phoneNumber;
        Email = email;
    }
}