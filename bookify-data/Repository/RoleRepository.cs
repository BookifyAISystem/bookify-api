using bookify_data.Data;
using bookify_data.Entities;
using bookify_data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly BookifyDbContext _dbContext;

        public RoleRepository(BookifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _dbContext.Roles
                .Where(r => r.Status != 0)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _dbContext.Roles
                .Where(r => r.Status != 0)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoleId == id);
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _dbContext.Roles
                .Where(r => r.Status != 0)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoleName == name);
        }

        public async Task AddAsync(Role role)
        {
            await _dbContext.Roles.AddAsync(role);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Role role)
        {
            _dbContext.Roles.Update(role);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        {
            return await _dbContext.Roles
                .AnyAsync(r => r.RoleName == name && (!excludeId.HasValue || r.RoleId != excludeId.Value) && r.Status != 0);
        }
    }
}
