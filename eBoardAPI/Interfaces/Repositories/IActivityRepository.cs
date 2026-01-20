using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface IActivityRepository
{
    Task AddActivityAsync(ExtracurricularActivity activity);
    void UpdateActivityAsync(ExtracurricularActivity activity);
    Task<Result> DeleteActivityAsync(int activityId);
    Task<Result<ExtracurricularActivity>> GetActivityByIdAsync(int activityId);
    Task<List<ExtracurricularActivity>> GetActivitiesInClassAsync(Guid classId);
}