using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;

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
        
        var student = await db.Students.FindAsync(id);
        return (student != null) ? Result<Student>.Success(student)
                                 : Result<Student>.Failure("Student is not exsist");
    }
}