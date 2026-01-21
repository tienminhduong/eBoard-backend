using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Models.Violation;
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
                    .Where(v => v.Id == id)
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

        public async Task<Result<IEnumerable<ViolationStudent>>> GetViolationStudentByViolationIdAsync(Guid id)
        {
            try
            {
                var violationStudents = await dbContext.ViolationStudents
                    .AsNoTracking()
                    .Where(vs => vs.ViolationId == id)
                    .ToListAsync();
                return Result<IEnumerable<ViolationStudent>>.Success(violationStudents);
            }
            catch
            {
                return Result<IEnumerable<ViolationStudent>>.Failure("Lỗi trong quá trình lấy dữ liệu");
            }
        }

        public async Task<Result> RemoveRangeViolationStudentsAsync(IEnumerable<ViolationStudent> violationStudent)
        {
            try
            {
                dbContext.ViolationStudents.RemoveRange(violationStudent);
                return Result.Success();
            }
            catch
            {
                return Result.Failure("Lỗi trong quá trình xóa vi phạm cho học sinh");
            }
        }

        public async Task<Result> UpdateAsync(Violation violation)
        {
            try
            {
                dbContext.Violations.Update(violation);
                return Result.Success();
            }
            catch
            {
                return Result.Failure("Lỗi trong quá trình cập nhật vi phạm");
            }
        }

        public async Task<Result> UpdateAndSaveAsync(Violation violation)
        {
            try
            {
                dbContext.Violations.Update(violation);
                await dbContext.SaveChangesAsync();
                return Result.Success();
            }
            catch
            {
                return Result.Failure("Lỗi trong quá trình cập nhật vi phạm");
            }
        }

        public async Task<Result<ViolationStudent>>GetViolationStudentByIds(Guid violationId, Guid studentId)
        {
            try
            {
                var violationStudent = await dbContext.ViolationStudents
                    .AsNoTracking()
                    .FirstOrDefaultAsync(vs => vs.ViolationId == violationId && vs.StudentId == studentId);
                if (violationStudent == null)
                {
                    return Result<ViolationStudent>.Failure("Vi phạm cho học sinh không tồn tại");
                }
                return Result<ViolationStudent>.Success(violationStudent);
            }
            catch
            {
                return Result<ViolationStudent>.Failure("Lỗi trong quá trình lấy dữ liệu");
            }
        }

        public async Task<Result> UpdateAsync(ViolationStudent violationStudent)
        {
            try
            {
                dbContext.ViolationStudents.Update(violationStudent);
                await dbContext.SaveChangesAsync();
                return Result.Success();
            }
            catch
            {
                return Result.Failure("Lỗi trong quá trình cập nhật vi phạm cho học sinh");
            }
        }

        public async Task<Result<SummaryViolation>> GetSummaryViolationAsync(Guid classId, Guid studentId)
        {
            try
            {
                var violations = await dbContext.Violations
                    .AsNoTracking()
                    .Where(v => v.ClassId == classId)
                    .Include(v => v.Students)
                    .ThenInclude(vs => vs.Student)
                    .Where(v => v.Students.Any(vs => vs.StudentId == studentId))
                    .ToListAsync();
                var unreadCount = violations.Count(v => !v.Students.First(vs => vs.StudentId == studentId).SeenByParent);
                var readCount = violations.Count - unreadCount;
                var summary = new SummaryViolation
                {
                    UnreadCount = unreadCount,
                    ReadCount = readCount
                };
                return Result<SummaryViolation>.Success(summary);
            }
            catch
            {
                return Result<SummaryViolation>.Failure("Lỗi trong quá trình lấy tóm tắt vi phạm");
            }
        }

        public async Task<Result<IEnumerable<ViolationStudent>>> GetViolationsByClassIdAndStudentId(Guid classId, Guid studentId, int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                // get violations by classId and studentId with pagination and order by violate date and seenbyparent
                var violationStudents = await dbContext.ViolationStudents
                                .Where(vs =>
                                    vs.StudentId == studentId &&
                                    vs.Violation.ClassId == classId)
                                .Include(vs => vs.Violation)
                                .OrderBy(vs => vs.SeenByParent)                  // chưa xem trước
                                .ThenByDescending(vs => vs.Violation.ViolateDate)
                                .Skip((pageNumber - 1) * pageSize)
                                .ToListAsync();

                return Result<IEnumerable<ViolationStudent>>.Success(violationStudents);
            }
            catch
            {
                return Result<IEnumerable<ViolationStudent>>.Failure("Lỗi trong quá trình lấy vi phạm cho học sinh");
            }
        }

        public async Task<Result<IEnumerable<Violation>>> GetViolationsByClassIdAsync(Guid classId, int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                var violations = await dbContext.Violations
                    .AsNoTracking()
                    .Include(v => v.Students)
                    .ThenInclude(vs => vs.Student)
                    .Where(v => v.ClassId == classId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(v => v.ViolateDate)
                    .ToListAsync();
                return Result<IEnumerable<Violation>>.Success(violations);
            }
            catch
            {
                return Result<IEnumerable<Violation>>.Failure("Lỗi trong quá trình lấy vi phạm");
            }
        }
    }
}
