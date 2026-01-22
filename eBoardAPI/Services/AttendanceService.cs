using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Consts;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.AbsentRequest;
using eBoardAPI.Models.Attendance;

namespace eBoardAPI.Services;

[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
public class AttendanceService(
    IAttendanceRepository attendanceRepository,
    IAbsentRequestRepository absentRequestRepository,
    IUnitOfWork unitOfWork,
    IParentNotificationService parentNotificationService,
    IMapper mapper
    ) : IAttendanceService
{
    public async Task<Result<AttendanceInfoByClassDto>> GetAttendanceInfoByClassAsync(Guid classId, DateOnly date)
    {
        var attendances = await attendanceRepository.GetAttendancesByClassAndDateAsync(classId, date);
        if (!attendances.Any())
            return Result<AttendanceInfoByClassDto>.Failure("Không tìm thấy thông tin điểm danh cho lớp học và ngày đã cho.");
        var attendanceDtos = mapper.Map<List<AttendanceDto>>(attendances);
        
        var attendanceInfo = new AttendanceInfoByClassDto
        {
            ClassId = classId,
            ClassName = attendances.FirstOrDefault()?.Class.Name ?? "Unknown Class",
            Date = date,
            Attendances = attendanceDtos
        };
        return Result<AttendanceInfoByClassDto>.Success(attendanceInfo);
    }

    public async Task<Result<AttendanceInfoByClassDto>> CreateAttendaceForDateAsync(CreateAttendaceForDateDto dto)
    {
        var isAttendanceExist = await unitOfWork
            .AttendanceRepository
            .ValidateAttendancesExistAsync(dto.ClassId, dto.Date);
        
        if (isAttendanceExist)
            return Result<AttendanceInfoByClassDto>.Failure("Thông tin điểm danh cho lớp học ngày này đã tồn tại.");
        
        var @class = await unitOfWork.ClassRepository.GetClassByIdAsync(dto.ClassId);
        if (!@class.IsSuccess)
            return Result<AttendanceInfoByClassDto>.Failure("Lớp học không tồn tại.");
        
        var students = await unitOfWork.ClassRepository.GetStudentsByClassAsync(dto.ClassId, 1, 200);
        
        var absentRequests = await unitOfWork
            .AbsentRequestRepository
            .GetAcceptedAbsentRequestsByDateAsync(dto.ClassId, dto.Date);
        var absentRequestMap = absentRequests.ToDictionary(ar => ar.StudentId);
        
        var attendances = students.Select(student => new Attendance
        {
            ClassId = dto.ClassId,
            StudentId = student.Id,
            Date = dto.Date,
            Status = absentRequestMap.ContainsKey(student.Id) ? EAttendanceStatus.ABSENT : EAttendanceStatus.PRESENT,
            AbsenceReason = absentRequestMap.TryGetValue(student.Id, out var reasonValue) ? reasonValue.Reason : string.Empty,
            Notes = absentRequestMap.TryGetValue(student.Id, out var notesValue) ? notesValue.Notes : string.Empty,
        }).ToList();
        
        await unitOfWork.AttendanceRepository.CreateAttendancesAsync(attendances);

        try
        {
            await unitOfWork.SaveChangesAsync();
            
            var studentMap = students.ToDictionary(s => s.Id, s => $"{s.LastName} {s.FirstName}");
            var attendanceInfo = new AttendanceInfoByClassDto
            {
                ClassId = dto.ClassId,
                ClassName = @class.Value!.Name,
                Date = dto.Date,
                Attendances = mapper.Map<List<AttendanceDto>>(attendances)
            };
            foreach (var attendanceDto in attendanceInfo.Attendances)
            {
                if (studentMap.TryGetValue(attendanceDto.StudentId, out var fullName))
                    attendanceDto.StudentName = fullName;
            }
            return Result<AttendanceInfoByClassDto>.Success(attendanceInfo);
        }
        catch (Exception ex)
        {
            return Result<AttendanceInfoByClassDto>.Failure("Đã xảy ra lỗi khi tạo thông tin điểm danh: " + ex.Message);
        }
    }

    public async Task<Result> PatchAttendanceRecordAsync(Guid attendanceId, PatchAttendanceDto dto)
    {
        var attendance = await attendanceRepository.GetAttendanceByIdAsync(attendanceId);
        if (attendance == null)
            return Result.Failure("Bản ghi điểm danh không tồn tại.");
        
        if (dto.Status is not null)
            attendance.Status = dto.Status;
        if (dto.AbsenceReason is not null)
            attendance.AbsenceReason = dto.AbsenceReason;
        if (dto.Notes is not null)
            attendance.Notes = dto.Notes;
        if (dto.PickupPerson is not null)
            attendance.PickupPerson = dto.PickupPerson;
        
        attendanceRepository.UpdateAttendanceAsync(attendance);
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> RegisterAbsencesForStudentInClassAsync(CreateAbsentRequestDto requestDto)
    {
        var result = await unitOfWork.ClassRepository.ValidateStudentsInClassAsync(requestDto.ClassId, [requestDto.StudentId]);
        if (!result.Any())
            return Result.Failure("Học sinh không thuộc lớp học đã cho.");
        
        var absentRequest = mapper.Map<AbsentRequest>(requestDto);
        await unitOfWork.AbsentRequestRepository.CreateAbsentRequestAsync(absentRequest);
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> ApproveAbsenceRequestAsync(Guid requestId)
    {
        var absentRequestResult = await absentRequestRepository.GetAbsentRequestById(requestId);
        if (!absentRequestResult.IsSuccess)
            return Result.Failure(absentRequestResult.ErrorMessage!);
        
        var absentRequest = absentRequestResult.Value!;
        absentRequest.Status = EAbsentRequestStatus.APPROVED;
        absentRequestRepository.UpdateAbsentRequest(absentRequest);
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> RejectAbsenceRequestAsync(Guid requestId)
    {
        var absentRequestResult = await absentRequestRepository.GetAbsentRequestById(requestId);
        if (!absentRequestResult.IsSuccess)
            return Result.Failure(absentRequestResult.ErrorMessage!);
        
        var absentRequest = absentRequestResult.Value!;
        absentRequest.Status = EAbsentRequestStatus.REJECTED;
        absentRequestRepository.UpdateAbsentRequest(absentRequest);
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<IEnumerable<AbsentRequestDto>> GetAbsentRequestsForClassAsync(Guid classId, string status, int pageNumber, int pageSize)
    {
        var absentRequests = await absentRequestRepository.GetAbsentRequestsByClassIdAsync(classId, status, pageNumber, pageSize);
        return mapper.Map<List<AbsentRequestDto>>(absentRequests);
    }

    public async Task<Result<ClassAttendanceSummary>> GetClassAttendanceSummaryAsync(Guid classId, DateOnly date)
    {
        var (absentWithExcuseCount, absentWithoutExcuseCount) = await attendanceRepository.GetAttendanceCountsByClassAndDateAsync(classId, date);
        var studentCountResult = await unitOfWork.ClassRepository.GetClassByIdAsync(classId);
        
        if (!studentCountResult.IsSuccess)
            return Result<ClassAttendanceSummary>.Failure("Lớp học không tồn tại.");
        
        var summary = new ClassAttendanceSummary
        {
            AbsentWithExcuse = absentWithExcuseCount,
            AbsentWithoutExcuse = absentWithoutExcuseCount,
            StudentCount = studentCountResult.Value!.CurrentStudentCount,
            CurrentAttendance = studentCountResult.Value!.CurrentStudentCount - absentWithExcuseCount - absentWithoutExcuseCount
        };
        return Result<ClassAttendanceSummary>.Success(summary);
    }

    public async Task SendNotificationForAbsenceWithoutExcuseToParentsAsync(Guid classId, DateOnly date)
    {
        var studentsWithAbsenceWithoutExcuse = await attendanceRepository.GetStudentsWithAbsenceWithoutExcuseAsync(classId, date);
        foreach (var student in studentsWithAbsenceWithoutExcuse)
        {
            await parentNotificationService.SendNotificationToParentAsync(student.ParentId,
                $"Học sinh {student.LastName} {student.FirstName} đã vắng mặt không lý do trong lớp ngày {date.ToString("dd/MM/yyyy")}.");
        }
    }

    public async Task<IEnumerable<string>> GetRecentPickUpPersonForStudentAsync(Guid studentId, int limit)
    {
        return await attendanceRepository.GetRecentPickUpPersonForStudentAsync(studentId, limit);
    }
}