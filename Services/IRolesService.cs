using API.DTOs.RoleDTOs;

namespace API.Services
{
    public interface IRolesService
    {
        Task<string> CreateRoleAsync(CreateRoleDTO createRoleDTO);
        Task<IEnumerable<RoleResponseDTO>> GetRolesAsync();
        Task<string> DeleteRoleAsync(string id);
        Task<string> AssignRoleAsync(RoleAssignDTO roleAssignDTO);
    }
}
