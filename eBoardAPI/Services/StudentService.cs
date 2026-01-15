using AutoMapper;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Student;

namespace eBoardAPI.Services;

public class StudentService(IStudentRepository studentRepository, IMapper mapper) : IStudentService
{
    public async Task<StudentInfoDto> CreateAsync(CreateStudentDto student)
    {
        if(student == null)
            throw new ArgumentNullException(nameof(student));
        try
        {
            var createStudent = mapper.Map<Student>(student);
            var createdStudent = await studentRepository.AddAsync(createStudent);
            return mapper.Map<StudentInfoDto>(createdStudent);

        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating the student.", ex);
        }
    }

    public async Task<StudentInfoDto?> GetByIdAsync(Guid id)
    {
        if(id == Guid.Empty)
            throw new ArgumentException("Invalid student ID.", nameof(id));

        try
        {
            var student = await studentRepository.GetByIdAsync(id);
            return student == null ? null : mapper.Map<StudentInfoDto>(student);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the student.", ex);
        }
    }
}