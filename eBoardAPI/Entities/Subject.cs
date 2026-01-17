namespace eBoardAPI.Entities;

public class Subject
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    
    public Guid? ClassId { get; set; }
    public Class? Class { get; set; }
}