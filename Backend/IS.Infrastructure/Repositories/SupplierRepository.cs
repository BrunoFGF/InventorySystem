using IS.Domain.Entities;
using IS.Domain.Interfaces;
using IS.Infrastructure.Persistence;

namespace IS.Infrastructure.Repositories
{
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(InventoryDbContext context) : base(context) { }
    }
}