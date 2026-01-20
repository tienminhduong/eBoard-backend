using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Models.ExamSchedule;

namespace eBoardAPI.Repositories
{
    public class ExamScheduleRepository(AppDbContext dbContext) : IExamScheduleRepository
    {
        public async Task<Result<ExamSchedule>> AddAsync(ExamSchedule examSchedule)
        {
            try
            {
                var result = await dbContext.ExamSchedules.AddAsync(examSchedule);
                await dbContext.SaveChangesAsync();
                return Result<ExamSchedule>.Success(result.Entity);
            }
            catch(Exception ex)
            {
                return Result<ExamSchedule>.Failure(ex.Message);
            }
        }

        public async Task<Result> DeteleAsync(ExamSchedule examSchedule)
        {
            try
            {
                dbContext.ExamSchedules.Remove(examSchedule);
                await dbContext.SaveChangesAsync();
                return Result.Success();
            }
            catch(Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public Task<Result<IEnumerable<ExamSchedule>>> GetExamScheduleByClassIdAndQuery(Guid classId, ExamScheduleFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<ExamSchedule?>> GetExamScheduleByIdAsync(Guid id)
        {
            try
            {
                var examSchedule = await dbContext.ExamSchedules.FindAsync(id);
                return Result<ExamSchedule?>.Success(examSchedule);
            }
            catch(Exception ex)
            {
                return Result<ExamSchedule?>.Failure(ex.Message);
            }
        }

        public Task<Result> UpdateAsync(ExamSchedule examSchedule)
        {
            throw new NotImplementedException();
        }
    }
}
