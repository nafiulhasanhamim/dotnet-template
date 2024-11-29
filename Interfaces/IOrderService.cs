using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_mvc.DTOs;

namespace dotnet_mvc.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderReadDto>> GetAllOrdersAsync();
        Task<OrderReadDto?> GetOrderByIdAsync(Guid id);
        Task<OrderReadDto> CreateOrderAsync(OrderCreateDto orderDto, string userId);
    }
}