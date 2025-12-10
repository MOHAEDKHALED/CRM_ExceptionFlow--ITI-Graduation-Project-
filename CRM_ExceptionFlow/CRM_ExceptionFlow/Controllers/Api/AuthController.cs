using AutoMapper;
using BCryptNet = BCrypt.Net.BCrypt;
using CRM_ExceptionFlow.Data;
using CRM_ExceptionFlow.DTOs.Auth;
using CRM_ExceptionFlow.DTOs.Users;
using CRM_ExceptionFlow.Models;
using CRM_ExceptionFlow.Options;
using CRM_ExceptionFlow.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CRM_ExceptionFlow.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtTokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly JwtOptions _jwtOptions;

        public AuthController(
            ApplicationDbContext context,
            IJwtTokenService tokenService,
            IMapper mapper,
            IOptions<JwtOptions> jwtOptions)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            if (!BCryptNet.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid username or password.");
            }

            user.LastLogin = DateTime.UtcNow;
            user.IsActive = true;
            await _context.SaveChangesAsync();

            var token = _tokenService.GenerateToken(user);
            var response = new LoginResponse
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes),
                User = _mapper.Map<UserDto>(user)
            };

            return Ok(response);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.Role == "Admin")
            {
                return BadRequest("Admin role can only be created by system administrators.");
            }

            var validRoles = new[] { "Manager", "Employee", "ITSupport" };
            if (!validRoles.Contains(request.Role))
            {
                return BadRequest($"Invalid role. Allowed roles: {string.Join(", ", validRoles)}");
            }

            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return Conflict("Username already exists.");
            }

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return Conflict("Email already exists.");
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                FullName = request.FullName,
                Role = request.Role,
                Department = request.Department,
                PasswordHash = BCryptNet.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
                IsActive = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProfile), new { }, _mapper.Map<UserDto>(user));
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetProfile()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var id = int.Parse(userId);
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            return Ok(_mapper.Map<UserDto>(user));
        }
    }
}

