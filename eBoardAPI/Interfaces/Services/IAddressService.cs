using eBoardAPI.Models.Address;
using Refit;

namespace eBoardAPI.Interfaces.Services;

public interface IAddressService
{
    [Get("/v2/p/")]
    Task<IEnumerable<ProvinceDto>> GetAllNewProvincesAsync();
    
    [Get("/v2/p/{provinceId}?depth=2")]
    Task<ProvinceWithWardsDto> GetProvinceWithWardsAsync(string provinceId);
}