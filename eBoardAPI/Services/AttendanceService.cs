using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Attendance;

namespace eBoardAPI.Services;

[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
public class AttendanceService(
    IAttendanceRepository attendanceRepository,
    IUnitOfWork unitOfWork,
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
        // check from absent request, if accepted, create with status absent
        
        var attendances = students.Select(student => new Attendance
        {
            ClassId = dto.ClassId,
            StudentId = student.Id,
            Date = dto.Date,
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
}