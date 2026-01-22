using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RefreshToken token)
        {
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(x => x.Teacher)
                .FirstOrDefaultAsync(x => x.Token == token && !x.IsRevoked);
        }

        public async Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token)
        {
            var result = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == token);
            return result;
        }

        public async Task RevokeToken(RefreshToken token)
        {
            _context.RefreshTokens.Remove(token);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
