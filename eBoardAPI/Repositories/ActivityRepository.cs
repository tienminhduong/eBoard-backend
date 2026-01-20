using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories;

public class ActivityRepository(AppDbContext dbContext) : IActivityRepository
{
    public async Task AddActivityAsync(ExtracurricularActivity activity)
    {
        await dbContext.ExtracurricularActivities.AddAsync(activity);
    }

    public void UpdateActivityAsync(ExtracurricularActivity activity)
    {
        dbContext.ExtracurricularActivities.Update(activity);
    }

    public async Task<Result> DeleteActivityAsync(int activityId)
    {
        var activity = await dbContext.ExtracurricularActivities.FindAsync(activityId);
        if (activity == null)
            return Result.Failure("Không tìm thấy hoạt động ngoại khóa.");
        
        dbContext.ExtracurricularActivities.Remove(activity);
        return Result.Success();
    }

    public async Task<Result<ExtracurricularActivity>> GetActivityByIdAsync(int activityId)
    {
        var activity = await dbContext.ExtracurricularActivities.FindAsync(activityId);
        return activity == null
            ? Result<ExtracurricularActivity>.Failure("Không tìm thấy hoạt động ngoại khóa.")
            : Result<ExtracurricularActivity>.Success(activity);
    }

    public async Task<List<ExtracurricularActivity>> GetActivitiesInClassAsync(Guid classId)
    {
        var query = from activity in dbContext.ExtracurricularActivities
                    where activity.ClassId == classId
                    select activity;
        
        return await query.ToListAsync();
    }
}