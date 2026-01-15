using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories;

public class ParentRepository(AppDbContext db) : IParentRepository
{
    public async Task<Parent?> GetByIdAsync(Guid id)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentException("The provided ID is empty.", nameof(id));
        }

        var parent = await db.Parents.FindAsync(id);
        return parent;
    }

    public void Update(Parent parent)
    {
        if(parent == null)
        {
            throw new ArgumentNullException(nameof(parent), "Parent entity cannot be null.");
        }

        db.Parents.Update(parent);
        db.SaveChanges();
    }
}