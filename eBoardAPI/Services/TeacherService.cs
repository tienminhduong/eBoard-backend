using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models;
using eBoardAPI.Models.Auth;
using eBoardAPI.Models.Teacher;

namespace eBoardAPI.Services;

public class TeacherService(ITeacherRepository teacherRepository, IMapper mapper) : ITeacherService
{
    public async Task<Result> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        if(changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
        {
            return Result.Failure("New password and confirm password do not match.");
        }
        var teacherResult = await teacherRepository.GetByIdAsync(changePasswordDto.Id);
        if(!teacherResult.IsSuccess)
        {
            return Result.Failure(teacherResult.ErrorMessage!);
        }
        var teacher = teacherResult.Value!;
        if(!BCrypt.Net.BCrypt.Verify(changePasswordDto.OldPassword, teacher.PasswordHash))
        {
            return Result.Failure("Old password is incorrect.");
        }
        teacher.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
        var updateResult = await teacherRepository.UpdateAsync(teacher);
        return updateResult.IsSuccess 
            ? Result.Success() 
            : Result.Failure(updateResult.ErrorMessage!);

    }

    public async Task<Result<TeacherInfoDto>> GetTeacherByClassIdAsync(Guid classId)
    {
        var teacherResult = await teacherRepository.GetTeacherByClassIdAsync(classId);
        return (teacherResult != null) ? Result<TeacherInfoDto>.Success(mapper.Map<TeacherInfoDto>(teacherResult))
            : Result<TeacherInfoDto>.Failure("Teacher not found for the given class ID.");
    }

    public async Task<Result<TeacherInfoDto>> GetTeacherInfoAsync(Guid id)
    {       
        var teacherResult = await teacherRepository.GetByIdAsync(id);
        return teacherResult.IsSuccess 
            ? Result<TeacherInfoDto>.Success(mapper.Map<TeacherInfoDto>(teacherResult.Value)) 
            : Result<TeacherInfoDto>.Failure(teacherResult.ErrorMessage!);
    }

    public async Task<Result<TeacherInfoDto>> UpdateTeacherInfoAsync(Guid id, UpdateTeacherInfoDto updateTeacherInfoDto)
    {
        var exsitingTeacherResult = await teacherRepository.GetByIdAsync(id);
        if(!exsitingTeacherResult.IsSuccess)
        {
            return Result<TeacherInfoDto>.Failure(exsitingTeacherResult.ErrorMessage!);
        }

        var exsitingTeacher = exsitingTeacherResult.Value!;
        mapper.Map(updateTeacherInfoDto, exsitingTeacher);
        var updateResult = await teacherRepository.UpdateAsync(exsitingTeacher);

        return updateResult.IsSuccess
            ? Result<TeacherInfoDto>.Success(mapper.Map<TeacherInfoDto>(updateResult.Value))
            : Result<TeacherInfoDto>.Failure(updateResult.ErrorMessage!);
    }
}