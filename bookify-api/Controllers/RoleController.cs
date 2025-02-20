using bookify_data.Model;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RoleModel>>> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleModel>> GetById(int id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpGet("role-name/{name}")]
        public async Task<ActionResult<RoleModel>> GetByName(string name)
        {
            var role = await _roleService.GetByNameAsync(name);
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleCreateRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RoleName))
            {
                return BadRequest("Role name is required.");
            }

            try
            {
                var role = await _roleService.CreateAsync(request.RoleName, request.Status);
                return CreatedAtAction(nameof(GetById), new { id = role.RoleId }, role);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoleUpdateRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RoleName))
            {
                return BadRequest("Role name is required.");
            }

            try
            {
                var updatedRole = await _roleService.UpdateAsync(id, request.RoleName, request.Status);
                if (updatedRole == null)
                {
                    return NotFound("Role not found.");
                }

                return Ok(updatedRole);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool deleted = await _roleService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return Ok();
        }

        [HttpPatch("{id}/change-status")]
        public async Task<ActionResult> ChangeStatus(int id, [FromBody] RoleUpdateRequest request)
        {
            if (request == null || request.Status == 0)
            {
                return BadRequest("Invalid status.");
            }
            bool changed = await _roleService.ChangeStatus(id, request.Status);
            if (!changed) return NotFound();
            return Ok();
        }

        public class RoleCreateRequest
        {
            public string RoleName { get; set; }
            public int Status { get; set; }
        }

        public class RoleUpdateRequest
        {
            public string RoleName { get; set; }
            public int Status { get; set; }
        }

    }

}
