using CRM.Application.DTOs.Common;
using CRM.Application.DTOs.Exceptions;
using CRM.Application.UseCases.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Manager,Employee")]
public class ExceptionsController : ControllerBase
{
    private readonly GetExceptionsUseCase _getExceptionsUseCase;
    private readonly CreateExceptionUseCase _createExceptionUseCase;
    
    public ExceptionsController(
        GetExceptionsUseCase getExceptionsUseCase,
        CreateExceptionUseCase createExceptionUseCase)
    {
        _getExceptionsUseCase = getExceptionsUseCase;
        _createExceptionUseCase = createExceptionUseCase;
    }
    
    [HttpGet]
    public async Task<ActionResult<PagedResult<ExceptionSummaryDto>>> GetAll(
        [FromQuery] string? status,
        [FromQuery] string? priority,
        [FromQuery] int? assignedToUserId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 25)
    {
        var result = await _getExceptionsUseCase.ExecuteAsync(
            status, priority, assignedToUserId, pageNumber, pageSize);
        
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExceptionDto>> GetById(int id)
    {
        // Implement GetExceptionByIdUseCase
        return NotFound();
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<ExceptionSummaryDto>> Create([FromBody] CreateExceptionRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
            var result = await _createExceptionUseCase.ExecuteAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<ExceptionSummaryDto>> Update(int id, [FromBody] UpdateExceptionRequest request)
    {
        // Implement UpdateExceptionUseCase
        return NotFound();
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public async Task<IActionResult> Delete(int id)
    {
        // Implement DeleteExceptionUseCase
        return NotFound();
    }
    
    [HttpGet("{id:int}/recommendations")]
    public async Task<ActionResult<IEnumerable<RecommendationDto>>> GetRecommendations(int id)
    {
        // Implement GetRecommendationsUseCase
        return NotFound();
    }
}


