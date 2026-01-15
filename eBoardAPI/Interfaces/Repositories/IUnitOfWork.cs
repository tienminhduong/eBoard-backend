namespace eBoardAPI.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IClassFundRepository ClassFundRepository { get; }
    IStudentRepository StudentRepository { get; }
    IClassRepository ClassRepository { get; }
    IParentRepository ParentRepository { get; }
    IScheduleRepository ScheduleRepository { get; }
    ISubjectRepository SubjectRepository { get; }
    
    Task<int> SaveChangesAsync();
}