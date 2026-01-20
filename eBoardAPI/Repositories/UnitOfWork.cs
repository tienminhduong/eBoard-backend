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
    IScheduleRepository? _scheduleRepository;
    ISubjectRepository? _subjectRepository;
    IScoreRepository? _scoreRepository;
    IAttendanceRepository? _attendanceRepository;
    IFundIncomeDetailRepository? _fundIncomeDetailRepository;
    IViolationRepository? _violationRepository;

    public IViolationRepository ViolationRepository
    {
        get
        {
            _violationRepository ??= new ViolationRepository(dbContext);
            return _violationRepository;
        }
    }
    IAbsentRequestRepository? _absentRequestRepository;

    public IFundIncomeDetailRepository FundIncomeDetailRepository
    {
        get
        {
            _fundIncomeDetailRepository ??= new FundIncomeDetailRepository(dbContext);
            return _fundIncomeDetailRepository;
        }
    }

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
    
    public IAttendanceRepository AttendanceRepository
    {
        get
        {
            _attendanceRepository ??= new AttendanceRepository(dbContext);
            return _attendanceRepository;
        }
    }
    
    public IAbsentRequestRepository AbsentRequestRepository
    {
        get
        {
            _absentRequestRepository ??= new AbsentRequestRepository(dbContext);
            return _absentRequestRepository;
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