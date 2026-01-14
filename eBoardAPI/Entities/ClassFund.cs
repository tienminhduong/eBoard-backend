namespace eBoardAPI.Entities;

public class ClassFund
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public int CurrentBalance { get; set; }
}