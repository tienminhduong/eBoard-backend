using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models;
using eBoardAPI.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("parent/login")]
    public async Task<ActionResult<LoginResponseDto>> ParentLogin([FromBody] ParentLoginDto parentLoginDto)
    {
        return Ok(new LoginResponseDto { AccessToken = "parent_access_token", RefreshToken = "parent_refresh_token" });
    }

    [HttpPost("teacher/login")]
    public async Task<ActionResult<LoginResponseDto>> TeacherLogin([FromBody] TeacherLoginDto teacherLoginDto)
    {
        var result = await authService.LoginAsync(teacherLoginDto);
        return Ok(result);
    }

    [HttpPost("teacher/register")]
    public async Task<ActionResult> RegisterTeacher([FromBody] RegisterTeacherDto registerTeacherDto)
    {
        var result = await authService.RegisterTeacherAsync(registerTeacherDto);
        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }
        return Created();
    }
}