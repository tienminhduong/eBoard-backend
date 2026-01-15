using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface IClassFundRepository
{
    Task<Result<ClassFund>> AddNewClassFundAsync(ClassFund classFund);
    Task<Result<ClassFund>> GetClassFundByIdAsync(Guid classFundId);
    Task<Result<ClassFund>> GetClassFundByClassIdAsync(Guid classId);
}