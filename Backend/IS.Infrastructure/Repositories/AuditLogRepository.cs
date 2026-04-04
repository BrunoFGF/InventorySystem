using IS.Domain.Entities;
using IS.Domain.Interfaces;
using IS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IS.Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly InventoryDbContext _context;

        public AuditLogRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AuditLog log) => await _context.AuditLogs.AddAsync(log);

        public async Task AddRangeAsync(IEnumerable<AuditLog> logs) => await _context.AuditLogs.AddRangeAsync(logs);

        public async Task<IEnumerable<AuditLog>> GetByRecordAsync(string tableName, int recordId)
        {
            return await _context.AuditLogs
                .Where(a => a.TableName == tableName && a.RecordId == recordId)
                .OrderByDescending(a => a.ChangedAt)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}