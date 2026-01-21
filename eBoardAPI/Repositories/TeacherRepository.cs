using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public async Task<bool> EmailExistsAsync(string email)
            => await db.Teachers.AnyAsync(x => x.Email == email);

    public async Task<bool> PhoneExistsAsync(string phone)
        => await db.Teachers.AnyAsync(x => x.PhoneNumber == phone);

    public async Task<Result> AddAsync(Teacher teacher)
    {
        try
        {
            await db.Teachers.AddAsync(teacher);
            await db.SaveChangesAsync();
            return Result.Success();
        }
        catch
        {
            return Result.Failure("Failed to add teacher.");
        }
    }
}