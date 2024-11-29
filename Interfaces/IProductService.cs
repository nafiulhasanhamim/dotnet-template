using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_mvc.DTOs;

namespace dotnet_mvc.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductReadDto>> GetAllAsync();
        Task<ProductReadDto> GetByIdAsync(Guid id);
        Task<ProductReadDto> CreateAsync(ProductCreateDto productDto);
        Task<ProductReadDto> UpdateAsync(Guid id, ProductUpdateDto productDto);
        Task<bool> DeleteAsync(Guid id);
    }
}