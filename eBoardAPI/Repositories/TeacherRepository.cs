using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;

namespace eBoardAPI.Repositories;

public class TeacherRepository(AppDbContext db) : ITeacherRepository
{
    public async Task<Result<Teacher>> GetByIdAsync(Guid id)
    {
        var teacher = await db.Teachers.FindAsync(id);
        return teacher != null ? Result<Teacher>.Success(teacher)
                               : Result<Teacher>.Failure("Teacher is not found");
    }

    public async Task<Result<Teacher>> UpdateAsync(Teacher teacher)
    {
        db.Teachers.Update(teacher);
        var rowEffect = await db.SaveChangesAsync();
        return rowEffect > 0 ? Result<Teacher>.Success(teacher)
                             : Result<Teacher>.Failure("Failed to update teacher information");
    }
}