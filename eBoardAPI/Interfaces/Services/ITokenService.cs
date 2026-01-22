using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(Teacher teacher);
        string GenerateAccessToken(Parent parent);
        string GenerateRefreshToken();
        string GenerateResetPasswordToken(Teacher teacher);
    }

}
