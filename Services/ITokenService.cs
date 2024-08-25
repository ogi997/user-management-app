using System.Security.Claims;
using API.Models;

namespace API.Services
{
    public interface ITokenService
    {
        string GenerateToken(AppUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
