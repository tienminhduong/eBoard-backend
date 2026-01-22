using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories
{
    public class RefreshTokenParentRepository(AppDbContext dbContext): IRefreshTokenParentRepository
    {
        public async Task AddAsync(RefreshTokenParent token)
        {
            dbContext.RefreshTokenParents.Add(token);
            await dbContext.SaveChangesAsync();
        }

        public async Task<RefreshTokenParent?> GetAsync(string token)
        {
            return await dbContext.RefreshTokenParents
                .Include(x => x.Parent)
                .FirstOrDefaultAsync(x => x.Token == token && !x.IsRevoked);
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
