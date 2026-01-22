using eBoardAPI.Common;
using eBoardAPI.Models.Auth;

namespace eBoardAPI.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result> RegisterTeacherAsync(RegisterTeacherDto registerTeacherDto);
        Task<Result<LoginResponseDto>> LoginAsync(TeacherLoginDto login);
        Task<Result<LoginResponseDto>> LoginAsync(ParentLoginDto login);
        Task<Result> ForgotPasswordAsync(ForgotPasswordDto dto);
        Task<Result> ResetPasswordAsync(ResetPasswordDto dto);
    }
}
