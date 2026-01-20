using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface IActivityRepository
{
    Task AddActivityAsync(ExtracurricularActivity activity);
    void UpdateActivityAsync(ExtracurricularActivity activity);
    Task<Result> DeleteActivityAsync(Guid activityId);
    Task<Result<ExtracurricularActivity>> GetActivityByIdAsync(Guid activityId);
    Task<List<ExtracurricularActivity>> GetActivitiesInClassAsync(Guid classId);
    
    Task<Result<ActivityParticipant>> GetActivityParticipantByIdAsync(Guid id);
    Task AddActivityParticipantAsync(ActivityParticipant participant);
    void UpdateActivityParticipantAsync(ActivityParticipant participant);
    Task<Result> RemoveActivityParticipantAsync(Guid id);
    
    Task AddActivitySignInAsync(ActivitySignIn signIn);
    void UpdateActivitySignInAsync(ActivitySignIn signIn);
    Task<Result> RemoveActivitySignInAsync(Guid id);
    Task<Result<ActivitySignIn>> GetActivitySignInByIdAsync(Guid id);
    Task<List<ActivitySignIn>> GetSignInsForActivityAsync(Guid classId, string status);
    Task<string> GetActivitySignInStatusAsync(Guid studentId, Guid activityId);
}