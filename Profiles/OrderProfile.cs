using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_mvc.DTOs;
using dotnet_mvc.Models;

namespace dotnet_mvc.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Map Order -> OrderReadDto
            CreateMap<Order, OrderReadDto>();
            // .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserDto { UserId = src.UserId, Email = src.User.Email }));
            CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));



            // Map OrderedProduct -> OrderedProductReadDto
            CreateMap<OrderedProduct, OrderedProductReadDto>();
            // .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))

            // Map OrderCreateDto -> Order
            CreateMap<OrderCreateDto, Order>()
                .ForMember(dest => dest.OrderedProducts, opt => opt.Ignore()); // Will handle products separately

            // Map OrderedProductCreateDto -> OrderedProduct
            CreateMap<OrderedProductCreateDto, OrderedProduct>();

            // Map OrderUpdateDto -> Order
            CreateMap<OrderUpdateDto, Order>()
                .ForMember(dest => dest.OrderedProducts, opt => opt.Ignore()); // Will handle updates to products separately
        }

    }
}