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
                                  IMapper mapper) : IViolationService
    {
        public async Task<Result<IEnumerable<ViolationDto>>> CreateNewViolation(CreateViolationDto createViolationDto)
        {
            // Create Violation entities for each student
            // Cannot use AutoMapper here due to the list of StudentIds
            var violationEntities = new List<Violation>();
            foreach (var studentId in createViolationDto.StudentIds)
            {
                var violationEntity = new Violation
                {
                    StudentId = studentId,
                    ClassId = createViolationDto.ClassId,
                    InChargeTeacherName = createViolationDto.InChargeTeacherName,
                    ViolateDate = createViolationDto.ViolateDate,
                    ViolationType = createViolationDto.ViolationType,
                    ViolationLevel = createViolationDto.ViolationLevel,
                    ViolationInfo = createViolationDto.ViolationInfo,
                    Penalty = createViolationDto.Penalty,
                };
                violationEntities.Add(violationEntity);
            }
            var result = await violationRepository.AddRangeAsync(violationEntities);
            if (!result.IsSuccess)
            {
                return Result<IEnumerable<ViolationDto>>.Failure(result.ErrorMessage ?? "Failed to create violations.");
            }
            var violationDtos = mapper.Map<IEnumerable<ViolationDto>>(result.Value);
            return Result<IEnumerable<ViolationDto>>.Success(violationDtos);
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
