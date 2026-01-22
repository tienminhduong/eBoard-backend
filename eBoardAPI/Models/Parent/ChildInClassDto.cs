using eBoardAPI.Models.Class;
using eBoardAPI.Models.Student;

namespace eBoardAPI.Models.Parent;

public class ChildInClassDto
{
    public StudentInfoDto StudentInfo { get; set; } = null!;
    public ClassInfoDto ClassInfo { get; set; } = null!;
}