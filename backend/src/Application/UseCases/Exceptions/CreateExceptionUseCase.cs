using AutoMapper;
using CRM.Application.DTOs.Exceptions;
using CRM.Domain.Entities;
using CRM.Domain.Repositories;
using CRM.Domain.UnitOfWork;
using DomainException = CRM.Domain.Entities.Exception;

namespace CRM.Application.UseCases.Exceptions;

public class CreateExceptionUseCase
{
    private readonly IExceptionRepository _exceptionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public CreateExceptionUseCase(
        IExceptionRepository exceptionRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _exceptionRepository = exceptionRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<ExceptionSummaryDto> ExecuteAsync(
        CreateExceptionRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!await _userRepository.ExistsAsync(u => u.Id == request.ReportedByUserId, cancellationToken))
            throw new InvalidOperationException("Reporter not found");
        
        if (request.AssignedToUserId.HasValue && 
            !await _userRepository.ExistsAsync(u => u.Id == request.AssignedToUserId.Value, cancellationToken))
            throw new InvalidOperationException("Assigned user not found");
        
        var priority = Enum.TryParse<ExceptionPriority>(request.Priority, out var p) ? p : ExceptionPriority.Medium;
        
        var exception = new DomainException(
            request.ProjectId,
            request.Module,
            request.Title,
            request.Description,
            request.ReportedByUserId,
            request.StackTrace,
            priority,
            request.AssignedToUserId);
        
        _exceptionRepository.Add(exception);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ExceptionSummaryDto>(exception);
    }
}

