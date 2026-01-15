using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Parent;

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
        Result<ParentInfoDto> result;
        if (parentDto == null)
        {
            result = Result<ParentInfoDto>.Failure("Parent not found");
        }
        else
        {
            result = Result<ParentInfoDto>.Success(parentDto);
        }
        return result;
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
}