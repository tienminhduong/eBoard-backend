using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Models.ExamSchedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<Result<IEnumerable<ExamSchedule>>> GetExamScheduleByClassIdAndQuery(Guid classId, ExamScheduleFilter filter)
        {
            try
            {
                var query = dbContext.ExamSchedules
                    .AsNoTracking()
                    .Include(es => es.Subject)
                    .Where(es => es.ClassId == classId);

                // filtering
                query = filter.From.HasValue
                    ? query.Where(es => es.StartTime >= filter.From.Value)
                    : query;

                query = filter.To.HasValue
                    ? query.Where(es => es.StartTime <= filter.To.Value)
                    : query;

                query = filter.SubjectId.HasValue
                    ? query.Where(es => es.SubjectId == filter.SubjectId.Value)
                    : query;

                query = !string.IsNullOrEmpty(filter.ExamFormat)
                    ? query.Where(es => es.ExamFormat == filter.ExamFormat)
                    : query;

                // pagination
                var examSchedules = await query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync();
                return Result<IEnumerable<ExamSchedule>>.Success(examSchedules);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ExamSchedule>>.Failure(ex.Message);
            }
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

        public async Task<Result> UpdateAsync(ExamSchedule examSchedule)
        {
            try
            {
                dbContext.ExamSchedules.Update(examSchedule);
                await dbContext.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
