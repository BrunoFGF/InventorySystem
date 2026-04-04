using IS.Application.DTOs.Products;

namespace IS.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllByUserAsync(int userId);
        Task<ProductDto> GetByIdAsync(int id, int userId);
        Task<ProductDto> CreateAsync(int userId, CreateProductDto dto);
        Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto, int userId);
        Task DeleteAsync(int id, int userId);
    }
}
