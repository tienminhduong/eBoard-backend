using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models;
using eBoardAPI.Models.Class;
using eBoardAPI.Models.Student;

namespace eBoardAPI.Services;

public class ClassService(
    IUnitOfWork unitOfWork,
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

    public async Task<PagedDto<ClassInfoDto>> GetPagedClassesByTeacherAsync(Guid teacherId, int pageNumber, int pageSize)
    {
        var classes = await classRepository.GetAllClassesByTeacherAsync(teacherId, pageNumber, pageSize);
        var classDtos = mapper.Map<IEnumerable<ClassInfoDto>>(classes);
        var pagedResult = new PagedDto<ClassInfoDto>
        {
            Data = classDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = classes.Count(),
        };
        return pagedResult;
    }

    public async Task<Result<ClassInfoDto>> AddNewClassAsync(CreateClassDto createClassDto, Guid teacherId)
    {
        try
        {
            var newClass = mapper.Map<Class>(createClassDto);
            newClass.TeacherId = teacherId;
            await unitOfWork.ClassRepository.AddNewClassAsync(newClass);

            var newClassFund = new ClassFund { ClassId = newClass.Id };
            await unitOfWork.ClassFundRepository.AddNewClassFundAsync(newClassFund);

            var saveCount = await unitOfWork.SaveChangesAsync();
            if (saveCount == 0)
                return Result<ClassInfoDto>.Failure("Tạo lớp học thất bại");
            
            var classDto = mapper.Map<ClassInfoDto>(newClass);
            return Result<ClassInfoDto>.Success(classDto);
        }
        catch (Exception ex)
        {
            return Result<ClassInfoDto>.Failure($"Đã xảy ra lỗi khi tạo lớp học: {ex.Message}");
        }
    }
}