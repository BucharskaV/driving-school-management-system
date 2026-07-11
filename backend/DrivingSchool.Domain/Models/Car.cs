using System.Text.RegularExpressions;

namespace DrivingSchool.Domain.Models;

public class Car
{
    public int Id { get; private init; }
    public string Brand { get; init; }
    public string Model { get; init; }
    public string RegistrationNumber { get; set; }
    public virtual ICollection<PracticalLesson> PracticalLessons => [];
    
    private Car(){}
    public Car(string brand, string model, string registrationNumber)
    {
        Brand = brand;
        Model = model;
        RegistrationNumber = registrationNumber;
    }
}