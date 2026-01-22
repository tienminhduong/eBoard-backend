namespace eBoardAPI.Models.Auth
{
    public class ChangePasswordDto
    {
        public required Guid Id { get; set; }
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
