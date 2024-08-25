namespace API.DTOs.AccountDTOs
{
    public class UserDetailDTO
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }

        public string? Email { get; set; }
        public string[]? Roles { get; set; }
        public string? PhoneNumber { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool PhoneNumberConfirm { get; set; }
        public int AccessFaildCount { get; set; }


    }
}
