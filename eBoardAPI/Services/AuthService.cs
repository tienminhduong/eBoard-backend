using eBoardAPI.Common;
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
        IConfiguration _config,
        ITokenService tokenService) : IAuthService
    {
        public async Task<Result> RegisterTeacherAsync(RegisterTeacherDto dto)
        {
            // 1. Validate password confirm
            if (dto.Password != dto.ConfirmPassword)
                throw new Exception("Mật khẩu xác nhận không khớp");

            // 2. Check email
            if (await teacherRepository.EmailExistsAsync(dto.Email))
                throw new Exception("Email đã được sử dụng");

            // 3. Check phone
            if (await teacherRepository.PhoneExistsAsync(dto.PhoneNumber))
                throw new Exception("Số điện thoại đã được sử dụng");

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
                throw new Exception("Đăng ký thất bại, vui lòng thử lại");
            return Result.Success();
        }

        public async Task<LoginResponseDto> LoginAsync(TeacherLoginDto dto)
        {
            var user = await teacherRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Email hoặc mật khẩu không đúng");

            var validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!validPassword)
                throw new Exception("Email hoặc mật khẩu không đúng");

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

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue
            };
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await teacherRepository.GetByEmailAsync(dto.Email);

            // KHÔNG tiết lộ email tồn tại hay không
            if (user == null)
                return;

            var token = tokenService.GenerateResetPasswordToken(user);

            var resetLink =
                $"{_config["Frontend:ResetPasswordUrl"]}?token={token}";

            await emailService.SendAsync(
                user.Email,
                "Đặt lại mật khẩu",
                $"Nhấn vào <a href='{resetLink}'>đây</a> để đặt lại mật khẩu. Link có hiệu lực 15 phút."
            );
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
                throw new Exception("Mật khẩu xác nhận không khớp");

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
                        ValidIssuer = _config["Jwt:Issuer"],
                        ValidAudience = _config["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
                        )
                    },
                    out _
                );
            }
            catch
            {
                throw new Exception("Token không hợp lệ hoặc đã hết hạn");
            }

            // Check đúng loại token
            if (principal.FindFirst("type")?.Value != "reset-password")
                throw new Exception("Token không hợp lệ");

            var userId = Guid.Parse(
                principal.FindFirst("id")!.Value
            );

            var teacherResult = await teacherRepository.GetByIdAsync(userId);
            if (!teacherResult.IsSuccess || teacherResult.Value == null)
                throw new Exception("User không tồn tại");
            var teacher = teacherResult.Value;
            teacher.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await teacherRepository.UpdateAsync(teacher);
        }


    }
}
