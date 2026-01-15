using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories;

public class StudentRepository(AppDbContext db) : IStudentRepository
{
    public async Task<Result> AddAsync(Student student)
    {
        await db.Students.AddAsync(student);
        var row_effect = await db.SaveChangesAsync();
        return row_effect > 0
            ? Result.Success()
            : Result.Failure("Failed to add student");
    }

    public async Task<Result<Student>> GetByIdAsync(Guid id)
    {
        if(id == Guid.Empty)
        {
            return Result<Student>.Failure("Id must be not empty");
        }
        
        var query = from s in db.Students
                    where s.Id == id
                    select s;
        var student = await query.Include(s => s.Parent)
                                 .FirstOrDefaultAsync();
        return (student != null) ? Result<Student>.Success(student)
                                 : Result<Student>.Failure("Student is not exsist");
    }
}