using eBoardAPI.Models;
using eBoardAPI.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("parent/login")]
    public async Task<ActionResult<LoginResponseDto>> ParentLogin([FromBody] ParentLoginDto parentLoginDto)
    {
        return Ok(new LoginResponseDto { AccessToken = "parent_access_token", RefreshToken = "parent_refresh_token" });
    }

    [HttpPost("teacher/login")]
    public async Task<ActionResult<LoginResponseDto>> TeacherLogin([FromBody] TeacherLoginDto teacherLoginDto)
    {
        return Ok(new LoginResponseDto { AccessToken = "teacher_access_token", RefreshToken = "teacher_refresh_token" });
    }
}