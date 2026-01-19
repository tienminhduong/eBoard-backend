using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.FundIncome;
using System.Net.WebSockets;
using eBoardAPI.Extensions;

namespace eBoardAPI.Services
{
    public class FundIncomeDetailService(IFundIncomeDetailRepository fundIncomeDetailRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IFundIncomeDetailService
    {
        public async Task<Result<FundIncomeDetailDto>> CreateContributeFundIncome(Guid fundIncomeId, ContributeFundIncomeDto contributeFund)
        {
            try
            {
                // get fund income entity
                var fundIncomeResult = await unitOfWork.FundIncomeRepository.GetByIdAsync(fundIncomeId);
                if(fundIncomeResult.IsSuccess == false)
                {
                    return Result<FundIncomeDetailDto>.Failure(fundIncomeResult.ErrorMessage!);
                }
                var fundIncomeEntity = fundIncomeResult.Value;
                if(fundIncomeEntity == null)
                {
                    return Result<FundIncomeDetailDto>.Failure("Fund income not found.");
                }

                // get class fund entity
                var classFundId = fundIncomeEntity.ClassFundId;
                var classFundResult = await unitOfWork.ClassFundRepository.GetClassFundByIdAsync(classFundId);
                if(classFundResult.IsSuccess == false)
                {
                    return Result<FundIncomeDetailDto>.Failure(classFundResult.ErrorMessage!);
                }
                var classFundEntity = classFundResult.Value;
                if(classFundEntity == null)
                {
                    return Result<FundIncomeDetailDto>.Failure("Class fund not found.");
                }
                // update balance
                classFundEntity.CurrentBalance += contributeFund.ContributedAmount;
                classFundEntity.TotalContributions += contributeFund.ContributedAmount;
                var updateClassFundResult = await unitOfWork.ClassFundRepository.UpdateAsync(classFundEntity);
                if (updateClassFundResult.IsSuccess == false)
                {
                    unitOfWork.Dispose();
                    return Result<FundIncomeDetailDto>.Failure("Failed to update class fund balance.");
                }
                // create fund income detail entity
                var fundIncomeDetailEntity = mapper.Map<FundIncomeDetail>(contributeFund);
                fundIncomeDetailEntity.FundIncomeId = fundIncomeId;
                fundIncomeDetailEntity.UpdateStatus(fundIncomeEntity.AmountPerStudent);
                var createResult = await fundIncomeDetailRepository.AddAsync(fundIncomeDetailEntity);
                if(createResult.IsSuccess == false)
                {
                    unitOfWork.Dispose();
                    return Result<FundIncomeDetailDto>.Failure(createResult.ErrorMessage!);
                }
                var saveResult = await unitOfWork.SaveChangesAsync();
                if(saveResult <= 0)
                {
                    unitOfWork.Dispose();
                }    
                var dto = mapper.Map<FundIncomeDetailDto>(createResult.Value);
                return Result<FundIncomeDetailDto>.Success(dto);
            }
            catch (Exception ex)
            {
                unitOfWork.Dispose();
                return Result<FundIncomeDetailDto>.Failure($"An error occurred while creating the fund income detail: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<StudentFundIncomeSummary>>> GetAllFundIncomeDetailsByIdFundIncomeAsync(Guid fundIncomeId)
        {
            var result = await fundIncomeDetailRepository.GetAllFundIncomeDetailsByIdFundIncomeAsync(fundIncomeId);
            if(result.IsSuccess == false)
            {
                return Result<IEnumerable<StudentFundIncomeSummary>>.Failure(result.ErrorMessage!);
            }
            return Result<IEnumerable<StudentFundIncomeSummary>>.Success(result.Value!);
        }

        public async Task<Result<FundIncomeDetailDto?>> GetFundIncomeDetailByIdAsync(Guid incomeDetailId)
        {
            var result = await fundIncomeDetailRepository.GetFundIncomeDetailByIdAsync(incomeDetailId);
            if(result.IsSuccess == false)
            {
                return Result<FundIncomeDetailDto?>.Failure(result.ErrorMessage!);
            }
            var incomeDetail = result.Value;
            var incomeDetailDto = (incomeDetail != null) ? mapper.Map<FundIncomeDetailDto>(incomeDetail) : null;
            return Result<FundIncomeDetailDto?>.Success(incomeDetailDto);
        }

        public async Task<Result<IEnumerable<FundIncomeStudent>>> GetFundIncomeDetailsByClassAndStudentAsync(Guid classId, Guid studentId)
        {
            var result = await fundIncomeDetailRepository.GetFundIncomeDetailsByClassAndStudentAsync(classId, studentId);
            if(result.IsSuccess == false)
            {
                return Result<IEnumerable<FundIncomeStudent>>.Failure(result.ErrorMessage!);
            }
            var incomeDetails = result.Value;
            //var incomeDetailsDto = mapper.Map<IEnumerable<FundIncomeStudent>>(incomeDetails);
            return Result<IEnumerable<FundIncomeStudent>>.Success(incomeDetails!);
        }

        public async Task<Result<IEnumerable<FundIncomeDetailDto>>> GetFundIncomeDetailsByIncomeIdAndStudentIdAsync(Guid incomeId, Guid studentId)
        {
            var result = await fundIncomeDetailRepository.GetFundIncomeDetailsByIncomeIdAndStudentIdAsync(incomeId, studentId);
            if(result.IsSuccess == false)
            {
                return Result<IEnumerable<FundIncomeDetailDto>>.Failure(result.ErrorMessage!);
            }
            var incomeDetails = result.Value;
            var incomeDetailsDto = mapper.Map<IEnumerable<FundIncomeDetailDto>>(incomeDetails);  
            return Result<IEnumerable<FundIncomeDetailDto>>.Success(incomeDetailsDto);
        }
    }
}
