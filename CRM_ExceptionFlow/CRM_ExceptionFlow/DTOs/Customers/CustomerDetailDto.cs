using CRM_ExceptionFlow.DTOs.Deals;
using CRM_ExceptionFlow.DTOs.Interactions;

namespace CRM_ExceptionFlow.DTOs.Customers
{
    public class CustomerDetailDto : CustomerSummaryDto
    {
        public string? Address { get; set; }
        public IReadOnlyCollection<DealDto> Deals { get; set; } = Array.Empty<DealDto>();
        public IReadOnlyCollection<InteractionDto> Interactions { get; set; } = Array.Empty<InteractionDto>();
    }
}

