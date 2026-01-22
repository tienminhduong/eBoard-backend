using eBoardAPI.Common;
using eBoardAPI.Models;
using eBoardAPI.Models.Auth;
using eBoardAPI.Models.Teacher;

namespace eBoardAPI.Interfaces.Services;

public interface ITeacherService
{
    Task<Result> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
    Task<Result<TeacherInfoDto>> GetTeacherInfoAsync(Guid id);
    Task<Result<TeacherInfoDto>> UpdateTeacherInfoAsync(Guid id, UpdateTeacherInfoDto updateTeacherInfoDto);
}