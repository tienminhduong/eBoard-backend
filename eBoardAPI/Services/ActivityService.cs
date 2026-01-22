using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Consts;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Activity;

namespace eBoardAPI.Services;

[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
public class ActivityService(
    IActivityRepository activityRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IActivityService
{
    public async Task<Result<ExtracurricularActivityDto>> CreateActivityAsync(CreateActivityDto createActivityDto)
    {
        var activity = mapper.Map<ExtracurricularActivity>(createActivityDto);
        await unitOfWork.ActivityRepository.AddActivityAsync(activity);

        try
        {
            await unitOfWork.SaveChangesAsync();
            return Result<ExtracurricularActivityDto>.Success(mapper.Map<ExtracurricularActivityDto>(activity));
        }
        catch (Exception ex)
        {
            return Result<ExtracurricularActivityDto>.Failure("Đã xảy ra lỗi khi tạo hoạt động ngoại khóa.");
        }
    }

    public async Task<Result> UpdateActivityAsync(Guid activityId, UpdateActivityDto updateActivityDto)
    {
        var activityResult = await unitOfWork.ActivityRepository.GetActivityByIdAsync(activityId);
        if (!activityResult.IsSuccess)
            return Result.Failure(activityResult.ErrorMessage!);
        var activity = activityResult.Value!;
        mapper.Map(updateActivityDto,  activity);
        unitOfWork.ActivityRepository.UpdateActivityAsync(activity);
        try
        {
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Đã xảy ra lỗi khi cập nhật hoạt động ngoại khóa.");
        }
    }

    public async Task<Result> DeleteActivityAsync(Guid activityId)
    {
        var deleteResult = await unitOfWork.ActivityRepository.DeleteActivityAsync(activityId);
        if (!deleteResult.IsSuccess)
            return Result.Failure(deleteResult.ErrorMessage!);
        try
        {
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Đã xảy ra lỗi khi xóa hoạt động ngoại khóa.");
        }
    }

    public async Task<List<ExtracurricularActivityDto>> GetActivitiesInClassAsync(Guid classId)
    {
        var activities =  await activityRepository.GetActivitiesInClassAsync(classId);
        return mapper.Map<List<ExtracurricularActivityDto>>(activities);
    }

    public async Task<List<ParentViewActivityDto>> GetParentsActivityAsync(Guid classId, Guid studentId)
    {
        var result = new List<ParentViewActivityDto>();
        var activities =  await activityRepository.GetActivitiesInClassAsync(classId);
        var registerStatus = await activityRepository.GetSignInsForActivityAsync(classId, string.Empty);
        
        foreach (var activity in activities)
        {
            var dto = mapper.Map<ParentViewActivityDto>(activity);
            var signIn = registerStatus.FirstOrDefault(rs => rs.ActivityId == activity.Id && rs.StudentId == studentId);
            if (activity.Participants.Any(s => s.StudentId == studentId))
                dto.AssignStatus = EActivitySignInStatus.PARTICIPATED;
            else if (signIn != null)
                dto.AssignStatus = signIn.Status;
            else
                dto.AssignStatus = EActivitySignInStatus.NOT_SIGNED_IN;
            
            result.Add(dto);
        }

        return result;
    }

    public async Task<Result<ExtracurricularActivityDto>> GetActivityByIdAsync(Guid activityId)
    {
        var activityResult = await activityRepository.GetActivityByIdAsync(activityId);
        if (!activityResult.IsSuccess)
            return Result<ExtracurricularActivityDto>.Failure(activityResult.ErrorMessage!);
        var activityDto = mapper.Map<ExtracurricularActivityDto>(activityResult.Value!);
        return Result<ExtracurricularActivityDto>.Success(activityDto);
    }

    public async Task<Result> AddParticipantAsync(AddActivityParticipantDto addParticipantDto)
    {
        var existingParticipantResult = await unitOfWork.ActivityRepository.GetActivityByIdAsync(addParticipantDto.ActivityId);
        if (!existingParticipantResult.IsSuccess)
            return Result.Failure("Hoạt động ngoại khóa không tồn tại.");
        
        if (existingParticipantResult.Value!.Participants.Any(p => p.StudentId == addParticipantDto.StudentId))
            return Result.Failure("Học sinh đã tham gia hoạt động này.");
        
        var existingStudentResult = await unitOfWork.StudentRepository.GetByIdAsync(addParticipantDto.StudentId);
        if (!existingStudentResult.IsSuccess)
            return Result.Failure("Học sinh không tồn tại.");
        
        var participant = mapper.Map<ActivityParticipant>(addParticipantDto);
        await unitOfWork.ActivityRepository.AddActivityParticipantAsync(participant);
        try
        {
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Đã xảy ra lỗi khi thêm học sinh tham gia hoạt động.");
        }
    }

    public async Task<Result> AddParticipantsAsync(IEnumerable<AddActivityParticipantDto> addParticipantDtos)
    {
        var activityId = addParticipantDtos.FirstOrDefault()?.ActivityId;
        if (activityId == null)
            return Result.Failure("Không có học sinh");
        
        var existingParticipantResult = await unitOfWork.ActivityRepository.GetActivityByIdAsync(activityId.Value);
        if (!existingParticipantResult.IsSuccess)
            return Result.Failure("Hoạt động ngoại khóa không tồn tại.");

        foreach (var addParticipantDto in addParticipantDtos)
        {
            if (existingParticipantResult.Value!.Participants.Any(p => p.StudentId == addParticipantDto.StudentId))
                return Result.Failure("Học sinh đã tham gia hoạt động này.");
            
            var participant = mapper.Map<ActivityParticipant>(addParticipantDto);
            await unitOfWork.ActivityRepository.AddActivityParticipantAsync(participant);
        }
        
        try
        {
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Đã xảy ra lỗi khi thêm học sinh tham gia hoạt động.");
        }
    }

    public async Task<Result> UpdateParticipantAsync(Guid id, UpdateActivityParticipantDto updateParticipantDto)
    {
        var existingParticipantResult = await unitOfWork.ActivityRepository.GetActivityParticipantByIdAsync(id);
        if (!existingParticipantResult.IsSuccess)
            return Result.Failure("Người tham gia hoạt động không tồn tại.");
        
        var participant = existingParticipantResult.Value!;
        mapper.Map(updateParticipantDto, participant);
        unitOfWork.ActivityRepository.UpdateActivityParticipantAsync(participant);
        try
        {
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Đã xảy ra lỗi khi cập nhật người tham gia hoạt động.");
        }
    }

    public async Task<Result> RemoveParticipantAsync(Guid id)
    {
        var removeResult = await unitOfWork.ActivityRepository.RemoveActivityParticipantAsync(id);
        if (!removeResult.IsSuccess)
            return Result.Failure(removeResult.ErrorMessage!);
        try
        {
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Đã xảy ra lỗi khi xóa người tham gia hoạt động.");
        }
    }

    public async Task<Result<Guid>> AddSignInAsync(AddActivitySignInDto addSignInDto)
    {
        var existingActivityResult = await unitOfWork.ActivityRepository.GetActivityByIdAsync(addSignInDto.ActivityId);
        if (!existingActivityResult.IsSuccess)
            return Result<Guid>.Failure("Hoạt động ngoại khóa không tồn tại.");
        
        if (existingActivityResult.Value!.Participants.Any(si => si.StudentId == addSignInDto.StudentId))
            return Result<Guid>.Failure("Học sinh đã đăng ký tham gia hoạt động này.");
        
        var existingStudentResult = await unitOfWork.StudentRepository.GetByIdAsync(addSignInDto.StudentId);
        if (!existingStudentResult.IsSuccess)
            return Result<Guid>.Failure("Học sinh không tồn tại.");
        
        var existingSignIns = await unitOfWork.ActivityRepository.GetSignInsForActivityAsync(addSignInDto.ActivityId, "");
        if (existingSignIns.Any(si => si.StudentId == addSignInDto.StudentId))
            return Result<Guid>.Failure("Học sinh đã đăng ký tham gia hoạt động này.");
        
        var signIn = mapper.Map<ActivitySignIn>(addSignInDto);
        await unitOfWork.ActivityRepository.AddActivitySignInAsync(signIn);
        try
        {
            await unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(signIn.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure("Đã xảy ra lỗi khi thêm đăng ký tham gia hoạt động.");
        }
    }

    public async Task<Result> RemoveSignInAsync(Guid id)
    {
        var removeResult = await unitOfWork.ActivityRepository.RemoveActivitySignInAsync(id);
        if (!removeResult.IsSuccess)
            return Result.Failure(removeResult.ErrorMessage!);
        try
        {
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Đã xảy ra lỗi khi xóa đăng ký tham gia hoạt động.");
        }
    }

    public async Task<Result> AcceptSignInAsync(Guid id)
    {
        return await SetSignInStatusAsync(id, EActivitySignInStatus.ACCEPTED);
    }

    public async Task<Result> RejectSignInAsync(Guid id)
    {
        return await SetSignInStatusAsync(id, EActivitySignInStatus.REJECTED);
    }

    public async Task<Result> CheckPaidSignInAsync(Guid id)
    {
        return await SetSignInStatusAsync(id, EActivitySignInStatus.PAID);
    }

    public async Task<List<ActivitySignInDto>> GetSignInsForActivityAsync(Guid classId, string status)
    {
        var signIns =  await activityRepository.GetSignInsForActivityAsync(classId, status);
        return mapper.Map<List<ActivitySignInDto>>(signIns);
    }
    
    private async Task<Result> SetSignInStatusAsync(Guid id, string status)
    {
        var signInResult = await unitOfWork.ActivityRepository.GetActivitySignInByIdAsync(id);
        if (!signInResult.IsSuccess)
            return Result.Failure(signInResult.ErrorMessage!);
        var signIn = signInResult.Value!;
        signIn.Status = status;
        unitOfWork.ActivityRepository.UpdateActivitySignInAsync(signIn);
        
        var activityResult = await unitOfWork.ActivityRepository.GetActivityByIdAsync(signIn.ActivityId);
        if (!activityResult.IsSuccess)
            return Result.Failure(activityResult.ErrorMessage!);
        var activity =  activityResult.Value!;
        if (status == EActivitySignInStatus.PAID || (status == EActivitySignInStatus.ACCEPTED && activity.Cost == 0))
        {
            var parentResult = await unitOfWork.StudentRepository.GetParentByStudentIdAsync(signIn.StudentId);
            var participant = new ActivityParticipant
            {
                ActivityId = signIn.ActivityId,
                StudentId = signIn.StudentId,
                ParentPhoneNumber = parentResult.IsSuccess ? parentResult.Value!.PhoneNumber : "",
            };
            await unitOfWork.ActivityRepository.AddActivityParticipantAsync(participant);
        }
        
        try
        {
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Đã xảy ra lỗi khi cập nhật trạng thái đăng ký tham gia hoạt động.");
        }
    }
}