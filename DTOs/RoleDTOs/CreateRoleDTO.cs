using System.ComponentModel.DataAnnotations;

namespace API.DTOs.RoleDTOs
{
    public class CreateRoleDTO
    {
        [Required(ErrorMessage = "RoleName is required.")]
        public string RoleName { get; set; } = null!;
    }
}
