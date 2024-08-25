using System.ComponentModel.DataAnnotations;

namespace API.DTOs.AccountDTOs
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
