using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Address;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/address")]
public class AddressController(IAddressService addressService) : ControllerBase
{
    [HttpGet("provinces")]
    public async Task<ActionResult<IEnumerable<ProvinceDto>>> GetAllProvinces()
    {
        var result = await addressService.GetAllNewProvincesAsync();
        return Ok(result);
    }

    [HttpGet("provinces/{provinceId}/wards")]
    public async Task<ActionResult<IEnumerable<WardDto>>> GetProvinceWithWards([FromRoute] string provinceId)
    {
        var result =  await addressService.GetProvinceWithWardsAsync(provinceId);
        return Ok(result.Wards);
    }
}