using eBoardAPI.Common;
using eBoardAPI.Models.Auth;

namespace eBoardAPI.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result> RegisterTeacherAsync(RegisterTeacherDto registerTeacherDto);
    }
}
