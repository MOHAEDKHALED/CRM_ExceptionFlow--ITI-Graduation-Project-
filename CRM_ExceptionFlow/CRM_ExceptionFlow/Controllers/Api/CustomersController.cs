using AutoMapper;
using CRM_ExceptionFlow.Data;
using CRM_ExceptionFlow.DTOs.Common;
using CRM_ExceptionFlow.DTOs.Customers;
using CRM_ExceptionFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM_ExceptionFlow.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager")]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CustomersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<CustomerSummaryDto>>> GetAll(
            [FromQuery] string? searchTerm,
            [FromQuery] string? status,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 25)
        {
            var query = _context.Customers
                .Include(c => c.AssignedToUser)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowered = searchTerm.ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(lowered) ||
                    (c.Email != null && c.Email.ToLower().Contains(lowered)) ||
                    (c.Company != null && c.Company.ToLower().Contains(lowered)));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(c => c.Status == status);
            }

            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dto = _mapper.Map<List<CustomerSummaryDto>>(items);
            return Ok(PagedResult<CustomerSummaryDto>.From(dto, total, pageNumber, pageSize));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerDetailDto>> GetById(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.AssignedToUser)
                .Include(c => c.Deals).ThenInclude(d => d.AssignedToUser)
                .Include(c => c.Interactions).ThenInclude(i => i.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return NotFound();

            var dto = _mapper.Map<CustomerDetailDto>(customer);
            dto.Deals = _mapper.Map<List<DTOs.Deals.DealDto>>(customer.Deals ?? new List<Deal>());
            dto.Interactions = _mapper.Map<List<DTOs.Interactions.InteractionDto>>(customer.Interactions ?? new List<Interaction>());
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerSummaryDto>> Create(CustomerUpsertRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assigneeExists = await _context.Users.AnyAsync(u => u.Id == request.AssignedToUserId && u.IsActive);
            if (!assigneeExists)
            {
                return BadRequest("Assigned user does not exist or is inactive.");
            }

            var customer = new Customer
            {
                Name = request.Name,
                Email = request.Email ?? string.Empty,
                Phone = request.Phone,
                Company = request.Company,
                Address = request.Address,
                Status = request.Status,
                AssignedToUserId = request.AssignedToUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            await _context.Entry(customer).Reference(c => c.AssignedToUser).LoadAsync();

            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, _mapper.Map<CustomerSummaryDto>(customer));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CustomerSummaryDto>> Update(int id, CustomerUpsertRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            var assigneeExists = await _context.Users.AnyAsync(u => u.Id == request.AssignedToUserId && u.IsActive);
            if (!assigneeExists)
            {
                return BadRequest("Assigned user does not exist or is inactive.");
            }

            customer.Name = request.Name;
            customer.Email = request.Email ?? string.Empty;
            customer.Phone = request.Phone;
            customer.Company = request.Company;
            customer.Address = request.Address;
            customer.Status = request.Status;
            customer.AssignedToUserId = request.AssignedToUserId;
            customer.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await _context.Entry(customer).Reference(c => c.AssignedToUser).LoadAsync();

            return Ok(_mapper.Map<CustomerSummaryDto>(customer));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

