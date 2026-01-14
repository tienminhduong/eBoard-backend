using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Entities;

[PrimaryKey(nameof(StudentId), nameof(ClassId))]
public class InClass
{
    public Guid StudentId { get; set; }
    public Guid ClassId { get; set; }

    public Student Student { get; set; } = null!;
    public Class Class { get; set; } = null!;
}