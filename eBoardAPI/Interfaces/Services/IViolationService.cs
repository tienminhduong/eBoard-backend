using eBoardAPI.Common;
using eBoardAPI.Models.Violation;

namespace eBoardAPI.Interfaces.Services
{
    public interface IViolationService
    {
        Task<Result> CreateNewViolation(CreateViolationDto createViolationDto);
        Task<Result> UpdateViolation(Guid violationId, UpdateViolationDto updateViolationDto);
        Task<Result<IEnumerable<ViolationDto>>> GetViolationsByClassId(Guid classId);
        Task<Result<ViolationsStatsDto>> GetViolationStatsByClassId(Guid classId);
        Task<Result<IEnumerable<ViolationDto>>> GetViolationsByClassIdAndStudentId(Guid classId, Guid studentId);
        Task<Result<ViolationDto>> GetViolationById(Guid violationId);
    }
}
