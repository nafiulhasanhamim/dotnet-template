using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_mvc.DTOs;
using dotnet_mvc.Models;

namespace dotnet_mvc.Profiles
{
    public class ProductProfile: Profile
    {
        public ProductProfile()
    {
        // Mapping for ProductReadDto
        CreateMap<Product, ProductReadDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

        // Mapping for ProductCreateDto -> Product
        CreateMap<ProductCreateDto, Product>();

        // Mapping for ProductUpdateDto -> Product
        CreateMap<ProductUpdateDto, Product>();

    }

    }
}