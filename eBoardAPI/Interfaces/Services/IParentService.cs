using eBoardAPI.Models.Parent;

namespace eBoardAPI.Interfaces.Services;

public interface IParentService
{
    Task<ParentInfoDto?> GetByIdAsync(Guid id);
    Task<ParentInfoDto?> UpdateAsync(Guid id, UpdateParentInfoDto updateParentInfoDto);
}