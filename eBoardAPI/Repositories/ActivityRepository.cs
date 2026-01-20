using eBoardAPI.Common;
using eBoardAPI.Consts;
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

    public async Task<Result> DeleteActivityAsync(Guid activityId)
    {
        var activity = await dbContext.ExtracurricularActivities.FindAsync(activityId);
        if (activity == null)
            return Result.Failure("Không tìm thấy hoạt động ngoại khóa.");
        
        dbContext.ExtracurricularActivities.Remove(activity);
        return Result.Success();
    }

    public async Task<Result<ExtracurricularActivity>> GetActivityByIdAsync(Guid activityId)
    {
        var query = from a in dbContext.ExtracurricularActivities
                    where a.Id == activityId
                    select a;
        
        var activity = await query
            .Include(ea => ea.Participants).ThenInclude(ap => ap.Student)
            .FirstOrDefaultAsync();
        
        return activity == null
            ? Result<ExtracurricularActivity>.Failure("Không tìm thấy hoạt động ngoại khóa.")
            : Result<ExtracurricularActivity>.Success(activity);
    }

    public async Task<List<ExtracurricularActivity>> GetActivitiesInClassAsync(Guid classId)
    {
        var query = from activity in dbContext.ExtracurricularActivities
                    where activity.ClassId == classId
                    select activity;
        
        return await query
            .Include(ea => ea.Participants).ThenInclude(ap => ap.Student)
            .ToListAsync();
    }

    public async Task<Result<ActivityParticipant>> GetActivityParticipantByIdAsync(Guid id)
    {
        var participant = await dbContext.ActivityParticipants.FindAsync(id);
        return participant == null
            ? Result<ActivityParticipant>.Failure("Không tìm thấy người tham gia hoạt động.")
            : Result<ActivityParticipant>.Success(participant);
    }

    public async Task AddActivityParticipantAsync(ActivityParticipant participant)
    {
        await dbContext.ActivityParticipants.AddAsync(participant);
    }

    public void UpdateActivityParticipantAsync(ActivityParticipant participant)
    {
        dbContext.ActivityParticipants.Update(participant);
    }

    public async Task<Result> RemoveActivityParticipantAsync(Guid id)
    {
        var participant = await dbContext.ActivityParticipants.FindAsync(id);
        if (participant == null)
            return Result.Failure("Không tìm thấy người tham gia hoạt động.");
        
        dbContext.ActivityParticipants.Remove(participant);
        return Result.Success();
    }

    public async Task AddActivitySignInAsync(ActivitySignIn signIn)
    {
        await dbContext.ActivitySignIns.AddAsync(signIn);
    }

    public void UpdateActivitySignInAsync(ActivitySignIn signIn)
    {
        dbContext.ActivitySignIns.Update(signIn);
    }

    public async Task<Result> RemoveActivitySignInAsync(Guid id)
    {
        var signIn = await dbContext.ActivitySignIns.FindAsync(id);
        if (signIn == null)
            return Result.Failure("Không tìm thấy thông tin đăng kí.");
        
        dbContext.ActivitySignIns.Remove(signIn);
        return Result.Success();
    }

    public async Task<Result<ActivitySignIn>> GetActivitySignInByIdAsync(Guid id)
    {
        var query = from s in dbContext.ActivitySignIns
                    where s.Id == id
                    select s;
        
        var signIn =  await query
            .Include(asi => asi.Student)
            .Include(asi => asi.Activity)
            .FirstOrDefaultAsync();
        
        return signIn == null
            ? Result<ActivitySignIn>.Failure("Không tìm thấy thông tin đăng kí.")
            : Result<ActivitySignIn>.Success(signIn);
    }

    public async Task<List<ActivitySignIn>> GetSignInsForActivityAsync(Guid classId, string status)
    {
        var query = from signIn in dbContext.ActivitySignIns
            join activity in dbContext.ExtracurricularActivities on signIn.ActivityId equals activity.Id
            where activity.ClassId == classId && (signIn.Status == status || string.IsNullOrEmpty(status))
            select signIn;
        
        return await query
            .Include(asi => asi.Student)
            .Include(asi => asi.Activity)
            .ToListAsync();
    }

    public async Task<string> GetActivitySignInStatusAsync(Guid studentId, Guid activityId)
    {
        var statusQuery = from signIn in dbContext.ActivitySignIns
                    where signIn.StudentId == studentId && signIn.ActivityId == activityId
                    select signIn.Status;
        
        var pariticipatedQuery = from participant in dbContext.ActivityParticipants
                                 where participant.StudentId == studentId && participant.ActivityId == activityId
                                 select participant;
        
        var status = await statusQuery.FirstOrDefaultAsync();
        if (!string.IsNullOrEmpty(status))
            return status;
        
        var participated = await pariticipatedQuery.AnyAsync();
        return participated ? EActivitySignInStatus.PARTICIPATED : EActivitySignInStatus.NOT_SIGNED_IN;
    }
}