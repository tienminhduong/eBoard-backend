using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Helpers;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Parent;
using eBoardAPI.Models.Student;

namespace eBoardAPI.Services;

public class StudentService(
    IStudentRepository studentRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IStudentService
{
    public async Task<Result<StudentInfoDto>> AddNewStudentAsync(CreateStudentDto createStudentDto)
    {
        try
        {
            var newStudent = mapper.Map<Student>(createStudentDto);
            var result = await unitOfWork.ParentRepository.GetByPhoneNumberAsync(createStudentDto.ParentPhoneNumber);
            Parent? parent = null;

            if (result.IsSuccess)
            {
                newStudent.ParentId = result.Value!.Id;
                parent = result.Value;
                // newStudent.Parent = result.Value;
            }
            else
            {
                var newParent = new Parent
                {
                    FullName = createStudentDto.ParentFullName,
                    PhoneNumber = createStudentDto.ParentPhoneNumber,
                    Email = "",
                    GeneratedPassword = RandomGeneratorHelper.GenerateRandomPassword(),
                    PasswordHash = "Hello",
                    Address = StringHelper.ParseFullAddress(createStudentDto),
                    HealthCondition = createStudentDto.ParentHealthCondition
                };

                await unitOfWork.ParentRepository.AddNewParentAsync(newParent);
                newStudent.ParentId = newParent.Id;
                parent = newParent;
                // newStudent.Parent = newParent;
            }
            
            await unitOfWork.StudentRepository.AddAsync(newStudent);
            
            var addToClassResult = await unitOfWork.ClassRepository.AddNewStudentsToClassAsync(createStudentDto.ClassId, [newStudent.Id]);
            if (!addToClassResult.IsSuccess)
                return Result<StudentInfoDto>.Failure("Lỗi khi thêm học sinh vào lớp: " + addToClassResult.ErrorMessage);

            await unitOfWork.SaveChangesAsync();
            
            var studentInfoDto = mapper.Map<StudentInfoDto>(newStudent);
            studentInfoDto.Parent = mapper.Map<ParentInfoDto>(parent);
            return Result<StudentInfoDto>.Success(studentInfoDto);
        }
        catch (Exception ex)
        {
            return Result<StudentInfoDto>.Failure("Lỗi khi thêm học sinh mới: " + ex.Message);
        }
    }

    public async Task<Result<StudentInfoDto>> GetByIdAsync(Guid id)
    {
            var studentResult = await studentRepository.GetByIdAsync(id);
            return studentResult.IsSuccess 
                ? Result<StudentInfoDto>.Success(mapper.Map<StudentInfoDto>(studentResult.Value)) 
                : Result<StudentInfoDto>.Failure("Không tìm thấy học sinh với ID đã cho.");
    }
}