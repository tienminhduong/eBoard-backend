using eBoardAPI.Common;
using eBoardAPI.Models.Violation;

namespace eBoardAPI.Interfaces.Services
{
    public interface IViolationService
    {
        Task<Result> CreateNewViolation(CreateViolationDto createViolationDto);
        Task<Result> UpdateViolation(Guid violationId, UpdateViolationDto updateViolationDto);
        Task<Result<IEnumerable<ViolationDto>>> GetViolationsByClassId(Guid classId, int pageNumber = 1, int pageSize = 20);
        Task<Result<ViolationsStatsDto>> GetViolationStatsByClassId(Guid classId);
        Task<Result<IEnumerable<ViolationForStudentDto>>> GetViolationsByClassIdAndStudentId(Guid classId, Guid studentId, int pageNumber, int pageSize);
        Task<Result<ViolationDto>> GetViolationById(Guid violationId);
        Task<Result> ConfirmViolation(Guid violationId, Guid studentId);
        Task<Result<SummaryViolation>> GetSummaryViolation(Guid classId, Guid studentId);
    }
}
