using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_mvc.Controllers;
using dotnet_mvc.DTOs;

namespace dotnet_mvc.Interfaces
{
    public interface ICategoryService
    {
        Task<PaginatedResult<CategoryReadDto>> GetAllCategories(int pageNumber, int pageSize, string? search = null, string? sortOrder = null);
        Task<CategoryReadDto?> GetCategoryById(Guid categoryId);
        Task<CategoryReadDto> CreateCategory(CategoryCreateDto categoryData);
        Task<CategoryReadDto?> UpdateCategoryById(Guid categoryId, CategoryUpdateDto categoryData);
        Task<bool> DeleteCategoryById(Guid categoryId);
    }
}