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
        return await dbContext.Grades.AsNoTracking().ToListAsync();
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
            .AsNoTracking()
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
            .AsNoTracking()
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
            .AsNoTracking()
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
            .AsNoTracking()
            .FirstOrDefaultAsync();
        
        return classEntity == null ? Result<Class>.Failure("Lớp không tồn tại") : Result<Class>.Success(classEntity);
    }

    public async Task<bool> ClassExistsAsync(Guid classId)
    {
        return await dbContext.Classes.AnyAsync(c => c.Id == classId);
    }

    public async Task<Class> AddNewClassAsync(Class newClass)
    {
        await dbContext.Classes.AddAsync(newClass);
        return newClass;
    }

    public async Task<Result> AddNewStudentsToClassAsync(Guid classId, List<Guid> studentIds)
    {
        var classResult = await GetClassByIdAsync(classId);
        if (!classResult.IsSuccess)
            return Result.Failure(classResult.ErrorMessage!);
        
        // Check if all students exist, if not return error with missing student ids
        var pendingStudent = dbContext.ChangeTracker
            .Entries<Student>()
            .Where(s => s.State == EntityState.Added && studentIds.Contains(s.Entity.Id))
            .Select(s => s.Entity.Id)
            .ToList();
        
        var existingStudents = await dbContext.Students
            .Where(s => studentIds.Contains(s.Id))
            .Select(s => s.Id)
            .ToListAsync();
        
        existingStudents.AddRange(pendingStudent);
        
        var missingStudentIds = studentIds.Except(existingStudents).ToList();
        if (missingStudentIds.Any())
            return Result.Failure("Một số học sinh không tồn tại: " + string.Join(", ", missingStudentIds));
        
        // Add students to class
        foreach (var studentId in studentIds)
        {
            await dbContext.InClasses.AddAsync(new InClass
            {
                ClassId = classId,
                StudentId = studentId,
            });
            classResult.Value!.CurrentStudentCount += 1;
        }
        return Result.Success();
    }

    public async Task<bool> IsStudentInClassAsync(Guid classId, Guid studentId)
    {
        return await dbContext.InClasses.AnyAsync(ic => ic.ClassId == classId && ic.StudentId == studentId);
    }
}