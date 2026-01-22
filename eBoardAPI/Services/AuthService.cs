using eBoardAPI.Common;
using eBoardAPI.Consts;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Auth;
using eBoardAPI.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace eBoardAPI.Services
{
    public class AuthService(ITeacherRepository teacherRepository,
        IEmailService emailService,
        IRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenParentRepository refreshTokenParentRepository,
        IParentRepository parentRepository,
        ITokenService tokenService) : IAuthService
    {
        const string LOGIN_FAILURE_MESSAGE = "Đăng nhập không thành công";
        const string REGISTER_FAILURE_MESSAGE = "Đăng kí không thành công";
        public async Task<Result> RegisterTeacherAsync(RegisterTeacherDto dto)
        {
            // 1. Validate password confirm
            if (dto.Password != dto.ConfirmPassword)
                return Result.Failure(REGISTER_FAILURE_MESSAGE);

            // 2. Check email
            if (await teacherRepository.EmailExistsAsync(dto.Email))
                return Result.Failure(REGISTER_FAILURE_MESSAGE);

            // 3. Check phone
            if (await teacherRepository.PhoneExistsAsync(dto.PhoneNumber))
                return Result.Failure(REGISTER_FAILURE_MESSAGE);

            // 4. Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // 5. Create teacher
            var teacher = new Teacher
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = passwordHash
            };

            var result = await teacherRepository.AddAsync(teacher);
            if (!result.IsSuccess)
                return Result.Failure(REGISTER_FAILURE_MESSAGE);
            return Result.Success();
        }

        public async Task<Result<LoginResponseDto>> LoginAsync(TeacherLoginDto dto)
        {
            var user = await teacherRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                return Result<LoginResponseDto>.Failure(LOGIN_FAILURE_MESSAGE);

            var validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!validPassword)
                return Result<LoginResponseDto>.Failure(LOGIN_FAILURE_MESSAGE);

            var accessToken = tokenService.GenerateAccessToken(user);
            var refreshTokenValue = tokenService.GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                TeacherId = user.Id,
                Token = refreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await refreshTokenRepository.AddAsync(refreshToken);

            return Result<LoginResponseDto>.Success(new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue
            });
        }

        public async Task<Result> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await teacherRepository.GetByEmailAsync(dto.Email);

            // KHÔNG tiết lộ email tồn tại hay không
            if (user == null)
                return Result.Failure("");

            var token = tokenService.GenerateResetPasswordToken(user);

            var resetLink =
                $"{Environment.GetEnvironmentVariable(EnvKey.FRONTEND_RESET_PASSWORD_URL)!}?token={token}";

            await emailService.SendAsync(
                user.Email,
                "Đặt lại mật khẩu",
                $"Nhấn vào <a href='{resetLink}'>đây</a> để đặt lại mật khẩu. Link có hiệu lực 15 phút."
            );
            return Result.Success();
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
                return Result.Failure("Mật khẩu xác nhận không khớp");

            var handler = new JwtSecurityTokenHandler();

            ClaimsPrincipal principal;
            try
            {
                principal = handler.ValidateToken(
                    dto.Token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Environment.GetEnvironmentVariable(EnvKey.JWT_ISSUER)!,
                        ValidAudience = Environment.GetEnvironmentVariable(EnvKey.JWT_AUDIENCE)!,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(EnvKey.JWT_KEY)!)
                        )
                    },
                    out _
                );
            }
            catch
            {
                return Result.Failure("Token không hợp lệ hoặc đã hết hạn");
            }

            // Check đúng loại token
            if (principal.FindFirst("type")?.Value != "reset-password")
                return Result.Failure("Token không hợp lệ");

            var userId = Guid.Parse(
                principal.FindFirst("id")!.Value
            );

            var teacherResult = await teacherRepository.GetByIdAsync(userId);
            if (!teacherResult.IsSuccess || teacherResult.Value == null)
                return Result.Failure("User không tồn tại");
            var teacher = teacherResult.Value;
            teacher.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await teacherRepository.UpdateAsync(teacher);
            return Result.Success();
        }

        public async Task<Result<LoginResponseDto>> LoginAsync(ParentLoginDto login)
        {
            var parentResult = await parentRepository.GetByPhoneNumberAsync(login.PhoneNumber);
            if (parentResult == null || !parentResult.IsSuccess)
                return Result<LoginResponseDto>.Failure(LOGIN_FAILURE_MESSAGE);
            var parent = parentResult.Value!;
            var validPassword = BCrypt.Net.BCrypt.Verify(login.Password, parent.PasswordHash);
            if (!validPassword)
                return Result<LoginResponseDto>.Failure(LOGIN_FAILURE_MESSAGE);

            var accessToken = tokenService.GenerateAccessToken(parent);
            var refreshTokenValue = tokenService.GenerateRefreshToken();

            var refreshToken = new RefreshTokenParent
            {
                Id = Guid.NewGuid(),
                ParentId = parent.Id,
                Token = refreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await refreshTokenParentRepository.AddAsync(refreshToken);
            return Result<LoginResponseDto>.Success(new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue
            });
        }

        public async Task<Result<string>> GetNewTokenByRefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var tokenEntity = await refreshTokenRepository.GetRefreshTokenByTokenAsync(refreshTokenRequestDto.RefreshToken);
            if (tokenEntity == null)
                return Result<string>.Failure("Không tồn tại refresh token");

            if (tokenEntity.ExpiresAt < DateTime.UtcNow)
                return Result<string>.Failure("Refresh token đã hết hạn");

            var teacherResult = await teacherRepository.GetByIdAsync(tokenEntity.TeacherId);
            if (!teacherResult.IsSuccess || teacherResult.Value == null)
                return Result<string>.Failure("User không tồn tại");

            var teacher = teacherResult.Value;
            var newAccessToken = tokenService.GenerateAccessToken(teacher);
            return Result<string>.Success(newAccessToken);
        }
    }
}
