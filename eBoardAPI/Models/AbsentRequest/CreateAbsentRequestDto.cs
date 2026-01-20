using eBoardAPI.Consts;

namespace eBoardAPI.Models.AbsentRequest;

public class CreateAbsentRequestDto
{
    public Guid StudentId { get; set; }
    public Guid ClassId { get; set; }
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}