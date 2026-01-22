using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Helpers;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Class;
using eBoardAPI.Models.Parent;
using eBoardAPI.Models.Student;

namespace eBoardAPI.Services;

public class ParentService(IParentRepository parentRepository, IMapper mapper) : IParentService
{
    public async Task<Result<ParentInfoDto>> GetByIdAsync(Guid id)
    {
        if(id == Guid.Empty)
        {
            return Result<ParentInfoDto>.Failure("Id cannot be empty");
        }

        var resultGet = await parentRepository.GetByIdAsync(id);

        if(resultGet.IsSuccess == false)
        {
            return Result<ParentInfoDto>.Failure(resultGet.ErrorMessage ?? "Error retrieving parent");
        }

        var parentDto = mapper.Map<ParentInfoDto?>(resultGet.Value);
        return parentDto == null
            ? Result<ParentInfoDto>.Failure("Không tìm thấy phụ huynh")
            : Result<ParentInfoDto>.Success(parentDto);
    }

    public async Task<Result<ParentInfoDto>> UpdateAsync(Guid id, UpdateParentInfoDto updateParentInfoDto)
    {
        // get existing parent
        var resultGet = await parentRepository.GetByIdAsync(id);
        var existingParent = resultGet.IsSuccess ? resultGet.Value : null;

        if (existingParent == null)
        {
            return Result<ParentInfoDto>.Failure("Parent not found");
        }

        // Map updated fields to existing parent entity
        mapper.Map(updateParentInfoDto, existingParent);

        // Update parent in repository
        var result = await parentRepository.Update(existingParent);

        if (!result.IsSuccess)
        {
            return Result<ParentInfoDto>.Failure("Failed to update parent");
        }

        var updatedParent = mapper.Map<ParentInfoDto>(existingParent);
        return Result<ParentInfoDto>.Success(updatedParent);
    }

    public async Task<IEnumerable<ParentInfoDto>> CreateAccountForParent(List<Guid> parentIds)
    {
        var parents = await parentRepository.GetParentsByIdsAsync(parentIds);
        var generatePassword = RandomGeneratorHelper.GenerateRandomPassword();
        foreach (var parent in parents)
        {
            parent.GeneratedPassword = generatePassword;
            parent.PasswordHash = BCrypt.Net.BCrypt.HashPassword(generatePassword);
        }    
        await parentRepository.UpdateRangeParentsAsync(parents);
        var dtos = mapper.Map<List<ParentInfoDto>>(parents);
        return dtos;
    }

    public async Task<IEnumerable<ParentInfoDto>> GetParentNotCreateAccountByClassId(Guid classId, int pageNumber = 1, int pageSize = 20)
    {
        var parents = await parentRepository.GetParentNotCreateAccountByClassId(classId, pageNumber, pageSize);
        var dtos = mapper.Map<List<ParentInfoDto>>(parents);
        return dtos;
    }

    public async Task<IEnumerable<ParentInfoDto>> GetParentCreateAccountByClassId(Guid classId, int pageNumber = 1, int pageSize = 20)
    {
        var parents = await parentRepository.GetParentCreateAccountByClassId(classId, pageNumber, pageSize);
        var dtos = mapper.Map<List<ParentInfoDto>>(parents);
        return dtos;
    }

    public async Task<IEnumerable<ChildInClassDto>> GetChildInClassesByClassId(Guid parentId, int pageNumber, int pageSize)
    {
        var studentWithClasses = await parentRepository.GetStudentsWithClassesByParentIdAsync(parentId);
        var dtos = studentWithClasses.Select(x => new ChildInClassDto
        {
            StudentInfo = mapper.Map<StudentInfoDto>(x.Item1),
            ClassInfo = mapper.Map<ClassInfoDto>(x.Item2)
        });
        
        return dtos;
    }
}