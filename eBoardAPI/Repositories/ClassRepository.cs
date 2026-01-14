using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories;

public class ClassRepository(AppDbContext dbContext) : IClassRepository
{
    public async Task<IEnumerable<Grade>> GetAllGradesAsync()
    {
        return await dbContext.Grades.ToListAsync();
    }

    public async Task<IEnumerable<Class>> GetAllTeachingClassByTeacherAsync(Guid teacherId)
    {
        var query = from c in dbContext.Classes
            where c.TeacherId == teacherId
                  && c.StartDate <= DateOnly.FromDateTime(DateTime.Now)
                  && c.EndDate >= DateOnly.FromDateTime(DateTime.Now)
            orderby c.StartDate descending
            select c;
        return await query
            .Include(c => c.Teacher)
            .Include(c => c.Grade)
            .ToListAsync();
    }

    public async Task<IEnumerable<Class>> GetAllClassesByTeacherAsync(Guid teacherId, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Student>> GetAllStudentsByClassAsync(Guid classId, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Class>> GetClassByIdAsync(Guid classId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> AddNewClassAsync(Class newClass)
    {
        throw new NotImplementedException();
    }
}