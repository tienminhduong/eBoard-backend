using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token);
        Task<RefreshToken?> GetAsync(string token);
        Task SaveAsync();
        Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token);
        Task RevokeToken(RefreshToken token);
    }

}
