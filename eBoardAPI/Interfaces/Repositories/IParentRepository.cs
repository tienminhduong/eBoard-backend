using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface IParentRepository
{
    Task<Result<Parent>> GetByIdAsync(Guid id);
    Task<Result> Update(Parent parent);
}