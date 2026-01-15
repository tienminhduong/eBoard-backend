using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models;
using eBoardAPI.Models.Teacher;

namespace eBoardAPI.Services;

public class TeacherService(ITeacherRepository teacherRepository, IMapper mapper) : ITeacherService
{
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