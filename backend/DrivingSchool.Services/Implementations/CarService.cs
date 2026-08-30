using DrivingSchool.Domain.Exceptions;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests.Car;
using DrivingSchool.Services.DTOs;
using DrivingSchool.Services.Interfaces;
using DrivingSchool.Services.Mappers;

namespace DrivingSchool.Services.Implementations;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;

    public CarService(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task<List<CarDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var cars = await _carRepository.GetAllAsync(cancellationToken);

        return cars
            .Select(CarMapper.MapToDto)
            .ToList();
    }

    public async Task<CarDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var car = await _carRepository.GetByIdAsync(id, cancellationToken);
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

        await _carRepository.AddAsync(car, cancellationToken);

        return CarMapper.MapToDto(car);
    }

    public async Task<CarDto> UpdateAsync(int id, UpdateCarRequest request, CancellationToken cancellationToken = default)
    {
        var car = await _carRepository.GetByIdAsync(id, cancellationToken);
        if (car == null)
            throw new CarNotFoundException(id);

        car.RegistrationNumber = request.RegistrationNumber;
        await _carRepository.UpdateAsync(car, cancellationToken);

        return CarMapper.MapToDto(car);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var car = await _carRepository.GetByIdAsync(id, cancellationToken);
        if (car == null)
            throw new CarNotFoundException(id);

        await _carRepository.DeleteAsync(car, cancellationToken);
    }
}