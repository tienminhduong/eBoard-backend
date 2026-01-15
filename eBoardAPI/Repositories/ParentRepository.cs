using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories;

public class ParentRepository(AppDbContext db) : IParentRepository
{
    public async Task<Result<Parent>> GetByIdAsync(Guid id)
    {
        var parent = await db.Parents.FindAsync(id);
        var result = parent == null
            ? Result<Parent>.Failure("Parent not found")
            : Result<Parent>.Success(parent);
        return result;
    }

    public async Task<Result> Update(Parent parent)
    {
        db.Parents.Update(parent);
        var effectRow = await db.SaveChangesAsync();
        return effectRow > 0
            ? Result.Success()
            : Result.Failure("Failed to update parent");
    }
}