using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;

namespace eBoardAPI.Repositories
{
    public class ViolationRepository(AppDbContext dbContext) : IViolationRepository
    {
        public async Task<Result<IEnumerable<Violation>>> AddRangeAsync(IEnumerable<Violation> violations)
        {
            try
            {
                await dbContext.Violations.AddRangeAsync(violations);
                await dbContext.SaveChangesAsync();
                return Result<IEnumerable<Violation>>.Success(violations);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
