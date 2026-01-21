using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Auth;
using System.Net.WebSockets;

namespace eBoardAPI.Services
{
    public class AuthService(ITeacherRepository teacherRepository) : IAuthService
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
    }
}
