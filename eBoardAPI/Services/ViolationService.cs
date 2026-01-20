using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Violation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace eBoardAPI.Services
{
    public class ViolationService(IViolationRepository violationRepository,
                                  IUnitOfWork unitOfWork,
                                  IMapper mapper) : IViolationService
    {
        public async Task<Result> CreateNewViolation(CreateViolationDto createViolationDto)
        {
            try
            {
                // Create Violation entities and ViolationStudent for each student
                // Cannot use AutoMapper here due to the list of StudentIds
                var violationEntity = new Violation
                {
                    ClassId = createViolationDto.ClassId,
                    InChargeTeacherName = createViolationDto.InChargeTeacherName,
                    ViolateDate = createViolationDto.ViolateDate,
                    ViolationType = createViolationDto.ViolationType,
                    ViolationLevel = createViolationDto.ViolationLevel,
                    ViolationInfo = createViolationDto.ViolationInfo,
                    Penalty = createViolationDto.Penalty,
                    SeenByParent = false,
                };
                var result = await violationRepository.AddAsync(violationEntity);
                if(!result.IsSuccess)
                {
                    unitOfWork.Dispose();
                    return Result.Failure(result.ErrorMessage ?? "Failed to create violation.");
                }
                var violationEntityCreated = result.Value;
                var violationStudentEntities = createViolationDto.StudentIds.Select(studentId => new ViolationStudent
                {
                    StudentId = studentId,
                    ViolationId = violationEntityCreated!.Id
                }).ToList();
                var addViolationStudentResult = await violationRepository.AddRangeViolationStudentsAsync(violationStudentEntities);
                if(!addViolationStudentResult.IsSuccess)
                {
                    unitOfWork.Dispose();
                    return Result.Failure(addViolationStudentResult.ErrorMessage ?? "Failed to create violation-student associations.");
                }
                await unitOfWork.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                unitOfWork.Dispose();
                return Result.Failure($"An error occurred while creating violations: {ex.Message}");
            }
        }

        public Task<Result<IEnumerable<ViolationDto>>> GetViolationsByClassId(Guid classId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<ViolationDto>>> GetViolationsByClassIdAndStudentId(Guid classId, Guid studentId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ViolationsStatsDto>> GetViolationStatsByClassId(Guid classId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IEnumerable<ViolationDto>>> UpdateViolation(Guid violationId, UpdateViolationDto updateViolationDto)
        {
            return Result<IEnumerable<ViolationDto>>.Failure("Not implemented yet");
            var violationEntityResult = await violationRepository.GetRangeByIdsAsync(updateViolationDto.StudentId);
            if(!violationEntityResult.IsSuccess)
            {
                return Result<IEnumerable<ViolationDto>>.Failure(violationEntityResult.ErrorMessage ?? "Failed to retrieve violations.");
            }
            var violationEntities = violationEntityResult.Value;
            if(violationEntities == null || !violationEntities.Any())
            {
                return Result<IEnumerable<ViolationDto>>.Failure("Không tìm thấy học sinh");
            }
            
        }
    }
}
