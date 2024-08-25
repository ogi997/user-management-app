using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Repositories
{
    public interface IRolesRepository
    {
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> CreateRoleAsync(IdentityRole role);
        Task<IEnumerable<IdentityRole>> GetRolesAsync();
        Task<IdentityRole?> FindRoleByIdAsync(string roleId);
        Task<IdentityResult> DeleteRoleAsync(IdentityRole role);
        Task<IList<AppUser>> GetUsersInRoleAsync(string roleName);
        Task<AppUser?> FindUserByIdAsync(string userId);
        Task<IdentityResult> AddUserToRoleAsync(AppUser user, string roleName);
    }
}
