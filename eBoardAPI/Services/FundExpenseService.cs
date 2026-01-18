using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.FundExpense;

namespace eBoardAPI.Services
{
    public class FundExpenseService(IFundExpenseRepository fundExpenseRepository,
                                    IUnitOfWork unitOfWork,
                                    IMapper mapper) : IFundExpenseService
    {
        public async Task<Result<FundExpenseDto?>> CreateNewFundExpenseAsync(Guid classId, FundExpenseCreateDto fundExpenseCreateDto)
        {
            var errorMessage = fundExpenseCreateDto.ValidateData();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return Result<FundExpenseDto?>.Failure(errorMessage);
            }
            try
            {
                // Start a transaction
                // get class fund by classId
                var classFundResult = await unitOfWork.ClassFundRepository.GetClassFundByClassIdAsync(classId);
                if(!classFundResult.IsSuccess || classFundResult.Value == null)
                {
                    return Result<FundExpenseDto?>.Failure("Class fund not found for the given class ID.");
                }
                var classFundEntity = classFundResult.Value;
                // check if current balance is sufficient
                if(classFundEntity.CurrentBalance < fundExpenseCreateDto.Amount)
                {
                    return Result<FundExpenseDto?>.Failure("The balance is not enough");
                }
                // deduct amount from current balance
                classFundEntity.CurrentBalance -= fundExpenseCreateDto.Amount;
                // update class fund
                var updateResult = await unitOfWork.ClassFundRepository.UpdateAsync(classFundEntity);
                if (!updateResult.IsSuccess)
                {
                    unitOfWork.Dispose();
                    return Result<FundExpenseDto?>.Failure("Them that bai");
                }
                // add classfundentity
                var fundExpenseEntity = mapper.Map<FundExpense>(fundExpenseCreateDto);
                fundExpenseEntity.ClassFundId = classFundEntity.Id;
                var result = await unitOfWork.FundExpenseRepository.AddnNewAsync(fundExpenseEntity);
                if (!result.IsSuccess)
                {
                    unitOfWork.Dispose();
                    return Result<FundExpenseDto?>.Failure("Add new fund expense failed");
                }
                await unitOfWork.SaveChangesAsync();
                var dto = mapper.Map<FundExpenseDto>(result.Value);
                return Result<FundExpenseDto?>.Success(dto);
            }
            catch (Exception ex)
            {
                unitOfWork.Dispose();
                return Result<FundExpenseDto?>.Failure($"An error occurred while creating the fund expense: {ex.Message}");
            }
            
        }

        public async Task<Result<IEnumerable<FundExpenseDto>>> GetFundExpensesByClassId(Guid classId, int pageNumber, int pageSize, DateOnly? startDate, DateOnly? endDate)
        {
            var result = await fundExpenseRepository.GetAllByClassIdAsync(classId, pageNumber, pageSize, startDate, endDate);
            if(!result.IsSuccess)
            {
                return Result<IEnumerable<FundExpenseDto>>.Failure("Failed to retrieve fund expenses.");
            }
            var dtos = mapper.Map<IEnumerable<FundExpenseDto>>(result.Value);
            return Result<IEnumerable<FundExpenseDto>>.Success(dtos);
        }
    }
}
