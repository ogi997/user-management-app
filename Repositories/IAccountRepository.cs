using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Repositories
{
    public interface IAccountRepository
    {
        Task<AppUser?> FindByEmailAsync(string email);
        Task<AppUser?> FindByIdAsync(string id);
        Task<IdentityResult> CreateAsync(AppUser user, string password);
        Task<IdentityResult> UpdateAsync(AppUser user);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
        Task<string> GeneratePasswordResetTokenAsync(AppUser user);
        Task<IdentityResult> ResetPasswordAsync(AppUser user, string token, string newPassword);
        Task<IdentityResult> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword);
        Task<IList<string>> GetRolesAsync(AppUser user);
        Task AddToRoleAsync(AppUser user, string role);
        Task<IList<AppUser>> GetUsersAsync();
    }
}
