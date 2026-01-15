using AutoMapper;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models;
using eBoardAPI.Models.Teacher;

namespace eBoardAPI.Services;

public class TeacherService(ITeacherRepository teacherRepository, IMapper mapper) : ITeacherService
{
    public async Task<TeacherInfoDto?> GetTeacherInfoAsync(Guid id)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentException("Invalid teacher ID.", nameof(id));
        }
        
        try
        {
            var teacher = await teacherRepository.GetByIdAsync(id);
            var teacherInfoDto = (teacher != null) ? mapper.Map<TeacherInfoDto>(teacher) : null;
            return teacherInfoDto;
        }
        catch (Exception ex)
        {
            // Log the exception (not implemented here)
            throw new ApplicationException("An error occurred while retrieving teacher information.", ex);
        }

    }

    public async Task<TeacherInfoDto?> UpdateTeacherInfoAsync(Guid id, UpdateTeacherInfoDto updateTeacherInfoDto)
    {
        // Validate inputs
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid teacher ID.", nameof(id));
        }
        if(updateTeacherInfoDto == null)
        {
            throw new ArgumentNullException(nameof(updateTeacherInfoDto), "Update data cannot be null.");
        }

        try
        {
            var exsitingTeacher = await teacherRepository.GetByIdAsync(id);
            if(exsitingTeacher == null)
            {
                return null;
            }
            
            mapper.Map(updateTeacherInfoDto, exsitingTeacher);
            var rowUpdate = await teacherRepository.Update(exsitingTeacher);

            return mapper.Map<TeacherInfoDto>(exsitingTeacher);
        }
        catch (Exception ex)
        {
            // Log the exception (not implemented here)
            throw new ApplicationException("An error occurred while updating teacher information.", ex);
        }
    }
}