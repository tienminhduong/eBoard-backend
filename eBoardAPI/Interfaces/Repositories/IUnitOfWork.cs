namespace eBoardAPI.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IClassFundRepository ClassFundRepository { get; }
    IStudentRepository StudentRepository { get; }
    IClassRepository ClassRepository { get; }
    IParentRepository ParentRepository { get; }
    
    Task<int> SaveChangesAsync();
}