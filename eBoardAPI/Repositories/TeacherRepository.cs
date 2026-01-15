using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;

namespace eBoardAPI.Repositories;

public class TeacherRepository(AppDbContext db) : ITeacherRepository
{
    public async Task<Teacher?> GetByIdAsync(Guid id)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentException("ID cannot be empty", nameof(id));
        }

        var teacher = await db.Teachers.FindAsync(id);
        return teacher;
    }

    public async Task<int> Update(Teacher teacher)
    {
        if(teacher == null)
        {
            throw new ArgumentNullException(nameof(teacher), "Teacher cannot be null");
        }
        db.Teachers.Update(teacher);
        return await db.SaveChangesAsync();
    }
}