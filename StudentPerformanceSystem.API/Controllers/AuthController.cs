using Microsoft.AspNetCore.Mvc;
using StudentPerformanceSystem.API.Models;
using StudentPerformanceSystem.Services;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(AuthService authService, JwtTokenService jwtTokenService)
    {
        _authService = authService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var success = await _authService.RegisterUserAsync(model.Email, model.Password, model.Role, model.TeacherID, model.SchoolID);
        if (!success)
        {
            return BadRequest("User already exists.");
        }
        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _authService.AuthenticateUserAsync(model.Email, model.Password);
        if (user == null)
        {
            return Unauthorized("Invalid credentials.");
        }

        var token = _jwtTokenService.GenerateToken(user);
        return Ok(new { Token = token });
    }
}