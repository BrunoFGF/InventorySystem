using IS.Domain.Interfaces;
using IS.Infrastructure.Persistence;

namespace IS.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InventoryDbContext _context;

        public IProductRepository Products { get; }
        public ISupplierRepository Suppliers { get; }
        public IUserRepository Users { get; }
        public IAuditLogRepository AuditLogs { get; }

        public UnitOfWork(InventoryDbContext context)
        {
            _context = context;
            Products = new ProductRepository(context);
            Suppliers = new SupplierRepository(context);
            Users = new UserRepository(context);
            AuditLogs = new AuditLogRepository(context);
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}