using eBoardAPI.Context;
using eBoardAPI.Interfaces.Repositories;

namespace eBoardAPI.Repositories;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    IStudentRepository? _studentRepository;
    IClassRepository? _classRepository;
    IParentRepository? _parentRepository;
    IClassFundRepository? _classFundRepository;
    IFundExpenseRepository? _fundExpenseRepository;
    IFundIncomeRepository? _fundIncomeRepository;

    public IFundIncomeRepository FundIncomeRepository
    {
        get
        {
            _fundIncomeRepository ??= new FundIncomeRepository(dbContext);
            return _fundIncomeRepository;
        }
    }

    public IFundExpenseRepository FundExpenseRepository
    {
        get
        {
            _fundExpenseRepository ??= new FundExpenseRepository(dbContext);
            return _fundExpenseRepository;
        }
    }

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
            _studentRepository ??= new StudentRepository(dbContext);
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
            _parentRepository ??= new ParentRepository(dbContext);
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