using AutoMapper;
using CRM_ExceptionFlow.Data;
using CRM_ExceptionFlow.DTOs.Interactions;
using CRM_ExceptionFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM_ExceptionFlow.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public class InteractionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public InteractionsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InteractionDto>>> Get([FromQuery] int? customerId)
        {
            var query = _context.Interactions
                .Include(i => i.User)
                .AsNoTracking()
                .AsQueryable();

            if (customerId.HasValue)
            {
                query = query.Where(i => i.CustomerId == customerId);
            }

            var interactions = await query
                .OrderByDescending(i => i.InteractionDate)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<InteractionDto>>(interactions));
        }

        [HttpPost]
        public async Task<ActionResult<InteractionDto>> Create(InteractionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _context.Customers.AnyAsync(c => c.Id == request.CustomerId))
            {
                return BadRequest("Customer not found.");
            }

            if (!await _context.Users.AnyAsync(u => u.Id == request.UserId))
            {
                return BadRequest("User not found.");
            }

            var interaction = new Interaction
            {
                Type = request.Type,
                Subject = request.Subject,
                Notes = request.Notes,
                InteractionDate = request.InteractionDate,
                CustomerId = request.CustomerId,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Interactions.Add(interaction);
            await _context.SaveChangesAsync();
            await _context.Entry(interaction).Reference(i => i.User).LoadAsync();

            return CreatedAtAction(nameof(Get), null, _mapper.Map<InteractionDto>(interaction));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<InteractionDto>> Update(int id, InteractionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var interaction = await _context.Interactions.FindAsync(id);
            if (interaction == null) return NotFound();

            if (!await _context.Customers.AnyAsync(c => c.Id == request.CustomerId))
            {
                return BadRequest("Customer not found.");
            }

            if (!await _context.Users.AnyAsync(u => u.Id == request.UserId))
            {
                return BadRequest("User not found.");
            }

            interaction.Type = request.Type;
            interaction.Subject = request.Subject;
            interaction.Notes = request.Notes;
            interaction.InteractionDate = request.InteractionDate;
            interaction.CustomerId = request.CustomerId;
            interaction.UserId = request.UserId;

            await _context.SaveChangesAsync();
            await _context.Entry(interaction).Reference(i => i.User).LoadAsync();

            return Ok(_mapper.Map<InteractionDto>(interaction));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var interaction = await _context.Interactions.FindAsync(id);
            if (interaction == null) return NotFound();

            _context.Interactions.Remove(interaction);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

