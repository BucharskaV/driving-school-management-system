using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchool.Infrastructure.Repositories;

public class CarRepository(ApplicationDbContext context) : ICarRepository
{
    public async Task<Car?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Cars.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<List<Car>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Cars.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Car car, CancellationToken cancellationToken = default)
    {
        await context.Cars.AddAsync(car, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Car car, CancellationToken cancellationToken = default)
    {
        context.Cars.Update(car);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Car car, CancellationToken cancellationToken = default)
    {
        context.Cars.Remove(car);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IsCarAvailableAsync(int carId, DateTime start, DateTime end, CancellationToken cancellationToken = default)
    {
        return !await context.LessonProgresses
            .Where(lp => lp.Lesson is PracticalLesson)
            .AnyAsync(lp =>
                ((PracticalLesson)lp.Lesson).CarId == carId &&
                lp.ProgressStatus == ProgressStatus.Booked &&
                lp.StartTime < end && lp.EndTime > start,
                cancellationToken);
    }
}