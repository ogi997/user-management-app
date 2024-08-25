using API.DTOs.RoleDTOs;
using Microsoft.AspNetCore.Identity;
using API.Repositories;

namespace API.Services.Impl
{
    public class RolesService(IRolesRepository roleRepository) : IRolesService
    {
        private readonly IRolesRepository _roleRepository = roleRepository;

        public async Task<string> CreateRoleAsync(CreateRoleDTO createRoleDTO)
        {
            if (string.IsNullOrEmpty(createRoleDTO.RoleName))
            {
                return "Role name is required.";
            }

            var roleExist = await _roleRepository.RoleExistsAsync(createRoleDTO.RoleName);

            if (roleExist)
            {
                return "Role already exists.";
            }

            var roleResult = await _roleRepository.CreateRoleAsync(new IdentityRole(createRoleDTO.RoleName));

            return roleResult.Succeeded ? "Role created successfully" : "Some problem with creating new role";
        }

        public async Task<IEnumerable<RoleResponseDTO>> GetRolesAsync()
        {
            var roles = await _roleRepository.GetRolesAsync();
            return roles.Select(role => new RoleResponseDTO
            {
                Id = role.Id,
                Name = role.Name,
                TotalUsers = _roleRepository.GetUsersInRoleAsync(role.Name!).Result.Count
            }).ToList();
        }

        public async Task<string> DeleteRoleAsync(string id)
        {
            var role = await _roleRepository.FindRoleByIdAsync(id);
            if (role is null)
            {
                return "Role not found.";
            }

            var roleResult = await _roleRepository.DeleteRoleAsync(role);

            return roleResult.Succeeded ? "Role deleted successfully." : "There is a problem with deleting this role.";
        }

        public async Task<string> AssignRoleAsync(RoleAssignDTO roleAssignDTO)
        {
            var user = await _roleRepository.FindUserByIdAsync(roleAssignDTO.UserId);

            if (user is null)
            {
                return "User not found.";
            }

            var role = await _roleRepository.FindRoleByIdAsync(roleAssignDTO.RoleId);

            if (role is null)
            {
                return "Role not found.";
            }

            var result = await _roleRepository.AddUserToRoleAsync(user, role.Name!);

            return result.Succeeded ? "Role assigned successfully." : result.Errors.FirstOrDefault()?.Description!;
        }
    }
}
