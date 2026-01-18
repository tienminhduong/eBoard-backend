using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories;

public class ClassFundRepository(AppDbContext dbContext) : IClassFundRepository
{
    public async Task<ClassFund> AddNewClassFundAsync(ClassFund classFund)
    {
        await dbContext.ClassFunds.AddAsync(classFund);
        return classFund;
    }

    public async Task<Result<ClassFund>> GetClassFundByIdAsync(Guid classFundId)
    {
        var classFund = await dbContext.ClassFunds
            .Include(cf => cf.Class)
            .FirstOrDefaultAsync(cf => cf.Id == classFundId);
        
        return classFund != null
            ? Result<ClassFund>.Success(classFund)
            : Result<ClassFund>.Failure("Không tìm thấy quỹ lớp");
    }

    public async Task<Result<ClassFund>> GetClassFundByClassIdAsync(Guid classId)
    {
        var classFund = await dbContext.ClassFunds
            .Include(cf => cf.Class)
            .FirstOrDefaultAsync(cf => cf.ClassId == classId);
        
        return classFund != null
            ? Result<ClassFund>.Success(classFund)
            : Result<ClassFund>.Failure("Không tìm thấy quỹ lớp cho lớp này");
    }

    public async Task<Result<ClassFund>> UpdateAsync(ClassFund classFund)
    {
        try
        {
            dbContext.ClassFunds.Update(classFund);
            return Result<ClassFund>.Success(classFund);
        }
        catch (Exception ex)
        {
            return Result<ClassFund>.Failure($"Cập nhật quỹ lớp thất bại: {ex.Message}");
        }
    }
}