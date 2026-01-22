using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories;

public class StudentRepository(AppDbContext db) : IStudentRepository
{
    public async Task<Student> AddAsync(Student student)
    {
        await db.Students.AddAsync(student);
        return student;
    }

    public async Task<bool> StudentExistsAsync(Guid studentId)
    {
        return await db.Students.AnyAsync(s => s.Id == studentId);
    }

    public async Task<IEnumerable<Tuple<Guid, string>>> GetStudentsOptionInClassAsync(Guid classId)
    {
        var query = from s in db.Students
            join ic in db.InClasses on s.Id equals ic.StudentId
            where ic.ClassId == classId
            select new Tuple<Guid, string>(s.Id, $"{s.LastName} {s.FirstName}");
        
        var result = await query.ToListAsync();
        return result;
    }

    public void UpdateStudent(Student student)
    {
        db.Students.Update(student);
    }

    public async Task<Result<Parent>> GetParentByStudentIdAsync(Guid studentId)
    {
        var query = from p in db.Parents
                    join s in db.Students on p.Id equals s.ParentId
                    where s.Id == studentId
                    select p;
        var parent =  await query.FirstOrDefaultAsync();
        return (parent != null) ? Result<Parent>.Success(parent)
                                : Result<Parent>.Failure("Phụ huynh không tồn tại");
    }

    public async Task<Result<Student>> GetByIdAsync(Guid id)
    {
        if(id == Guid.Empty)
            return Result<Student>.Failure("Id không được để trống");
        
        var query = from s in db.Students
                    where s.Id == id
                    select s;
        
        var student = await query.Include(s => s.Parent)
                                 .FirstOrDefaultAsync();
        
        return (student != null) ? Result<Student>.Success(student)
                                 : Result<Student>.Failure("Học sinh không tồn tại");
    }

    public async Task<Result<int>> CountStudentByClassIdAsync(Guid classId)
    {
        try
        {
            return Result<int>.Success(await db.InClasses
                .Where(ic => ic.ClassId == classId)
                .CountAsync());
        }
        catch (Exception ex)
        {
            return Result<int>.Failure($"Đếm học sinh thất bại: {ex.Message}");
        }
    }
}