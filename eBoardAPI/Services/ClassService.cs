using AutoMapper;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Class;

namespace eBoardAPI.Services;

public class ClassService(
    IClassRepository classRepository,
    IMapper mapper
    ) : IClassService
{
    public async Task<IEnumerable<ClassInfoDto>> GetAllTeachingClassesByTeacher(Guid teacherId)
    {
        var classes = await classRepository.GetAllTeachingClassByTeacherAsync(teacherId);
        var classDtos = mapper.Map<IEnumerable<ClassInfoDto>>(classes);
        return classDtos;
    }
}