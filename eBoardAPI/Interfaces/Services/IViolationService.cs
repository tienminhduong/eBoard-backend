using eBoardAPI.Common;
using eBoardAPI.Models.Violation;

namespace eBoardAPI.Interfaces.Services
{
    public interface IViolationService
    {
        Task<Result<IEnumerable<ViolationDto>>> CreateNewViolation(CreateViolationDto createViolationDto);
        Task<Result<IEnumerable<ViolationDto>>> UpdateViolation(Guid violationId, UpdateViolationDto updateViolationDto);
        Task<Result<IEnumerable<ViolationDto>>> GetViolationsByClassId(Guid classId);
        Task<Result<ViolationsStatsDto>> GetViolationStatsByClassId(Guid classId);
        Task<Result<IEnumerable<ViolationDto>>> GetViolationsByClassIdAndStudentId(Guid classId, Guid studentId);
    }
}
