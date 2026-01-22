using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface IParentRepository
{
    Task<Result<Parent>> GetByIdAsync(Guid id);
    Task<Result> Update(Parent parent);
    Task<Result<Parent>> GetByPhoneNumberAsync(string phoneNumber);
    Task<Parent> AddNewParentAsync(Parent parent);
    Task<IEnumerable<Parent>> GetParentsByIdsAsync(List<Guid> parentIds);
    Task UpdateRangeParentsAsync(IEnumerable<Parent> parents);
    Task<IEnumerable<Parent>> GetParentNotCreateAccountByClassId(Guid classId, int pageNumber, int pageSize);
    Task<IEnumerable<Parent>> GetParentCreateAccountByClassId(Guid classId, int pageNumber, int pageSize);
}