using eBoardAPI.Context;
using eBoardAPI.Interfaces.Repositories;

namespace eBoardAPI.Repositories;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    IStudentRepository? _studentRepository;
    IClassRepository? _classRepository;
    IParentRepository? _parentRepository;
    IClassFundRepository? _classFundRepository;

    public IClassFundRepository ClassFundRepository
    {
        get
        {
            _classFundRepository ??= new ClassFundRepository(dbContext);
            return _classFundRepository;
        }
    }
    
    public IStudentRepository StudentRepository
    {
        get
        {
            _studentRepository ??= new StudentRepository();
            return _studentRepository;
        }
    }
    
    public IClassRepository ClassRepository
    {
        get
        {
            _classRepository ??= new ClassRepository(dbContext);
            return _classRepository;
        }
    }
    
    public IParentRepository ParentRepository
    {
        get
        {
            _parentRepository ??= new ParentRepository();
            return _parentRepository;
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }
}