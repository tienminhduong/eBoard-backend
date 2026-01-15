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
        var query = from c in dbContext.Classes
            where c.TeacherId == teacherId
            orderby c.StartDate descending
            select c;
        
        return await query
            .Include(c => c.Teacher)
            .Include(c => c.Grade)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Student>> GetStudentsByClassAsync(Guid classId, int pageNumber, int pageSize)
    {
        var query = from s in dbContext.Students
            join sc in dbContext.InClasses on s.Id equals sc.StudentId
            where sc.ClassId == classId
            orderby s.FirstName ascending
            select s;
        
        return await query
            .Include(s => s.Parent)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Result<Class>> GetClassByIdAsync(Guid classId)
    {
        var query = from c in dbContext.Classes
            where c.Id == classId
            select c;
        
        var classEntity = await query
            .Include(c => c.Teacher)
            .Include(c => c.Grade)
            .FirstOrDefaultAsync();
        
        return classEntity == null ? Result<Class>.Failure("Lớp không tồn tại") : Result<Class>.Success(classEntity);
    }

    public async Task<Class> AddNewClassAsync(Class newClass)
    {
        await dbContext.Classes.AddAsync(newClass);
        return newClass;
    }
}