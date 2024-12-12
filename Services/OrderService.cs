using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_mvc.data;
using dotnet_mvc.DTOs;
using dotnet_mvc.Interfaces;
using dotnet_mvc.Models;
using dotnet_mvc.SignalRHub;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
namespace dotnet_mvc.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IHubContext<DemoHub> _hubContext1;

        public OrderService(AppDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager,
         IHubContext<NotificationHub> hubContext,
         IHubContext<DemoHub> hubContext1
         )
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _hubContext = hubContext;
            _hubContext1 = hubContext1;
        }
        // Get all orders with related data
        public async Task<IEnumerable<OrderReadDto>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderedProducts)
                    .ThenInclude(op => op.Product)
                        .ThenInclude(p => p.Category)
                .Include(o => o.User)
                .ToListAsync();

            return _mapper.Map<IEnumerable<OrderReadDto>>(orders);
        }

        public async Task<OrderReadDto?> GetOrderByIdAsync(Guid id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderedProducts)
                    .ThenInclude(op => op.Product)
                        .ThenInclude(p => p.Category)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            return order == null ? null : _mapper.Map<OrderReadDto>(order);
        }

        public async Task<OrderReadDto> CreateOrderAsync(OrderCreateDto orderDto, string userId)
        {
            var order = _mapper.Map<Order>(orderDto);
            // order.UserId = orderDto.UserId;
            order.UserId = userId;
            order.OrderId = Guid.NewGuid();
            order.OrderDate = DateTime.UtcNow;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var orderedProductDto in orderDto.OrderedProducts)
            {
                var orderedProduct = _mapper.Map<OrderedProduct>(orderedProductDto);
                orderedProduct.OrderId = order.OrderId;
                orderedProduct.OrderedProductId = Guid.NewGuid();
                _context.OrderedProducts.Add(orderedProduct);
            }

            await _context.SaveChangesAsync();

            var createdOrder = await _context.Orders
                .Include(o => o.OrderedProducts)
                    .ThenInclude(op => op.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

            await _hubContext1.Clients.All.SendAsync("ReceiveMessage", "connected");
            await NotifyUser(orderDto);
            // await _hubContext.Clients.All.SendAsync("ReceiveMessage", orderDto);
            return _mapper.Map<OrderReadDto>(createdOrder);
        }

        public async Task NotifyUser(OrderCreateDto orderDto)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", orderDto);
        }

    }
}