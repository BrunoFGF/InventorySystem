using IS.Domain.Entities;
using IS.Domain.Interfaces;
using IS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IS.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(InventoryDbContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetActiveProductsByUserAsync(int userId)
        {
            return await _dbSet
                .Where(p => p.UserId == userId)
                .Include(p => p.ProductSuppliers)
                    .ThenInclude(ps => ps.Supplier)
                .OrderByDescending(p => p.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product?> GetProductWithSuppliersAsync(int id)
        {
            return await _dbSet
                .Include(p => p.ProductSuppliers)
                    .ThenInclude(ps => ps.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public void RemoveProductSuppliers(IEnumerable<ProductSupplier> suppliers)
        {
            _context.RemoveRange(suppliers);
        }
    }
}