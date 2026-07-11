using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Models;

public class User
{
    public int Id { get; private init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Role Role { get; init; }
    public string Pesel { get; init; }
    public string? Email { get; set; }
    public string PhoneNumber { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    protected User(){}
    public User(string firstName, string lastName, Role role, string pesel, string phoneNumber, string? email = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        Pesel = pesel; 
        PhoneNumber = phoneNumber;
        Email = email;
    }
}