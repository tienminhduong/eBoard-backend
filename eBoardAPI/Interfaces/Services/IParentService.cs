using eBoardAPI.Common;
using eBoardAPI.Models.Parent;

namespace eBoardAPI.Interfaces.Services;

public interface IParentService
{
    Task<Result<ParentInfoDto>> GetByIdAsync(Guid id);
    Task<Result<ParentInfoDto>> UpdateAsync(Guid id, UpdateParentInfoDto updateParentInfoDto);
    Task<IEnumerable<ParentInfoDto>> CreateAccountForParent(List<Guid> parentIds);
    Task<IEnumerable<ParentInfoDto>> GetParentNotCreateAccountByClassId(Guid classId, int pageNumber, int pageSize);
}