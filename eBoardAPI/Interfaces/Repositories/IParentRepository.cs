using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface IParentRepository
{
    Task<Parent?> GetByIdAsync(Guid id);
    void Update(Parent parent);
}