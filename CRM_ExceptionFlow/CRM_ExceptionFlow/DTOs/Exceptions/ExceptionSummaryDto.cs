namespace CRM_ExceptionFlow.DTOs.Exceptions
{
    public class ExceptionSummaryDto
    {
        public int Id { get; set; }
        public string ProjectId { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = "Open";
        public string Priority { get; set; } = "Medium";
        public int ReportedByUserId { get; set; }
        public int? AssignedToUserId { get; set; }
        public string? ReportedBy { get; set; }
        public string? AssignedTo { get; set; }
        public DateTime ReportedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }
}

