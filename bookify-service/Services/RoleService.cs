using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_data.Model;
using bookify_data.Repository;
using bookify_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<RoleModel>> GetAllAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return roles.Select(r => new RoleModel
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName,
                Status = r.Status,
                CreatedDate = r.CreatedDate,
                LastEdited = r.LastEdited
            }).ToList();
        }

        public async Task<RoleModel?> GetByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null) return null;

            return new RoleModel
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Status = role.Status,
                CreatedDate = role.CreatedDate,
                LastEdited = role.LastEdited
            };
        }

        public async Task<RoleModel?> GetByNameAsync(string name)
        {
            var role = await _roleRepository.GetByNameAsync(name);
            if (role == null) return null;

            return new RoleModel
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Status = role.Status,
                CreatedDate = role.CreatedDate,
                LastEdited = role.LastEdited
            };
        }

        public async Task<Role> CreateAsync(string name, int status)
        {
            // Check if RoleName already exists
            bool exists = await _roleRepository.ExistsByNameAsync(name);
            if (exists)
            {
                throw new Exception("Role name already exists.");
            }

            var role = new Role
            {
                RoleName = name,
                Status = status,
                CreatedDate = DateTime.UtcNow,
                LastEdited = DateTime.UtcNow
            };

            await _roleRepository.AddAsync(role);
            return role;
        }

        public async Task<Role> UpdateAsync(int id, string name, int status)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                throw new Exception("Role not found.");
            }

            // Check if RoleName already exists (excluding the current Role)
            bool exists = await _roleRepository.ExistsByNameAsync(name, id);
            if (exists)
            {
                throw new Exception("Role name already exists.");
            }

            role.RoleName = name;
            role.Status = status;
            role.LastEdited = DateTime.UtcNow;

            await _roleRepository.UpdateAsync(role);
            return role;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null) return false;

            role.Status = 0;
            role.LastEdited = DateTime.UtcNow;

            await _roleRepository.UpdateAsync(role);
            return true;
        }
    }

}
