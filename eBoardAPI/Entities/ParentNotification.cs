namespace eBoardAPI.Entities;

public class ParentNotification
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid ParentId { get; set; }
    public Parent Parent { get; set; } = null!;
    public string Title { get; set; } = string.Empty; 
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;
}