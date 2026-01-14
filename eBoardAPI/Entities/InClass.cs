using System.ComponentModel.DataAnnotations;

namespace eBoardAPI.Entities;

public class InClass
{
    [Key] public Guid StudentId { get; set; }
    [Key] public Guid ClassId { get; set; }

    public Student Student { get; set; } = null!;
    public Class Class { get; set; } = null!;
}