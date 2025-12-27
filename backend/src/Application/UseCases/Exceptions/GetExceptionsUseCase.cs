using AutoMapper;
using CRM.Application.DTOs.Common;
using CRM.Application.DTOs.Exceptions;
using CRM.Domain.Entities;
using CRM.Domain.Repositories;
using System.Linq;

namespace CRM.Application.UseCases.Exceptions;

public class GetExceptionsUseCase
{
    private readonly IExceptionRepository _exceptionRepository;
    private readonly IMapper _mapper;
    
    public GetExceptionsUseCase(
        IExceptionRepository exceptionRepository,
        IMapper mapper)
    {
        _exceptionRepository = exceptionRepository;
        _mapper = mapper;
    }
    
    public async Task<PagedResult<ExceptionSummaryDto>> ExecuteAsync(
        string? status,
        string? priority,
        int? assignedToUserId,
        int pageNumber = 1,
        int pageSize = 25,
        CancellationToken cancellationToken = default)
    {
        var allExceptions = await _exceptionRepository.FindAsync(e => true, cancellationToken);
        var query = allExceptions.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<ExceptionStatus>(status, out var statusEnum))
        {
            query = query.Where(e => e.Status == statusEnum);
        }
        
        if (!string.IsNullOrWhiteSpace(priority) && Enum.TryParse<ExceptionPriority>(priority, out var priorityEnum))
        {
            query = query.Where(e => e.Priority == priorityEnum);
        }
        
        if (assignedToUserId.HasValue)
        {
            query = query.Where(e => e.AssignedToUserId == assignedToUserId);
        }
        
        pageNumber = Math.Max(pageNumber, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);
        
        var total = query.Count();
        var items = query
            .OrderByDescending(e => e.ReportedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        var dtos = _mapper.Map<List<ExceptionSummaryDto>>(items);
        return PagedResult<ExceptionSummaryDto>.From(dtos, total, pageNumber, pageSize);
    }
}

