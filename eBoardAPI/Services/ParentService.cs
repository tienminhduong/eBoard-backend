using AutoMapper;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Parent;

namespace eBoardAPI.Services;

public class ParentService(IParentRepository parentRepository, IMapper mapper) : IParentService
{
    public async Task<ParentInfoDto?> GetByIdAsync(Guid id)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty", nameof(id));
        }

        var parent = await parentRepository.GetByIdAsync(id);
        var parentDto = parent == null ? null : mapper.Map<ParentInfoDto?>(parent);
        return parentDto;
    }

    public async Task<ParentInfoDto?> UpdateAsync(Guid id, UpdateParentInfoDto updateParentInfoDto)
    {
        // Validate input parameters
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty", nameof(id));
        }

        if(updateParentInfoDto == null)
        {
            throw new ArgumentNullException(nameof(updateParentInfoDto));
        }

        // get existing parent
        var existingParent = await parentRepository.GetByIdAsync(id);
        if (existingParent == null)
        {
            return null; // or throw an exception if preferred
        }

        // Map updated fields to existing parent entity
        mapper.Map(updateParentInfoDto, existingParent);

        // Update parent in repository
        parentRepository.Update(existingParent);

        return mapper.Map<ParentInfoDto>(existingParent);
    }
}