using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Models.ClassFund;

namespace eBoardAPI.Interfaces.Services;

public interface IClassFundService
{
    Task<Result<ClassFundDto>> GetClassFundByIdAsync(Guid classFundId);
    Task<Result<ClassFundDto>> GetClassFundByClassIdAsync(Guid classId);
}