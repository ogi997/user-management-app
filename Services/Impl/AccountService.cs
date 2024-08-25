using System.Net;
using API.DTOs.AccountDTOs;
using API.Models;
using API.Repositories;
using RestSharp;

namespace API.Services.Impl
{
    public class AccountService(IAccountRepository accountRepository, ITokenService tokenService, IConfiguration configuration) : IAccountService
    {
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IConfiguration _configuration = configuration;

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new AppUser
            {
                Email = registerDTO.Email,
                FullName = registerDTO.FullName,
                UserName = registerDTO.Email
            };

            var result = await _accountRepository.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                return new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            if (registerDTO.Roles is null)
            {
                await _accountRepository.AddToRoleAsync(user, "User");
            }
            else
            {
                foreach (var role in registerDTO.Roles)
                {
                    await _accountRepository.AddToRoleAsync(user, role);
                }
            }

            return new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Account Created Successfully"
            };
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _accountRepository.FindByEmailAsync(loginDTO.Email);

            if (user is null)
            {
                return new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "User not found with this email."
                };
            }

            var result = await _accountRepository.CheckPasswordAsync(user, loginDTO.Password);

            if (!result)
            {
                return new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Invalid Password."
                };
            }

            var token = _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            _ = int.TryParse(_configuration.GetSection("JWTSetting").GetSection("RefreshTokenValidityIn").Value!, out int RefreshTokenValidityIn);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(RefreshTokenValidityIn);
            await _accountRepository.UpdateAsync(user);
            return new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Successfully logged.",
                Token = token,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponseDTO> RefreshTokenAsync(TokenDTO tokenDTO)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenDTO.Token);
            var user = await _accountRepository.FindByEmailAsync(tokenDTO.Email);

            if (principal is null || user is null || user.RefreshToken != tokenDTO.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Invalid client request."
                };
            }

            var newJwtToken = _tokenService.GenerateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            _ = int.TryParse(_configuration.GetSection("JWTSetting").GetSection("RefreshTokenValidityIn").Value!, out int RefreshTokenValidityIn);
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(RefreshTokenValidityIn);
            await _accountRepository.UpdateAsync(user);

            return new AuthResponseDTO
            {
                IsSuccess = true,
                Token = newJwtToken,
                RefreshToken = newRefreshToken,
                Message = "Refresh token successfully."
            };
        }

        public async Task<UserDetailDTO> GetUserDetailAsync(string userId)
        {
            var user = await _accountRepository.FindByIdAsync(userId);

            if (user is null)
            {
                throw new Exception("User not found");
            }

            return new UserDetailDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Roles = (await _accountRepository.GetRolesAsync(user)).ToArray(),
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirm = user.PhoneNumberConfirmed,
                AccessFaildCount = user.AccessFailedCount
            };
        }

        public async Task<IEnumerable<UserDetailDTO>> GetUsersAsync()
        {
            var users = await _accountRepository.GetUsersAsync();
            return users.Select(u => new UserDetailDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Roles = _accountRepository.GetRolesAsync(u).Result.ToArray()
            }).ToList();
        }

        public async Task<AuthResponseDTO> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO)
        {
            var user = await _accountRepository.FindByEmailAsync(forgotPasswordDTO.Email);
            if (user is null)
            {
                return new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "User does not exist with this email."
                };
            }

            var token = await _accountRepository.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"http://localhost:4200/reset-password?email={user.Email}&token={WebUtility.UrlEncode(token)}";

            var client = new RestClient("urlclient");
            var request = new RestRequest
            {
                Method = Method.Post,
                RequestFormat = DataFormat.Json
            };
            request.AddHeader("Authorization", "Bearer xxxTOKEN");
            request.AddJsonBody(new
            {
                from = new { email = "emailAddress", },
                to = new[] { new { email = user.Email } },
                template_uuid = "token uuid",
                template_variables = new { user_email = user.Email, pass_reset_link = resetLink }
            });

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return new AuthResponseDTO
                {
                    IsSuccess = true,
                    Message = "Email sent with password reset link. Please check your email."
                };
            }

            return new AuthResponseDTO
            {
                IsSuccess = false,
                Message = response.Content
            };
        }

        public async Task<AuthResponseDTO> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var user = await _accountRepository.FindByEmailAsync(resetPasswordDTO.Email);
            if (user is null)
            {
                return new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "User does not exist with this email."
                };
            }

            var result = await _accountRepository.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.NewPassword);

            if (result.Succeeded)
            {
                return new AuthResponseDTO
                {
                    IsSuccess = true,
                    Message = "Password reset successfully."
                };
            }

            return new AuthResponseDTO
            {
                IsSuccess = false,
                Message = result.Errors.FirstOrDefault()?.Description
            };
        }

        public async Task<AuthResponseDTO> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            var user = await _accountRepository.FindByEmailAsync(changePasswordDTO.Email);

            if (user is null)
            {
                return new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "User does not exist with this email."
                };
            }

            var result = await _accountRepository.ChangePasswordAsync(user, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);

            if (result.Succeeded)
            {
                return new AuthResponseDTO
                {
                    IsSuccess = true,
                    Message = "Password changed successfully."
                };
            }

            return new AuthResponseDTO
            {
                IsSuccess = false,
                Message = result.Errors.FirstOrDefault()?.Description
            };
        }
    }
}
