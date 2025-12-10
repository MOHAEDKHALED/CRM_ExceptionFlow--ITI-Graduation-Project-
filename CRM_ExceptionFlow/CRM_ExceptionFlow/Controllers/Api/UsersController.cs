using AutoMapper;
using CRM_ExceptionFlow.Data;
using CRM_ExceptionFlow.DTOs.Users;
using CRM_ExceptionFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCryptNet = BCrypt.Net.BCrypt;

namespace CRM_ExceptionFlow.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UsersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _context.Users.AsNoTracking().OrderByDescending(u => u.CreatedAt).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                IsActive = request.IsActive
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, _mapper.Map<UserDto>(user));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserDto>> Update(int id, UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.FullName = request.FullName;
            user.Role = request.Role;
            user.Department = request.Department;
            user.IsActive = request.IsActive;

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                user.PasswordHash = BCryptNet.HashPassword(request.Password);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.IsActive = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

