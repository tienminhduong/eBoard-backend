using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface IAbsentRequestRepository
{
    Task<Result<AbsentRequest>> GetAbsentRequestById(Guid id);
    Task<IEnumerable<AbsentRequest>> GetAbsentRequestsByClassIdAsync(Guid classId, string status, int pageNumber, int pageSize);
    Task CreateAbsentRequestAsync(AbsentRequest absentRequest);
    void UpdateAbsentRequest(AbsentRequest absentRequest);
}