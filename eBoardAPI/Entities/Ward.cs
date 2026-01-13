namespace eBoardAPI.Entities;

public class Ward
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public Guid? DistrictId { get; set; }
    public District? District { get; set; }
}