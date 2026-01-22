using eBoardAPI.Common;
using eBoardAPI.Models.Activity;

namespace eBoardAPI.Interfaces.Services;

public interface IActivityService
{
    Task<Result<ExtracurricularActivityDto>> CreateActivityAsync(CreateActivityDto createActivityDto);
    Task<Result> UpdateActivityAsync(Guid activityId, UpdateActivityDto updateActivityDto);
    Task<Result> DeleteActivityAsync(Guid activityId);
    Task<List<ExtracurricularActivityDto>> GetActivitiesInClassAsync(Guid classId);
    Task<List<ParentViewActivityDto>> GetParentsActivityAsync(Guid classId, Guid studentId);
    Task<Result<ExtracurricularActivityDto>> GetActivityByIdAsync(Guid activityId);
    
    Task<Result> AddParticipantAsync(AddActivityParticipantDto addParticipantDto);
    Task<Result> AddParticipantsAsync(IEnumerable<AddActivityParticipantDto> addParticipantDtos);
    Task<Result> UpdateParticipantAsync(Guid id, UpdateActivityParticipantDto updateParticipantDto);
    Task<Result> RemoveParticipantAsync(Guid id);
    
    Task<Result<Guid>> AddSignInAsync(AddActivitySignInDto addSignInDto);
    Task<Result> RemoveSignInAsync(Guid id);
    Task<Result> AcceptSignInAsync(Guid id);
    Task<Result> RejectSignInAsync(Guid id);
    Task<Result> CheckPaidSignInAsync(Guid id);
    Task<List<ActivitySignInDto>> GetSignInsForActivityAsync(Guid classId, string status);
}