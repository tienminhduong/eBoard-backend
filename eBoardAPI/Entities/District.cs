namespace eBoardAPI.Entities;

public class District
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public Guid? ProvinceId { get; set; }
    public Province? Province { get; set; }
}