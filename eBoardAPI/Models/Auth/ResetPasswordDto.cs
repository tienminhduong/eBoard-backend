namespace eBoardAPI.Models.Auth
{
    public class ResetPasswordDto
    {
        required public string Token { get; set; }
        required public string NewPassword { get; set; }
        required public string ConfirmPassword { get; set; }
    }
}
