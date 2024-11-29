using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_mvc.DTOs;
using dotnet_mvc.Models;

namespace dotnet_mvc.Profiles
{
    public class CategoryProfile: Profile
    {
        public CategoryProfile() {
            CreateMap<Category, CategoryReadDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
        }   
    }
}