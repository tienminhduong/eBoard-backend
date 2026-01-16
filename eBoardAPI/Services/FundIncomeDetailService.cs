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
    }
}
