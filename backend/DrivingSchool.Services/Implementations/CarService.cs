using DrivingSchool.Domain.Exceptions;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests.Car;
using DrivingSchool.Services.DTOs;
using DrivingSchool.Services.Interfaces;
using DrivingSchool.Services.Mappers;

namespace DrivingSchool.Services.Implementations;

public class CarService(ICarRepository carRepository) : ICarService
{
    public async Task<List<CarDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var cars = await carRepository.GetAllAsync(cancellationToken);

        return cars
            .Select(CarMapper.MapToDto)
            .ToList();
    }

    public async Task<CarDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var car = await carRepository.GetByIdAsync(id, cancellationToken);
        if (car == null)
            throw new CarNotFoundException(id);

        return CarMapper.MapToDto(car);
    }

    public async Task<CarDto> CreateAsync(CreateCarRequest request, CancellationToken cancellationToken = default)
    {
        var car = new Car(
            request.Brand,
            request.Model,
            request.RegistrationNumber);

        await carRepository.AddAsync(car, cancellationToken);

        return CarMapper.MapToDto(car);
    }

    public async Task<CarDto> UpdateAsync(int id, UpdateCarRequest request, CancellationToken cancellationToken = default)
    {
        var car = await carRepository.GetByIdAsync(id, cancellationToken);
        if (car == null)
            throw new CarNotFoundException(id);

        car.RegistrationNumber = request.RegistrationNumber;
        await carRepository.UpdateAsync(car, cancellationToken);

        return CarMapper.MapToDto(car);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var car = await carRepository.GetByIdAsync(id, cancellationToken);
        if (car == null)
            throw new CarNotFoundException(id);

        await carRepository.DeleteAsync(car, cancellationToken);
    }
}