namespace DrivingSchool.Services.Contracts.Requests.Auth;

public record RegisterRequest(string FirstName, string LastName, string Pesel, string PhoneNumber, string Email, string Password);
