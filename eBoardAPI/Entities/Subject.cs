namespace eBoardAPI.Entities;

public class Subject
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
}