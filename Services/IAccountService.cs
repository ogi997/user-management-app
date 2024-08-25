using API.DTOs.AccountDTOs;

namespace API.Services
{
    public interface IAccountService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDTO);
        Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO);
        Task<AuthResponseDTO> RefreshTokenAsync(TokenDTO tokenDTO);
        Task<UserDetailDTO> GetUserDetailAsync(string userId);
        Task<IEnumerable<UserDetailDTO>> GetUsersAsync();
        Task<AuthResponseDTO> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO);
        Task<AuthResponseDTO> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        Task<AuthResponseDTO> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO);
    }
}
