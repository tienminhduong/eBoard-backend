using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Class;
using eBoardAPI.Models.Student;

namespace eBoardAPI.Services;

public class ClassService(
    IClassRepository classRepository,
    IMapper mapper
    ) : IClassService
{
    public async Task<IEnumerable<ClassInfoDto>> GetAllTeachingClassesByTeacherAsync(Guid teacherId)
    {
        var classes = await classRepository.GetAllTeachingClassByTeacherAsync(teacherId);
        var classDtos = mapper.Map<IEnumerable<ClassInfoDto>>(classes);
        return classDtos;
    }

    public async Task<IEnumerable<Grade>> GetAllGradesAsync()
    {
        var grades = await classRepository.GetAllGradesAsync();
        return grades;
    }

    public async Task<Result<ClassInfoDto>> GetClassByIdAsync(Guid classId)
    {
        var result = await classRepository.GetClassByIdAsync(classId);
        
        if (!result.IsSuccess)
            return Result<ClassInfoDto>.Failure(result.ErrorMessage!);
        
        var classDto = mapper.Map<ClassInfoDto>(result.Value);
        return Result<ClassInfoDto>.Success(classDto);
    }

    public async Task<PagedStudentInClassDto> GetPagedStudentsByClassAsync(Guid classId, int pageNumber, int pageSize)
    {
        var students = await classRepository.GetStudentsByClassAsync(classId, pageNumber, pageSize);
        var studentDtos = mapper.Map<IEnumerable<StudentInfoDto>>(students);
        
        var pagedResult = new PagedStudentInClassDto
        {
            Data = studentDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = students.Count(),
            ClassId = classId
        };
        return pagedResult;
    }
}