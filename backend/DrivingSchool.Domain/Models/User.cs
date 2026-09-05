using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Models;

public class User
{
    public int Id { get; private init; }
    private string _firstName;
    public string FirstName
    {
        get => _firstName;
        set
        {
            if(String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Firstname cannot be empty");
            }
            if (value.Length > 50)
            {
                throw new ArgumentException("First name cannot be longer than 50 characters");
            }
            _firstName = value.Trim();
        }
    }
    
    private string _lastName;
    public string LastName
    {
        get => _lastName;
        set
        {
            if(String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Lastname cannot be empty");
            }
            if (value.Length > 50)
            {
                throw new ArgumentException("Last name cannot be longer than 50 characters");
            }
            _lastName = value.Trim();
        }
    }
    public Role Role { get; init; }
    private string _pesel;
    public string Pesel
    {
        get => _pesel;
        init
        {
            if(String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Pesel number cannot be empty");
            }
            Regex codeRegex = new Regex("^\\d{11}$");
            if (!codeRegex.IsMatch(value.Trim()))
            {
                throw new ArgumentException("Invalid PESEL number. The PESEL consists of 11 digits.");
            }
            _pesel = value.Trim();
        }
    }
    
    private string? _email;
    public string? Email
    {
        get => _email;
        set
        {
            if (value != null && value.Trim() == "")
            {
                throw new ArgumentException("Email cannot be empty.");
            }
            Regex validateEmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");
            if (value != null && !validateEmailRegex.IsMatch(value))
            {
                throw new ArgumentException("Invalid email address.");
            }
            if (value!=null && value.Length > 50)
            {
                throw new ArgumentException("Email cannot be longer than 50 characters");
            }
            _email = value;
        }
    }
    
    private string _phoneNumber;
    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if(String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Phone number cannot be empty");
            }
            Regex codeRegex = new Regex("^(\\+48)?\\d{9}$");
            if (!codeRegex.IsMatch(value.Trim()))
            {
                throw new ArgumentException("Invalid phone number. The phone number consists of 9 digits.");
            }
            _phoneNumber = value.Trim();
        }
    }
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