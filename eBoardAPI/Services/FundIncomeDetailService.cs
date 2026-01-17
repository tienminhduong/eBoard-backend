using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.FundIncome;
using System.Net.WebSockets;

namespace eBoardAPI.Services
{
    public class FundIncomeDetailService(IFundIncomeDetailRepository fundIncomeDetailRepository,
        IMapper mapper) : IFundIncomeDetailService
    {
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
