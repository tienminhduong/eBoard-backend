namespace eBoardAPI.Models.Address;

public class ProvinceWithWardsDto : ProvinceDto
{
    public IEnumerable<WardDto> Wards { get; set; } = [];
}