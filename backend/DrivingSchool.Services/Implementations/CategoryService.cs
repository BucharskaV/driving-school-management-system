using DrivingSchool.Domain.Exceptions;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests.Category;
using DrivingSchool.Services.DTOs;
using DrivingSchool.Services.Interfaces;
using DrivingSchool.Services.Mappers;

namespace DrivingSchool.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);

        return categories
            .Select(CategoryMapper.MapToDto)
            .ToList();
    }

    public async Task<CategoryDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category == null)
            throw new CategoryNotFoundException(id);

        return CategoryMapper.MapToDto(category);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request,CancellationToken cancellationToken = default)
    {
        var category = new Category(request.Name, request.MinimumAge);

        await _categoryRepository.AddAsync(category, cancellationToken);

        return CategoryMapper.MapToDto(category);
    }

    public async Task<CategoryDto> UpdateAsync(int id,UpdateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category == null)
            throw new CategoryNotFoundException(id);

        category.Name = request.Name;
        category.MinimumAge = request.MinimumAge;

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return CategoryMapper.MapToDto(category);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category == null)
            throw new CategoryNotFoundException(id);

        await _categoryRepository.DeleteAsync(category, cancellationToken);
    }
}