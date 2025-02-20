using bookify_data.Entities;
using bookify_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleModel>> GetAllAsync();
        Task<RoleModel?> GetByIdAsync(int id);
        Task<RoleModel?> GetByNameAsync(string name);
        Task<Role> CreateAsync(string name, int status);
        Task<Role> UpdateAsync(int id, string name, int status);
        Task<bool> DeleteAsync(int id);
        Task<bool> ChangeStatus(int id, int status);
    }

}
