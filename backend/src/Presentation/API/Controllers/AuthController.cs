using CRM.Application.DTOs.Auth;
using CRM.Application.UseCases.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginUseCase _loginUseCase;
    private readonly RegisterUseCase _registerUseCase;
    
    public AuthController(
        LoginUseCase loginUseCase,
        RegisterUseCase registerUseCase)
    {
        _loginUseCase = loginUseCase;
        _registerUseCase = registerUseCase;
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var response = await _loginUseCase.ExecuteAsync(request);
        
        if (response == null)
            return Unauthorized(new { message = "Invalid username or password" });
        
        return Ok(response);
    }
    
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (request.Role == "Admin")
            return BadRequest(new { message = "Admin role can only be created by system administrators" });
        
        var validRoles = new[] { "Manager", "Employee", "ITSupport" };
        if (!validRoles.Contains(request.Role))
            return BadRequest(new { message = $"Invalid role. Allowed roles: {string.Join(", ", validRoles)}" });
        
        try
        {
            var response = await _registerUseCase.ExecuteAsync(request);
            return CreatedAtAction(nameof(Login), new { }, response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
    
    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<UserInfo>> GetProfile()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();
        
        // This would typically use a GetUserUseCase
        return Ok(new { message = "Profile endpoint - implement GetUserUseCase" });
    }
}


