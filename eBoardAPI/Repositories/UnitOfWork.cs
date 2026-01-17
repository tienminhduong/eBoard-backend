using eBoardAPI.Context;
using eBoardAPI.Interfaces.Repositories;

namespace eBoardAPI.Repositories;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    IStudentRepository? _studentRepository;
    IClassRepository? _classRepository;
    IParentRepository? _parentRepository;
    IClassFundRepository? _classFundRepository;
    IScheduleRepository? _scheduleRepository;
    ISubjectRepository? _subjectRepository;
    IScoreRepository? _scoreRepository;

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

    public ISubjectRepository SubjectRepository
    {
        get
        {
            _subjectRepository ??= new SubjectRepository(dbContext);
            return _subjectRepository;
        }
    }

    public IScoreRepository ScoreRepository
    {
        get
        {
            _scoreRepository ??= new ScoreRepository(dbContext);
            return _scoreRepository;
        }
    }

    public IScheduleRepository ScheduleRepository
    {
        get
        {
            _scheduleRepository ??= new ScheduleRepository(dbContext);
            return _scheduleRepository;
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