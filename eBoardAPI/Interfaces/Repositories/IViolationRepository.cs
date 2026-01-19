using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories
{
    public interface IViolationRepository
    {
        Task<Result<IEnumerable<Violation>>> AddRangeAsync(IEnumerable<Violation> violations);
        Task<Result<IEnumerable<Violation>>> GetRangeByIdsAsync(IEnumerable<Guid> ids);
    }
}
