namespace eBoardAPI.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IFundIncomeRepository FundIncomeRepository { get; }
    IFundExpenseRepository FundExpenseRepository { get; }
    IClassFundRepository ClassFundRepository { get; }
    IStudentRepository StudentRepository { get; }
    IClassRepository ClassRepository { get; }
    IParentRepository ParentRepository { get; }
    IScheduleRepository ScheduleRepository { get; }
    ISubjectRepository SubjectRepository { get; }
    IScoreRepository ScoreRepository { get; }
    
    Task<int> SaveChangesAsync();
}