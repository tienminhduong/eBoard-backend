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
            .FirstOrDefaultAsync();
        
        return classEntity == null ? Result<Class>.Failure("Lớp không tồn tại") : Result<Class>.Success(classEntity);
    }

    public async Task<bool> ClassExistsAsync(Guid classId)
    {
        return await dbContext.Classes.AnyAsync(c => c.Id == classId);
    }

    public void UpdateClass(Class @class)
    {
        dbContext.Classes.Update(@class);
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
        UpdateClass(classResult.Value!);
        return Result.Success();
    }

    public async Task<bool> IsStudentInClassAsync(Guid classId, Guid studentId)
    {
        return await dbContext.InClasses.AnyAsync(ic => ic.ClassId == classId && ic.StudentId == studentId);
    }

    public async Task<IEnumerable<Guid>> ValidateStudentsInClassAsync(Guid classId, IEnumerable<Guid> studentIds)
    {
        var query = from ic in dbContext.InClasses
            where ic.ClassId == classId
            select ic.StudentId;

        var studentsInClass = await query.ToListAsync();
        return studentIds.Intersect(studentsInClass);
    }

    public async Task<Result> RemoveStudentFromClassAsync(Guid classId, Guid studentId)
    {
        var inClassEntity = await dbContext.InClasses
            .FirstOrDefaultAsync(ic => ic.ClassId == classId && ic.StudentId == studentId);
        
        if (inClassEntity == null)
            return Result.Failure("Học sinh không thuộc lớp học.");
        
        dbContext.InClasses.Remove(inClassEntity);
        
        var classResult = await GetClassByIdAsync(classId);
        if (!classResult.IsSuccess)
            return Result.Failure(classResult.ErrorMessage!);
        
        classResult.Value!.CurrentStudentCount -= 1;
        UpdateClass(classResult.Value!);
        
        return Result.Success();
    }
}