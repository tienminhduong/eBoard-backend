using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models;
using eBoardAPI.Models.Auth;
using eBoardAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("parent/login")]
    public async Task<ActionResult<LoginResponseDto>> ParentLogin([FromBody] ParentLoginDto parentLoginDto)
    {
        var result = await authService.LoginAsync(parentLoginDto);
        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }
        return Ok(result.Value);
    }

    [HttpPost("teacher/login")]
    public async Task<ActionResult<LoginResponseDto>> TeacherLogin([FromBody] TeacherLoginDto teacherLoginDto)
    {
        var result = await authService.LoginAsync(teacherLoginDto);
        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }
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

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        var result = await authService.ForgotPasswordAsync(dto);
        if (!result.IsSuccess) {
            return BadRequest(new { Message = result.ErrorMessage });
        }
        return Ok("N?u email t?n t?i, link reset ?ã ???c g?i");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        var res = await authService.ResetPasswordAsync(dto);
        if (!res.IsSuccess)
        {
            return BadRequest(new { Message = res.ErrorMessage });
        }
        return Ok("??i m?t kh?u thành công");
    }
}