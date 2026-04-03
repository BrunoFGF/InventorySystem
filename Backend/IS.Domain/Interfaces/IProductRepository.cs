using IS.Domain.Entities;

namespace IS.Domain.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetActiveProductsByUserAsync(int userId);
        Task<Product?> GetProductWithSuppliersAsync(int id);
    }
}