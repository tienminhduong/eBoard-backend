using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories;

public class ParentRepository(AppDbContext dbContext) : IParentRepository
{
    public async Task<Result<Parent>> GetByIdAsync(Guid id)
    {
        var parent = await dbContext.Parents.FindAsync(id);
        var result = parent == null
            ? Result<Parent>.Failure("Parent not found")
            : Result<Parent>.Success(parent);
        return result;
    }

    public async Task<Result> Update(Parent parent)
    {
        dbContext.Parents.Update(parent);
        var effectRow = await dbContext.SaveChangesAsync();
        return effectRow > 0
            ? Result.Success()
            : Result.Failure("Failed to update parent");
    }

    public async Task<Result<Parent>> GetByPhoneNumberAsync(string phoneNumber)
    {
        var query = from p in dbContext.Parents
            where p.PhoneNumber == phoneNumber
            select p;

        var parent = await query
            .AsNoTracking()
            .FirstOrDefaultAsync();
        
        return parent != null
            ? Result<Parent>.Success(parent)
            : Result<Parent>.Failure("Không tìm thấy phụ huynh với số điện thoại đã cho.");
    }

    public async Task<Parent> AddNewParentAsync(Parent parent)
    {
        await dbContext.Parents.AddAsync(parent);
        return parent;
    }

    public async Task<IEnumerable<Parent>> GetParentsByIdsAsync(List<Guid> parentIds)
    {
        return await dbContext.Parents
            .AsNoTracking()
            .Where(p => parentIds.Contains(p.Id))
            .ToListAsync();
    }

    public async Task UpdateRangeParentsAsync(IEnumerable<Parent> parents)
    {
        dbContext.Parents.UpdateRange(parents);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Parent>> GetParentNotCreateAccountByClassId(Guid classId, int pageNumber = 1, int pageSize = 20)
    {
        var query = await dbContext.InClasses
            .AsNoTracking()
            .Where(ic => ic.ClassId == classId)
            .Include(ic => ic.Student)
            .ThenInclude(s => s.Parent)
            .Select(ic => ic.Student.Parent)
            .Where(pr => pr.GeneratedPassword == "")
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return query;
    }

    public async Task<IEnumerable<Parent>> GetParentCreateAccountByClassId(Guid classId, int pageNumber = 1, int pageSize = 20)
    {
        var query = await dbContext.InClasses
            .AsNoTracking()
            .Where(ic => ic.ClassId == classId)
            .Include(ic => ic.Student)
            .ThenInclude(s => s.Parent)
            .Select(ic => ic.Student.Parent)
            .Where(pr => pr.GeneratedPassword != "")
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return query;
    }

    public async Task<IEnumerable<Tuple<Student, Class>>> GetStudentsWithClassesByParentIdAsync(Guid parentId)
    {
        var query = dbContext.InClasses
            .Include(ic => ic.Student)
            .Where(ic => ic.Student.ParentId == parentId)
            .Include(ic => ic.Class)
            .ThenInclude(c => c.Grade)
            .Include(ic => ic.Class)
            .ThenInclude(c => c.Teacher)
            .Select(ic => new Tuple<Student, Class>(ic.Student, ic.Class));
        
        return await query
            .AsNoTracking()
            .ToListAsync();
    }
}