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

        public Task<Result> DeleteExamSchedule(Guid id)
        {
            throw new NotImplementedException();
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

        public Task<Result<IEnumerable<ExamScheduleDto>>> GetExamSchedules(Guid classId, ExamScheduleFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateExamSchedule(UpdateExamScheduleDto updateExamScheduleDto)
        {
            throw new NotImplementedException();
        }
    }
}
