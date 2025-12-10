using CRM_ExceptionFlow.Data;
using CRM_ExceptionFlow.DTOs.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM_ExceptionFlow.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
        {
            var summary = new DashboardSummaryDto
            {
                TotalCustomers = await _context.Customers.CountAsync(),
                ActiveDeals = await _context.Deals.CountAsync(d => d.Stage != "Closed Lost"),
                OpenExceptions = await _context.Exceptions.CountAsync(e => e.Status != "Resolved"),
                UpcomingInteractions = await _context.Interactions.CountAsync(i => i.InteractionDate >= DateTime.UtcNow.Date)
            };

            return Ok(summary);
        }
    }
}

