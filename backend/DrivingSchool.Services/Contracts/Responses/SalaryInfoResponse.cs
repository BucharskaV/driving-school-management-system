namespace DrivingSchool.Services.Contracts.Responses;

public record SalaryInfoResponse(decimal BaseSalary, decimal? Bonus, decimal TotalSalary);