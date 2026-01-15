using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Student;

namespace eBoardAPI.Services;

public class StudentService(IStudentRepository studentRepository, IMapper mapper) : IStudentService
{
    public async Task<Result<StudentInfoDto>> CreateAsync(CreateStudentDto student)
    {
        var createStudent = mapper.Map<Student>(student);
        var addResult = await studentRepository.AddAsync(createStudent);

        return addResult.IsSuccess
            ? Result<StudentInfoDto>.Success(mapper.Map<StudentInfoDto>(createStudent))
            : Result<StudentInfoDto>.Failure("Failed to create student.");
    }

    public async Task<Result<StudentInfoDto>> GetByIdAsync(Guid id)
    {
            var studentResult = await studentRepository.GetByIdAsync(id);
            return studentResult.IsSuccess 
                ? Result<StudentInfoDto>.Success(mapper.Map<StudentInfoDto>(studentResult.Value)) 
                : Result<StudentInfoDto>.Failure("Student not found.");
    }
}