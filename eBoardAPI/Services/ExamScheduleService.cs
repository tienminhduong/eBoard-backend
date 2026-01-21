using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.ExamSchedule;
using System.Net.WebSockets;

namespace eBoardAPI.Services
{
    public class ExamScheduleService(IExamScheduleRepository examScheduleRepository,
        IMapper mapper) : IExamScheduleService
    {
        public async Task<Result<ExamScheduleDto>> CreateNewExamSchedule(CreateExamScheduleDto createExamScheduleDto)
        { 
            var examScheduleEntity = mapper.Map<ExamSchedule>(createExamScheduleDto);
            var result = await examScheduleRepository.AddAsync(examScheduleEntity);
            if (!result.IsSuccess)
            {
                return Result<ExamScheduleDto>.Failure(result.ErrorMessage ?? "Failed to create exam schedule.");
            }
            var examScheduleDto = mapper.Map<ExamScheduleDto>(result.Value);
            return Result<ExamScheduleDto>.Success(examScheduleDto);
        }

        public async Task<Result> DeleteExamSchedule(Guid id)
        {
            var examScheduleResult = await examScheduleRepository.GetExamScheduleByIdAsync(id);
            if(!examScheduleResult.IsSuccess || examScheduleResult.Value == null)
            {
                return Result.Failure(examScheduleResult.ErrorMessage ?? "Exam schedule not found.");
            }
            var deleteResult = await examScheduleRepository.DeteleAsync(examScheduleResult.Value);
            if(!deleteResult.IsSuccess)
            {
                return Result.Failure(deleteResult.ErrorMessage ?? "Failed to delete exam schedule.");
            }
            return Result.Success();
        }

        public async Task<Result<ExamScheduleDto>> GetExamScheduleById(Guid examScheduleId)
        {
            var result = await examScheduleRepository.GetExamScheduleByIdAsync(examScheduleId);
            if (!result.IsSuccess)
            {
                return Result<ExamScheduleDto>.Failure(result.ErrorMessage ?? "Exam schedule not found.");
            }
            var examScheduleDto = result.Value != null ? mapper.Map<ExamScheduleDto>(result.Value) : null;
            return Result<ExamScheduleDto>.Success(examScheduleDto!);
        }

        public async Task<Result<IEnumerable<ExamScheduleDto>>> GetExamSchedules(Guid classId, ExamScheduleFilter filter)
        {
            var result = await examScheduleRepository.GetExamScheduleByClassIdAndQuery(classId, filter);
            if (!result.IsSuccess)
            {
                return Result<IEnumerable<ExamScheduleDto>>.Failure(result.ErrorMessage ?? "Failed to retrieve exam schedules.");
            }
            var examScheduleDtos = mapper.Map<IEnumerable<ExamScheduleDto>>(result.Value);
            return Result<IEnumerable<ExamScheduleDto>>.Success(examScheduleDtos);
        }

        public async Task<Result> UpdateExamSchedule(Guid examScheduleId, UpdateExamScheduleDto updateExamScheduleDto)
        {
            var existingExamScheduleResult = await examScheduleRepository.GetExamScheduleByIdAsync(examScheduleId);
            if (!existingExamScheduleResult.IsSuccess || existingExamScheduleResult.Value == null)
            {
                return Result.Failure(existingExamScheduleResult.ErrorMessage ?? "Exam schedule not found.");
            }

            var examScheduleEntity = existingExamScheduleResult.Value;
            mapper.Map(updateExamScheduleDto, examScheduleEntity);
            var updateResult = await examScheduleRepository.UpdateAsync(examScheduleEntity);
            if (!updateResult.IsSuccess)
            {
                return Result.Failure(updateResult.ErrorMessage ?? "Failed to update exam schedule.");
            }
            return Result.Success();
        }
    }
}
