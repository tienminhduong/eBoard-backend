using eBoardAPI.Models;
using eBoardAPI.Models.Teacher;

namespace eBoardAPI.Interfaces.Services;

public interface ITeacherService
{
    Task<TeacherInfoDto?> GetTeacherInfoAsync(Guid id);
    Task<TeacherInfoDto?> UpdateTeacherInfoAsync(Guid id, UpdateTeacherInfoDto updateTeacherInfoDto);
}