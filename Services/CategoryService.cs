using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_mvc.Controllers;
using dotnet_mvc.data;
using dotnet_mvc.DTOs;
using dotnet_mvc.Enums;
using dotnet_mvc.Interfaces;
using dotnet_mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_mvc.Services
{
    public class CategoryService: ICategoryService
    {
        // private static readonly List<Category> _categories = new List<Category>();
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext appDbContext, IMapper mapper) {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        public async Task<PaginatedResult<CategoryReadDto>> GetAllCategories(int pageNumber, int pageSize, string? search = null, string? sortOrder = null) {
            // return _categories.Select(c => new CategoryReadDto {
            //     CategoryId = c.CategoryId,
            //     Name = c.Name,
            //     Description = c.Description
            //     // CreatedAt = c.CreatedAt
            // }).ToList();

            IQueryable<Category> query = _appDbContext.Categories;

            if(!string.IsNullOrWhiteSpace(search)) {
                var formattedSearch = $"%{search.Trim()}%";
                query = query.Where(c => EF.Functions.ILike(c.Name, formattedSearch) || EF.Functions.ILike(c.Description, formattedSearch));
            }

            if(string.IsNullOrWhiteSpace(sortOrder)) {
                query = query.OrderBy(c => c.Name);
            } else {
               var formattedSortOrder = sortOrder.Trim().ToLower();
               if(Enum.TryParse<SortOrder>(formattedSortOrder, true, out var parsedSortOrder)) {

                query = parsedSortOrder switch {
                 SortOrder.NameAsc => query.OrderBy(c => c.Name),
                 SortOrder.NameDesc => query.OrderByDescending(c => c.Name),
                 SortOrder.CreatedAtAsc => query.OrderBy(c => c.CreatedAt),
                 SortOrder.CreatedAtDesc => query.OrderByDescending(c => c.CreatedAt),
                  _ => query.OrderBy(c => c.Name),
                };
               }
            }

            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            // var categories = await _appDbContext.Categories.ToListAsync();
            var results = _mapper.Map<List<CategoryReadDto>>(items);
            return new PaginatedResult<CategoryReadDto> {
                Items = results,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<CategoryReadDto?> GetCategoryById(Guid categoryId) {
            
            // var foundCategory = _categories.FirstOrDefault(c => c.CategoryId == categoryId);
            
            // if(foundCategory == null) {
            //     return null;
            // }
            // return new CategoryReadDto {
            //     CategoryId = foundCategory.CategoryId,
            //     Name = foundCategory.Name,
            //     Description = foundCategory.Description
            // };
            // var foundCategory = await _appDbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
            var foundCategory = await _appDbContext.Categories.FindAsync(categoryId);
            return foundCategory == null ? null : _mapper.Map<CategoryReadDto>(foundCategory);
        }

        public async Task<CategoryReadDto> CreateCategory(CategoryCreateDto categoryData) {
            // var newCategory = new Category {
            // CategoryId = Guid.NewGuid(),
            // Name = categoryData.Name,
            // Description = categoryData.Description,
            // CreatedAt = DateTime.UtcNow,
            // };
            
            var newCategory = _mapper.Map<Category>(categoryData);
            newCategory.CategoryId = Guid.NewGuid();
            newCategory.CreatedAt = DateTime.UtcNow;

            await _appDbContext.Categories.AddAsync(newCategory);
            await _appDbContext.SaveChangesAsync();
       
        //     return new CategoryReadDto {
        //     CategoryId = newCategory.CategoryId,
        //     Name = newCategory.Name,
        //     Description = newCategory.Description
        // };
        
        return _mapper.Map<CategoryReadDto>(newCategory);

    }
        public async Task<CategoryReadDto?> UpdateCategoryById(Guid categoryId, CategoryUpdateDto categoryData) {
            // var foundCategory = _categories.FirstOrDefault(category => category.CategoryId == categoryId);
            // var foundCategory = await _appDbContext.Categories.FirstOrDefaultAsync(category => category.CategoryId == categoryId);
            var foundCategory = await _appDbContext.Categories.FindAsync(categoryId);
            if(foundCategory == null) {
                return null;
           }

        //    if(!string.IsNullOrWhiteSpace(categoryData.Name)) {
        //     foundCategory.Name = categoryData.Name;
        //     }

        //    if(!string.IsNullOrWhiteSpace(categoryData.Description)) {
        //     foundCategory.Description = categoryData.Description;
        //     }

            _mapper.Map(categoryData, foundCategory);
            _appDbContext.Categories.Update(foundCategory);
            await _appDbContext.SaveChangesAsync();
            return _mapper.Map<CategoryReadDto>(foundCategory);


    }

        public async Task<bool> DeleteCategoryById(Guid categoryId) {
        // var foundCategory = _categories.FirstOrDefault(category => category.CategoryId == categoryId);
        // var foundCategory = await _appDbContext.Categories.FirstOrDefaultAsync(category => category.CategoryId == categoryId);
        var foundCategory = await _appDbContext.Categories.FindAsync(categoryId);
        if(foundCategory == null) {
            return false;
            }
            _appDbContext.Categories.Remove(foundCategory);
            await _appDbContext.SaveChangesAsync();
            return true;


    }

}

}