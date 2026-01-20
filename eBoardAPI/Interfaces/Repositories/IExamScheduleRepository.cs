using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Models.ExamSchedule;

namespace eBoardAPI.Interfaces.Repositories
{
    public interface IExamScheduleRepository
    {
        Task<Result<ExamSchedule>> AddAsync(ExamSchedule examSchedule);
        Task<Result> DeteleAsync(ExamSchedule examSchedule);
        Task<Result<ExamSchedule>> GetExamScheduleByIdAsync(Guid id);
        Task<Result<IEnumerable<ExamSchedule>>> GetExamScheduleByClassIdAndQuery(Guid classId, ExamScheduleFilter filter);
        Task<Result> UpdateAsync(ExamSchedule examSchedule);
        Task<Result> DeleteAsync(ExamSchedule examSchedule);
    }
}
