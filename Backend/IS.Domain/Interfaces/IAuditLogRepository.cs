using IS.Domain.Entities;

namespace IS.Domain.Interfaces
{
    public interface IAuditLogRepository
    {
        Task AddAsync(AuditLog log);
        Task AddRangeAsync(IEnumerable<AuditLog> logs);
        Task<IEnumerable<AuditLog>> GetByRecordAsync(string tableName, int recordId);
    }
}