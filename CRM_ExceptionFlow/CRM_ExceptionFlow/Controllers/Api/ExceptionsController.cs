using AutoMapper;
using CRM_ExceptionFlow.Data;
using CRM_ExceptionFlow.DTOs.Common;
using CRM_ExceptionFlow.DTOs.Exceptions;
using CRM_ExceptionFlow.Models;
using CRM_ExceptionFlow.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM_ExceptionFlow.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public class ExceptionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAIRecommendationService _aiService;

        public ExceptionsController(
            ApplicationDbContext context,
            IMapper mapper,
            IAIRecommendationService aiService)
        {
            _context = context;
            _mapper = mapper;
            _aiService = aiService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ExceptionSummaryDto>>> GetAll(
            [FromQuery] string? status,
            [FromQuery] string? priority,
            [FromQuery] int? assignedToUserId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 25)
        {
            var query = _context.Exceptions
                .Include(e => e.ReportedByUser)
                .Include(e => e.AssignedToUser)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(e => e.Status == status);
            }

            if (!string.IsNullOrWhiteSpace(priority))
            {
                query = query.Where(e => e.Priority == priority);
            }

            if (assignedToUserId.HasValue)
            {
                query = query.Where(e => e.AssignedToUserId == assignedToUserId);
            }

            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(e => e.ReportedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dto = _mapper.Map<List<ExceptionSummaryDto>>(items);
            return Ok(PagedResult<ExceptionSummaryDto>.From(dto, total, pageNumber, pageSize));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ExceptionDetailDto>> GetById(int id)
        {
            var exception = await _context.Exceptions
                .Include(e => e.ReportedByUser)
                .Include(e => e.AssignedToUser)
                .Include(e => e.History)
                .Include(e => e.AIRecommendations)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exception == null) return NotFound();

            var dto = _mapper.Map<ExceptionDetailDto>(exception);
            dto.History = _mapper.Map<List<ExceptionHistoryDto>>(exception.History ?? new List<ExceptionHistory>());
            dto.Recommendations = _mapper.Map<List<RecommendationDto>>(exception.AIRecommendations ?? new List<AIRecommendation>());
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ExceptionSummaryDto>> Create(ExceptionUpsertRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _context.Users.AnyAsync(u => u.Id == request.ReportedByUserId))
            {
                return BadRequest("Reporter not found.");
            }

            if (request.AssignedToUserId.HasValue && !await _context.Users.AnyAsync(u => u.Id == request.AssignedToUserId))
            {
                return BadRequest("Assigned user not found.");
            }

            var exception = new Models.Exception
            {
                ProjectId = request.ProjectId,
                Module = request.Module,
                Title = request.Title,
                Description = request.Description,
                StackTrace = request.StackTrace,
                Status = request.Status,
                Priority = request.Priority,
                ReportedByUserId = request.ReportedByUserId,
                AssignedToUserId = request.AssignedToUserId,
                ResolutionNotes = request.ResolutionNotes,
                ReportedAt = DateTime.UtcNow
            };

            _context.Exceptions.Add(exception);
            await _context.SaveChangesAsync();

            AddHistory(exception.Id, exception.Status, "Exception created.");
            await _context.SaveChangesAsync();

            await _context.Entry(exception).Reference(e => e.ReportedByUser).LoadAsync();
            await _context.Entry(exception).Reference(e => e.AssignedToUser).LoadAsync();

            return CreatedAtAction(nameof(GetById), new { id = exception.Id }, _mapper.Map<ExceptionSummaryDto>(exception));
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ExceptionSummaryDto>> Update(int id, ExceptionUpsertRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exception = await _context.Exceptions
                .Include(e => e.History)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exception == null) return NotFound();

            if (!await _context.Users.AnyAsync(u => u.Id == request.ReportedByUserId))
            {
                return BadRequest("Reporter not found.");
            }

            if (request.AssignedToUserId.HasValue && !await _context.Users.AnyAsync(u => u.Id == request.AssignedToUserId))
            {
                return BadRequest("Assigned user not found.");
            }

            var originalStatus = exception.Status;
            exception.ProjectId = request.ProjectId;
            exception.Module = request.Module;
            exception.Title = request.Title;
            exception.Description = request.Description;
            exception.StackTrace = request.StackTrace;
            exception.Status = request.Status;
            exception.Priority = request.Priority;
            exception.ReportedByUserId = request.ReportedByUserId;
            exception.AssignedToUserId = request.AssignedToUserId;
            exception.ResolutionNotes = request.ResolutionNotes;

            if (request.Status == "Resolved" && exception.ResolvedAt == null)
            {
                exception.ResolvedAt = DateTime.UtcNow;
            }
            else if (request.Status != "Resolved")
            {
                exception.ResolvedAt = null;
            }

            if (originalStatus != request.Status)
            {
                AddHistory(exception.Id, request.Status, $"Status changed from {originalStatus} to {request.Status}.");
            }

            await _context.SaveChangesAsync();
            await _context.Entry(exception).Reference(e => e.ReportedByUser).LoadAsync();
            await _context.Entry(exception).Reference(e => e.AssignedToUser).LoadAsync();

            return Ok(_mapper.Map<ExceptionSummaryDto>(exception));
        }

        [HttpGet("{id:int}/recommendations")]
        public async Task<ActionResult<IEnumerable<RecommendationDto>>> GetRecommendations(int id)
        {
            var exceptionExists = await _context.Exceptions.AnyAsync(e => e.Id == id);
            if (!exceptionExists) return NotFound();

            var recommendation = await _aiService.GetRecommendationAsync(id);

            var record = new AIRecommendation
            {
                ExceptionId = id,
                RecommendationText = recommendation.Recommendation,
                ConfidenceScore = recommendation.Confidence,
                Model = recommendation.Model,
                Source = recommendation.Source,
                IsFromDatabase = recommendation.IsFromDatabase,
                GeneratedAt = DateTime.UtcNow
            };

            _context.AIRecommendations.Add(record);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<RecommendationDto>(record);
            return Ok(new[] { dto });
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Manager,Employee")]
        public async Task<IActionResult> Delete(int id)
        {
            var exception = await _context.Exceptions
                .Include(e => e.History)
                .Include(e => e.AIRecommendations)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exception == null) return NotFound();

            // Delete related records first
            if (exception.History != null && exception.History.Any())
            {
                _context.ExceptionHistory.RemoveRange(exception.History);
            }

            if (exception.AIRecommendations != null && exception.AIRecommendations.Any())
            {
                _context.AIRecommendations.RemoveRange(exception.AIRecommendations);
            }

            _context.Exceptions.Remove(exception);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private void AddHistory(int exceptionId, string status, string notes)
        {
            var changedBy = User?.Claims?.FirstOrDefault(c => c.Type == "fullName")?.Value
                            ?? User?.Identity?.Name
                            ?? "System";

            _context.ExceptionHistory.Add(new ExceptionHistory
            {
                ExceptionId = exceptionId,
                Status = status,
                Notes = notes,
                ChangedByUserName = changedBy,
                ChangedAt = DateTime.UtcNow
            });
        }
    }
}

