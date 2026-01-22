using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Extensions;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.FundIncome;

namespace eBoardAPI.Services
{
    public class FundIncomeService(IFundIncomeRepository fundIncomeRepository,
                                   IClassFundRepository classFundRepository,
                                   IClassRepository classRepository,
                                   IStudentRepository studentRepository,
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

        public async Task<Result<FundIncomeDto>> GetFundIncomeByIdAsync(Guid id)
        {
            var result = await fundIncomeRepository.GetByIdAsync(id);
            if(!result.IsSuccess)
            {
                return Result<FundIncomeDto>.Failure(result.ErrorMessage!);
            }
            var fundIncomeDto = mapper.Map<FundIncomeDto>(result.Value);
            return Result<FundIncomeDto>.Success(fundIncomeDto);
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

        public async Task<Result<IEnumerable<FundIncomeDetailDto>>> GetFundIncomeDetailsByStudentIdAsync(Guid studentId)
        {
            var fundIncomeDetailsResult = await fundIncomeRepository.GetFundIncomeDetailsByStudentIdAsync(studentId);
            if(!fundIncomeDetailsResult.IsSuccess)
            {
                return Result<IEnumerable<FundIncomeDetailDto>>.Failure(fundIncomeDetailsResult.ErrorMessage ?? "Failed to retrieve Fund Income Details.");
            }

            var fundIncomeDetailDtos = mapper.Map<IEnumerable<FundIncomeDetailDto>>(fundIncomeDetailsResult.Value);
            return Result<IEnumerable<FundIncomeDetailDto>>.Success(fundIncomeDetailDtos);
        }

        public async Task<Result<FundIncomeDto>> UpdateFundIncomeAsync(Guid id, UpdateFundIncomeDto updatedFundIncome)
        {
            try
            {


                var existingFundIncomeResult = await fundIncomeRepository.GetByIdAsync(id);
                if (!existingFundIncomeResult.IsSuccess)
                {
                    return Result<FundIncomeDto>.Failure(existingFundIncomeResult.ErrorMessage!);
                }
                if (existingFundIncomeResult.Value == null)
                {
                    return Result<FundIncomeDto>.Failure("Không tồn tại quỹ thu này");
                }
                var existingFundIncome = existingFundIncomeResult.Value;
                mapper.Map(updatedFundIncome, existingFundIncome);

                // update expected amount
                var classFundResult = await classFundRepository.GetClassFundByIdAsync(existingFundIncome.ClassFundId);
                if (!classFundResult.IsSuccess || classFundResult.Value == null)
                {
                    return Result<FundIncomeDto>.Failure("Class Fund not found.");
                }

                var countStudentResult = await studentRepository.CountStudentByClassIdAsync(classFundResult.Value.ClassId);
                existingFundIncome.CalculateExpectedAmount(countStudentResult.Value);
                var updateResult = await fundIncomeRepository.UpdateAsync(existingFundIncome);

                if (!updateResult.IsSuccess)
                {
                    return Result<FundIncomeDto>.Failure(updateResult.ErrorMessage!);
                }
                var fundIncomeDto = mapper.Map<FundIncomeDto>(existingFundIncome);
                return Result<FundIncomeDto>.Success(fundIncomeDto);
            } 
            catch (Exception ex)
            {
                return Result<FundIncomeDto>.Failure($"An error occurred while updating Fund Income: {ex.Message}");
            }
        }
    }
}
