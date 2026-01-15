using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface IClassFundRepository
{
    Task<ClassFund> AddNewClassFundAsync(ClassFund classFund);
    Task<Result<ClassFund>> GetClassFundByIdAsync(Guid classFundId);
    Task<Result<ClassFund>> GetClassFundByClassIdAsync(Guid classId);
}