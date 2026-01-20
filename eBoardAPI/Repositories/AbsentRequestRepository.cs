using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories;

public class AbsentRequestRepository(AppDbContext dbContext) : IAbsentRequestRepository
{
    public async Task<Result<AbsentRequest>> GetAbsentRequestById(Guid id)
    {
        var absentRequest = await dbContext.AbsentRequests.FindAsync(id);
        return absentRequest == null
            ? Result<AbsentRequest>.Failure("Đơn xin nghỉ không tồn tại")
            : Result<AbsentRequest>.Success(absentRequest);
    }

    public async Task<IEnumerable<AbsentRequest>> GetAbsentRequestsByClassIdAsync(Guid classId, string status, int pageNumber, int pageSize)
    {
        var query = from request in dbContext.AbsentRequests
                    where request.ClassId == classId && (string.IsNullOrEmpty(status) || request.Status == status)
                    select request;

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(a => a.Student)
            .Include(a => a.Class)
            .ToListAsync();
    }

    public async Task CreateAbsentRequestAsync(AbsentRequest absentRequest)
    {
        await dbContext.AbsentRequests.AddAsync(absentRequest);
    }

    public void UpdateAbsentRequest(AbsentRequest absentRequest)
    {
        dbContext.AbsentRequests.Update(absentRequest);
    }
}