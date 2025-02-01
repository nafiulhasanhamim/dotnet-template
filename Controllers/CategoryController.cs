using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using dotnet_mvc.DTOs;
using dotnet_mvc.Interfaces;
using dotnet_mvc.Models;
using dotnet_mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_mvc.Controllers
{
    [ApiController]
    [Route("api/categories/")]
    public class CategoryController: ControllerBase
    {
        private ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger) {
            _categoryService = categoryService;
            _logger = logger;

        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5, [FromQuery] string? search = null, [FromQuery] string? sortOrder = null) {
            //  if(!string.IsNullOrEmpty(searchValue)) { 
            //     var searchCategories = categories.Where(c => !string.IsNullOrEmpty(c.Name) && c.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
            //     return Ok(searchCategories);
            //    }
            var categoryList = await _categoryService.GetAllCategories(PageNumber, PageSize, search, sortOrder);
            _logger.LogInformation("Fetching all categories...");
            // return Ok(categoryList);
            return ApiResponse.Success(categoryList, "Categories are returned succesfully");

        }

        [HttpGet("{categoryId:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid categoryId) {
            var category = await _categoryService.GetCategoryById(categoryId);
            if(category == null) { 
            return ApiResponse.NotFound("Category with this id is not found");
            }
            return ApiResponse.Success(category, "Categories with this id returned succesfully");     
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromQuery] CategoryCreateDto categoryData ) {
            if (!ModelState.IsValid) {
                return ApiResponse.BadRequest("Invalid category Data");
            }
       
            var categoryReadDto = await _categoryService.CreateCategory(categoryData);
        //  return Created($"/api/categories/{newCategory.CategoryId}", categoryReadDto);
         return ApiResponse.Created(categoryReadDto, "Category is created");
    }
        
       [Authorize(Roles ="Admin")]
       [HttpPut("{categoryId:guid}")]
        public async Task<IActionResult> UpdateCategoryById(Guid categoryId, [FromBody] CategoryUpdateDto categoryData) {
            var foundCategory = await _categoryService.UpdateCategoryById(categoryId, categoryData);
            if(foundCategory == null) {
                // return NotFound("Category with this id is not found");
                return ApiResponse.NotFound("Category with this id is not found");
           }

            if(categoryData == null) {
          // return NotFound("categoryData is missing");
             return ApiResponse.NotFound("categoryData is missing");
    }
    // // return NoContent();
          return ApiResponse.Success(foundCategory, "CategoryData is updated");
    }
       
        [Authorize(Roles ="Admin")]
        [HttpDelete("{categoryId:guid}")]
        public async Task<IActionResult> DeleteCategoryById(Guid categoryId) {
            var foundCategory = await _categoryService.DeleteCategoryById(categoryId);
            if(!foundCategory) {
                return NotFound("Category with this id is not found");
                }
                // return NoContent();
                return ApiResponse.Success<object>(null, "Successfully deleted");
        }
    
    
    }
    }
     