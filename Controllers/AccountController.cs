using System.Security.Claims;
using API.DTOs.AccountDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Services;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<string>> Register(RegisterDTO registerDTO)
        {
            var response = await _accountService.RegisterAsync(registerDTO);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO loginDTO)
        {
            var response = await _accountService.LoginAsync(loginDTO);
            if (!response.IsSuccess)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Refresh-Token")]
        public async Task<ActionResult<AuthResponseDTO>> RefreshToken(TokenDTO tokenDTO)
        {
            var response = await _accountService.RefreshTokenAsync(tokenDTO);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet("Detail")]
        public async Task<ActionResult<UserDetailDTO>> GetUserDetail()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _accountService.GetUserDetailAsync(currentUserId!);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetailDTO>>> GetUsers()
        {
            var users = await _accountService.GetUsersAsync();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("Forgot-Password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            var response = await _accountService.ForgotPasswordAsync(forgotPasswordDTO);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var response = await _accountService.ResetPasswordAsync(resetPasswordDTO);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("Change-Password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            var response = await _accountService.ChangePasswordAsync(changePasswordDTO);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
