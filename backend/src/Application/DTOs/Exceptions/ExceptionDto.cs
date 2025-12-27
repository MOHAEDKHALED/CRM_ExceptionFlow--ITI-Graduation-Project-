namespace CRM.Application.DTOs.Exceptions;

public class ExceptionDto
{
    public int Id { get; set; }
    public string ProjectId { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public int ReportedByUserId { get; set; }
    public string? ReportedByUserName { get; set; }
    public int? AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; }
    public DateTime ReportedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ResolutionNotes { get; set; }
    public List<ExceptionHistoryDto> History { get; set; } = new();
    public List<RecommendationDto> Recommendations { get; set; } = new();
}

public class ExceptionSummaryDto
{
    public int Id { get; set; }
    public string ProjectId { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string? ReportedByUserName { get; set; }
    public string? AssignedToUserName { get; set; }
    public DateTime ReportedAt { get; set; }
}

public class ExceptionHistoryDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ChangedByUserName { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime ChangedAt { get; set; }
}

public class RecommendationDto
{
    public int Id { get; set; }
    public string RecommendationText { get; set; } = string.Empty;
    public decimal ConfidenceScore { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public bool IsFromDatabase { get; set; }
    public DateTime GeneratedAt { get; set; }
    public bool? WasHelpful { get; set; }
}

public class CreateExceptionRequest
{
    public string ProjectId { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
    public string Priority { get; set; } = "Medium";
    public int ReportedByUserId { get; set; }
    public int? AssignedToUserId { get; set; }
}

public class UpdateExceptionRequest
{
    public string ProjectId { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public int ReportedByUserId { get; set; }
    public int? AssignedToUserId { get; set; }
    public string? ResolutionNotes { get; set; }
}


