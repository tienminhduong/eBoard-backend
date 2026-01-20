using eBoardAPI.Common;
using eBoardAPI.Models.ExamSchedule;

namespace eBoardAPI.Interfaces.Services
{
    public interface IExamScheduleService
    {
        Task<Result<ExamScheduleDto>> CreateNewExamSchedule(CreateExamScheduleDto createExamScheduleDto);
        Task<Result<ExamScheduleDto>> GetExamScheduleById(Guid examScheduleId);
        Task<Result<IEnumerable<ExamScheduleDto>>> GetExamSchedules(Guid classId, ExamScheduleFilter filter);
        Task<Result> UpdateExamSchedule(UpdateExamScheduleDto updateExamScheduleDto);
        Task<Result> DeleteExamSchedule(Guid id);
    }
}
