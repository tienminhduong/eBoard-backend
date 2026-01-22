using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Violation;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.WebSockets;

namespace eBoardAPI.Services
{
    public class ViolationService(IViolationRepository violationRepository,
                                  IUnitOfWork unitOfWork) : IViolationService
    {
        public async Task<Result> ConfirmViolation(Guid violationId, Guid studentId)
        {
            var result = await violationRepository.GetViolationStudentByIds(violationId, studentId);
            if(!result.IsSuccess)
            {
                return Result.Failure(result.ErrorMessage ?? "Failed to retrieve violation.");
            }
            var violationStudentEntity = result.Value;
            if(violationStudentEntity == null)
            {
                return Result.Failure("Không tìm thấy vi phạm");
            }

            violationStudentEntity.SeenByParent = true;
            var updateResult = await violationRepository.UpdateAsync(violationStudentEntity);

            if(!updateResult.IsSuccess)
            {
                return Result.Failure(updateResult.ErrorMessage ?? "Failed to update violation.");
            }
            return Result.Success();
        }

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
                    Penalty = createViolationDto.Penalty
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

        public async Task<Result<SummaryViolation>> GetSummaryViolation(Guid classId, Guid studentId)
        {
            var result = await violationRepository.GetSummaryViolationAsync(classId, studentId);
            if(!result.IsSuccess)
            {
                return Result<SummaryViolation>.Failure(result.ErrorMessage ?? "Failed to retrieve summary violation.");
            }
            return Result<SummaryViolation>.Success(result.Value!);
        }

        public async Task<Result<ViolationDto>> GetViolationById(Guid violationId)
        {
            var result = await violationRepository.GetByIdAsync(violationId);
            if(!result.IsSuccess)
            {
                return Result<ViolationDto>.Failure(result.ErrorMessage ?? "Failed to retrieve violation.");
            }
            var violationEntity = result.Value;
            if(violationEntity == null)
            {
                return Result<ViolationDto>.Failure("Không tìm thấy vi phạm");
            }
            // Map to DTO
            var violationDto = new ViolationDto
            {
                Id = violationEntity.Id,
                ClassId = violationEntity.ClassId,
                InChargeTeacherName = violationEntity.InChargeTeacherName,
                ViolateDate = violationEntity.ViolateDate,
                ViolationType = violationEntity.ViolationType,
                ViolationLevel = violationEntity.ViolationLevel,
                ViolationInfo = violationEntity.ViolationInfo,
                Penalty = violationEntity.Penalty,
                InvolvedStudents = violationEntity.Students.Select(s => new IdStudentPair
                {
                    StudentId = s.Student.Id,
                    StudentName = s.Student.FirstName + " " + s.Student.LastName
                }).ToList()
            };
            return Result<ViolationDto>.Success(violationDto);
        }

        public async Task<Result<IEnumerable<ViolationDto>>> GetViolationsByClassId(Guid classId, int pageNumber = 1, int pageSize = 20)
        {
            var result = await violationRepository.GetViolationsByClassIdAsync(classId, pageNumber, pageSize);
            if(!result.IsSuccess)
                {
                return Result<IEnumerable<ViolationDto>>.Failure(result.ErrorMessage ?? "Failed to retrieve violations.");
            }
            var dtos = result.Value!.Select(violationEntity => new ViolationDto
            {
                Id = violationEntity.Id,
                ClassId = violationEntity.ClassId,
                InChargeTeacherName = violationEntity.InChargeTeacherName,
                ViolateDate = violationEntity.ViolateDate,
                ViolationType = violationEntity.ViolationType,
                ViolationLevel = violationEntity.ViolationLevel,
                ViolationInfo = violationEntity.ViolationInfo,
                Penalty = violationEntity.Penalty,
                InvolvedStudents = violationEntity.Students.Select(s => new IdStudentPair
                {
                    StudentId = s.Student.Id,
                    StudentName = s.Student.FirstName + " " + s.Student.LastName
                }).ToList()
            });
            return Result<IEnumerable<ViolationDto>>.Success(dtos);
        }

        public async Task<Result<IEnumerable<ViolationForStudentDto>>> GetViolationsByClassIdAndStudentId(Guid classId, Guid studentId, int pageNumber = 1, int pageSize = 20)
        {
            var result = await violationRepository.GetViolationsByClassIdAndStudentId(classId, studentId, pageNumber, pageSize);
            if(!result.IsSuccess)
            {
                return Result<IEnumerable<ViolationForStudentDto>>.Failure(result.ErrorMessage ?? "Failed to retrieve violations.");
            }
            var violationDtos = new List<ViolationForStudentDto>();
            foreach(var violationEntity in result.Value!)
            {
                var violationDto = new ViolationForStudentDto
                {
                    Id = violationEntity.Violation.Id,
                    InChargeTeacherName = violationEntity.Violation.InChargeTeacherName,
                    ViolateDate = violationEntity.Violation.ViolateDate,
                    ViolationType = violationEntity.Violation.ViolationType,
                    ViolationLevel = violationEntity.Violation.ViolationLevel,
                    ViolationInfo = violationEntity.Violation.ViolationInfo,
                    Penalty = violationEntity.Violation.Penalty,
                    SeenByParent = violationEntity.SeenByParent
                };
                violationDtos.Add(violationDto);
            }
            return Result<IEnumerable<ViolationForStudentDto>>.Success(violationDtos);
        }

        public async Task<Result<ViolationsStatsDto>> GetViolationStatsByClassId(Guid classId, DateOnly? from, DateOnly? to)
        {
            var res = await violationRepository.GetViolationStatByClassId(classId, from, to);
            if(!res.IsSuccess)
                return Result<ViolationsStatsDto>.Failure(res.ErrorMessage ?? "Failed to retrieve violation stats.");
            return Result<ViolationsStatsDto>.Success(res.Value!);
        }

        public async Task<Result> UpdateViolation(Guid violationId, UpdateViolationDto updateViolationDto)
        {
            try
            {
                var violationResult = await violationRepository.GetByIdAsync(violationId);
                if (!violationResult.IsSuccess)
                {
                    unitOfWork.Dispose();
                    return Result.Failure(violationResult.ErrorMessage ?? "Failed to retrieve violation.");
                }

                var violationEntity = violationResult.Value;
                if (violationEntity == null)
                {
                    unitOfWork.Dispose();
                    return Result.Failure("Violation not found.");
                }

                var violationStudentsResult = await violationRepository.GetViolationStudentByViolationIdAsync(violationId);
                if (!violationStudentsResult.IsSuccess)
                {
                    unitOfWork.Dispose();
                    return Result.Failure(violationStudentsResult.ErrorMessage ?? "Failed to retrieve violation-student associations.");
                }

                var studentIdsToUpdate = updateViolationDto.StudentIds;
                var studentIdsExisting = violationStudentsResult.Value?.Select(vs => vs.StudentId).ToList();

                var violationStudentsToRemove = new List<Guid>();
                var violationStudentsToAdd = new List<Guid>();
                // Determine which students to remove
                foreach (var vs in studentIdsExisting)
                {
                    var isExist = studentIdsToUpdate.Contains(vs);
                    if (!isExist)
                    {
                        violationStudentsToRemove.Add(vs);
                    }
                }
                // Determine which students to add
                foreach (var su in studentIdsToUpdate)
                {
                    var isExist = studentIdsExisting!.Contains(su);
                    if (!isExist)
                    {
                        violationStudentsToAdd.Add(su);
                    }
                }
                // Remove old ViolationStudent associations
                var removeViolationStudents = violationStudentsToRemove.Select(studentId => new ViolationStudent
                {
                    StudentId = studentId,
                    ViolationId = violationId
                }).ToList();
                var addViolationStudents = violationStudentsToAdd.Select(studentId => new ViolationStudent
                {
                    StudentId = studentId,
                    ViolationId = violationId
                }).ToList();
                var removeResult = await violationRepository.RemoveRangeViolationStudentsAsync(removeViolationStudents);
                if (!removeResult.IsSuccess)
                {
                    unitOfWork.Dispose();
                    return Result.Failure(removeResult.ErrorMessage ?? "Failed to remove old violation-student associations.");
                }
                var addResult = await violationRepository.AddRangeViolationStudentsAsync(addViolationStudents);
                if (!addResult.IsSuccess)
                {
                    unitOfWork.Dispose();
                    return Result.Failure(addResult.ErrorMessage ?? "Failed to add new violation-student associations.");
                }
                // Update Violation entity
                violationEntity.InChargeTeacherName = updateViolationDto.InChargeTeacherName;
                violationEntity.ViolateDate = updateViolationDto.ViolateDate;
                violationEntity.ViolationType = updateViolationDto.ViolationType;
                violationEntity.ViolationLevel = updateViolationDto.ViolationLevel;
                violationEntity.ViolationInfo = updateViolationDto.ViolationInfo;
                violationEntity.Penalty = updateViolationDto.Penalty;
                var updateResult = await violationRepository.UpdateAsync(violationEntity);
                await unitOfWork.SaveChangesAsync();
                if (!updateResult.IsSuccess)
                {
                    unitOfWork.Dispose();
                    return Result.Failure(updateResult.ErrorMessage ?? "Failed to update violation.");
                }
                return Result.Success();
            }
            catch (Exception ex)
            {
                unitOfWork.Dispose();
                return Result.Failure($"An error occurred while updating violation: {ex.Message}");
            }
        }
    }
}
