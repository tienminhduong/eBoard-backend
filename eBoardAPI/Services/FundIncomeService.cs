using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.FundIncome;

namespace eBoardAPI.Services
{
    public class FundIncomeService(IFundIncomeRepository fundIncomeRepository,
                                   IClassFundRepository classFundRepository,
                                   IClassRepository classRepository,
                                   IMapper mapper) : IFundIncomeService
    {
        public async Task<Result<FundIncomeDto>> CreateFundIncomeAsync(Guid classId, CreateFundIncomeDto fundIncome)
        {
            var fundIncomeEntity = mapper.Map<FundIncome>(fundIncome);
            if(fundIncomeEntity == null)
            {
                return Result<FundIncomeDto>.Failure("Mapping failed.");
            }

            // tim entity class fund
            var classFundResult = await classFundRepository.GetClassFundByClassIdAsync(classId);
            if (!classFundResult.IsSuccess)
            {
                return Result<FundIncomeDto>.Failure("Class Fund not found.");
            }
            fundIncomeEntity.ClassFundId = classFundResult.Value!.Id;

            // tim entity class de lay so luong hoc sinh
            var classResult = await classRepository.GetClassByIdAsync(classId);
            if(!classResult.IsSuccess || classResult.Value == null)
            {
                return Result<FundIncomeDto>.Failure("Class not found.");
            }
            var numberOfStudents = classResult.Value.CurrentStudentCount;

            // tinh toan so tien thu duoc
            fundIncomeEntity.CalculateExpectedAmount(numberOfStudents);

            var addResult = await fundIncomeRepository.AddAsync(fundIncomeEntity);
            return (addResult.IsSuccess)
                ? Result<FundIncomeDto>.Success(mapper.Map<FundIncomeDto>(addResult.Value))
                : Result<FundIncomeDto>.Failure(addResult.ErrorMessage ?? "Failed to create Fund Income.");
        }

        public Task<Result<FundIncomeDto>> GetFundIncomeByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IEnumerable<FundIncomeDto>>> GetFundIncomesByClassIdAsync(Guid classId, int pageNumber, int pageSize)
        {
            var fundIncomesResult = await fundIncomeRepository.GetAllByClassIdAsync(classId, pageNumber, pageSize);

            if(!fundIncomesResult.IsSuccess)
            {
                return Result<IEnumerable<FundIncomeDto>>.Failure(fundIncomesResult.ErrorMessage ?? "Failed to retrieve Fund Incomes.");
            }    

            var fundIncomeDtos = mapper.Map<IEnumerable<FundIncomeDto>>(fundIncomesResult.Value);
            return Result<IEnumerable<FundIncomeDto>>.Success(fundIncomeDtos);
        }
    }
}
