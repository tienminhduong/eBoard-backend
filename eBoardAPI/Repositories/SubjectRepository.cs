using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories;

public class SubjectRepository(AppDbContext dbContext) : ISubjectRepository
{
    public async Task<Result<Subject>> GetSubjectByIdAsync(Guid subjectId)
    {
        var subject = await dbContext.Subjects.FindAsync(subjectId);
        return subject == null
            ? Result<Subject>.Failure("Không tìm thấy môn học")
            : Result<Subject>.Success(subject);
    }

    public async Task<Result<Subject>> GetSubjectByNameAsync(string subjectName)
    {
        var query = from s in dbContext.Subjects
                    where s.Name == subjectName
                    select s;
        
        var subject = await query
            .AsNoTracking()
            .FirstOrDefaultAsync();
        
        return subject == null
            ? Result<Subject>.Failure("Không tìm thấy môn học")
            : Result<Subject>.Success(subject);
    }

    public async Task<Subject> GetOrAddSubjectByNameAsync(string subjectName)
    {
        var existingResult = await GetSubjectByNameAsync(subjectName);
        if (existingResult.IsSuccess)
            return existingResult.Value!;

        var newSubject = new Subject { Name = subjectName };
        
        await dbContext.Subjects.AddAsync(newSubject);
        return newSubject;
    }

    public async Task<Subject> AddSubjectAsync(Subject subject)
    {
        await dbContext.Subjects.AddAsync(subject);
        return subject;
    }

    public async Task<IEnumerable<Subject>> GetAllSubjectsAsync() => await dbContext.Subjects.AsNoTracking().ToListAsync();
}