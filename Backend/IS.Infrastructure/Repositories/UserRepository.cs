using IS.Domain.Entities;
using IS.Domain.Interfaces;
using IS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IS.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(InventoryDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}