using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories
{
    public class ViolationRepository(AppDbContext dbContext) : IViolationRepository
    {
        public async Task<Result<Violation>> AddAsync(Violation violation)
        {
            try
            {
                await dbContext.Violations.AddAsync(violation);
                return Result<Violation>.Success(violation);
            }
            catch
            {
                return Result<Violation>.Failure("Lỗi trong quá trình thêm vi phạm");
            }
        }
        
        public async Task<Result> AddRangeViolationStudentsAsync(IEnumerable<ViolationStudent> violationStudents)
        {
            try
            {
                await dbContext.ViolationStudents.AddRangeAsync(violationStudents);
                return Result.Success();
            }
            catch
            {
                return Result.Failure("Lỗi trong quá trình thêm vi phạm cho học sinh");
            }
        }

        public async Task<Result<IEnumerable<Violation>>> AddRangeAsync(IEnumerable<Violation> violations)
        {
            try
            {
                await dbContext.Violations.AddRangeAsync(violations);
                await dbContext.SaveChangesAsync();
                return Result<IEnumerable<Violation>>.Success(violations);
            }
            catch
            {
                return Result<IEnumerable<Violation>>.Failure("Lỗi trong quá trình thêm vi phạm");
            }
        }

        public async Task<Result<IEnumerable<Violation>>> GetRangeByIdsAsync(IEnumerable<Guid> ids)
        {
            try
            {
                var result = await dbContext.Violations
                    .Where(v => ids.Contains(v.Id))
                    .ToListAsync();
                return Result<IEnumerable<Violation>>.Success(result);
            }
            catch
            {
                return Result<IEnumerable<Violation>>.Failure("Lỗi trong quá trình lấy dữ liệu");
            }
        }

        public async Task<Result<Violation>> GetByIdAsync(Guid id)
        {
            try
            {
                var violation = await dbContext.Violations
                    .AsNoTracking()
                    .Include(v => v.Students)
                    .ThenInclude(vs => vs.Student)
                    .FirstOrDefaultAsync();
                if (violation == null)
                {
                    return Result<Violation>.Failure("Vi phạm không tồn tại");
                }
                return Result<Violation>.Success(violation);
            }
            catch
            {
                return Result<Violation>.Failure("Lỗi trong quá trình lấy dữ liệu");
            }
        }
    }
}
