using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;

namespace eBoardAPI.Repositories;

public class StudentRepository(AppDbContext db) : IStudentRepository
{
    public async Task<bool> AddAsync(Student student)
    {
        if (student == null)
            return false;
        try
        {
            await db.Students.AddAsync(student);
            await db.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Student?> GetByIdAsync(Guid id)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty", nameof(id));
        }
        
        var student = await db.Students.FindAsync(id);
        return student;
    }
}