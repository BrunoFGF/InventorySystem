namespace IS.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ISupplierRepository Suppliers { get; }
        IUserRepository Users { get; }
        IAuditLogRepository AuditLogs { get; }
        Task<int> SaveChangesAsync();
    }
}