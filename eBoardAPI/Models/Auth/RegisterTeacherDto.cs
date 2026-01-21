namespace eBoardAPI.Models.Auth
{
    public class RegisterTeacherDto
    {
        public required string FullName { get; set; }

        public required string PhoneNumber { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string ConfirmPassword { get; set; }
    }

}
