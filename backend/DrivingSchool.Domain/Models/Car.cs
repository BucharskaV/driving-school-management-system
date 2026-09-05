using System.Text.RegularExpressions;

namespace DrivingSchool.Domain.Models;

public class Car
{
    public int Id { get; private init; }
    private string _brand;
    public string Brand
    {
        get => _brand;
        init
        {
            if(String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Brand cannot be empty");
            }
            if (value.Length > 50)
            {
                throw new ArgumentException("Brand cannot be longer than 50 characters");
            }
            _brand = value.Trim();
        }
    }
    
    private string _model;
    public string Model
    {
        get => _model;
        init
        {
            if(String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Model cannot be empty");
            }
            if (value.Length > 50)
            {
                throw new ArgumentException("Model cannot be longer than 50 characters");
            }
            _model = value.Trim();
        }
    }
    
    private string _registrationNumber;
    public string RegistrationNumber
    {
        get => _registrationNumber;
        set
        {
            if(String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Car registration number cannot be empty");
            }
            Regex codeRegex = new Regex("^[A-Z]{1,3}\\s?[A-Z0-9]{4,6}$");
            if (!codeRegex.IsMatch(value.Trim()))
            {
                throw new ArgumentException("Invalid registration car number.");
            }
            _registrationNumber = value.Trim();
        }
    }
    public virtual ICollection<PracticalLesson> PracticalLessons { get; set; } = [];
    
    private Car(){}
    public Car(string brand, string model, string registrationNumber)
    {
        Brand = brand;
        Model = model;
        RegistrationNumber = registrationNumber;
    }
}