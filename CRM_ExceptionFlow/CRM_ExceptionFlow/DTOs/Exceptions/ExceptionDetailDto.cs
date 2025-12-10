namespace CRM_ExceptionFlow.DTOs.Exceptions
{
    public class ExceptionDetailDto : ExceptionSummaryDto
    {
        public string Description { get; set; } = string.Empty;
        public string? StackTrace { get; set; }
        public string? ResolutionNotes { get; set; }
        public IReadOnlyCollection<ExceptionHistoryDto> History { get; set; } = Array.Empty<ExceptionHistoryDto>();
        public IReadOnlyCollection<RecommendationDto> Recommendations { get; set; } = Array.Empty<RecommendationDto>();
    }
}

