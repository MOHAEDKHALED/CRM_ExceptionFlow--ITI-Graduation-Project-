using AutoMapper;
using CRM_ExceptionFlow.Data;
using CRM_ExceptionFlow.DTOs.Deals;
using CRM_ExceptionFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM_ExceptionFlow.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager")]
    public class DealsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DealsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DealDto>>> GetDeals(
            [FromQuery] int? customerId,
            [FromQuery] int? assignedToUserId,
            [FromQuery] string? stage)
        {
            var query = _context.Deals
                .Include(d => d.AssignedToUser)
                .AsNoTracking()
                .AsQueryable();

            if (customerId.HasValue)
            {
                query = query.Where(d => d.CustomerId == customerId);
            }

            if (assignedToUserId.HasValue)
            {
                query = query.Where(d => d.AssignedToUserId == assignedToUserId);
            }

            if (!string.IsNullOrWhiteSpace(stage))
            {
                query = query.Where(d => d.Stage == stage);
            }

            var deals = await query.OrderByDescending(d => d.CreatedAt).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<DealDto>>(deals));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DealDto>> GetById(int id)
        {
            var deal = await _context.Deals
                .Include(d => d.AssignedToUser)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);

            if (deal == null) return NotFound();
            return Ok(_mapper.Map<DealDto>(deal));
        }

        [HttpPost]
        public async Task<ActionResult<DealDto>> Create(DealUpsertRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _context.Customers.AnyAsync(c => c.Id == request.CustomerId))
            {
                return BadRequest("Customer not found.");
            }

            if (!await _context.Users.AnyAsync(u => u.Id == request.AssignedToUserId && u.IsActive))
            {
                return BadRequest("Assigned user not found or inactive.");
            }

            var deal = new Deal
            {
                Title = request.Title,
                Description = request.Description ?? string.Empty,
                Amount = request.Amount,
                Stage = request.Stage,
                Priority = request.Priority,
                CustomerId = request.CustomerId,
                AssignedToUserId = request.AssignedToUserId,
                ExpectedCloseDate = request.ExpectedCloseDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Deals.Add(deal);
            await _context.SaveChangesAsync();
            await _context.Entry(deal).Reference(d => d.AssignedToUser).LoadAsync();

            return CreatedAtAction(nameof(GetById), new { id = deal.Id }, _mapper.Map<DealDto>(deal));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<DealDto>> Update(int id, DealUpsertRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deal = await _context.Deals.FindAsync(id);
            if (deal == null) return NotFound();

            if (!await _context.Customers.AnyAsync(c => c.Id == request.CustomerId))
            {
                return BadRequest("Customer not found.");
            }

            if (!await _context.Users.AnyAsync(u => u.Id == request.AssignedToUserId && u.IsActive))
            {
                return BadRequest("Assigned user not found or inactive.");
            }

            deal.Title = request.Title;
            deal.Description = request.Description ?? string.Empty;
            deal.Amount = request.Amount;
            deal.Stage = request.Stage;
            deal.Priority = request.Priority;
            deal.CustomerId = request.CustomerId;
            deal.AssignedToUserId = request.AssignedToUserId;
            deal.ExpectedCloseDate = request.ExpectedCloseDate;
            deal.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await _context.Entry(deal).Reference(d => d.AssignedToUser).LoadAsync();

            return Ok(_mapper.Map<DealDto>(deal));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deal = await _context.Deals.FindAsync(id);
            if (deal == null) return NotFound();

            _context.Deals.Remove(deal);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

