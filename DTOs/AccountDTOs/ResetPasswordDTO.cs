using System.ComponentModel.DataAnnotations;

namespace API.DTOs.AccountDTOs
{
    public class ResetPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;

    }
}
