using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.ClassFund;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Services;

public class ClassFundService(
    IClassFundRepository classFundRepository,
    IMapper mapper
    ) : IClassFundService
{
    public async Task<Result<ClassFundDto>> GetClassFundByIdAsync(Guid classFundId)
    {
        var result = await classFundRepository.GetClassFundByIdAsync(classFundId);
        return result.IsSuccess
            ? Result<ClassFundDto>.Success(mapper.Map<ClassFundDto>(result.Value!))
            : Result<ClassFundDto>.Failure(result.ErrorMessage!);
    }

    public async Task<Result<ClassFundDto>> GetClassFundByClassIdAsync(Guid classId)
    {
        var result = await classFundRepository.GetClassFundByClassIdAsync(classId);
        return result.IsSuccess
            ? Result<ClassFundDto>.Success(mapper.Map<ClassFundDto>(result.Value!))
            : Result<ClassFundDto>.Failure(result.ErrorMessage!);
    }
}