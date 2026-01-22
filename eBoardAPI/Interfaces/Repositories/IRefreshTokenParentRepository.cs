using eBoardAPI.Context;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories
{
    public interface IRefreshTokenParentRepository
    {
        Task AddAsync(RefreshTokenParent token);
        Task<RefreshTokenParent?> GetAsync(string token);
        Task SaveAsync();
    }
}
