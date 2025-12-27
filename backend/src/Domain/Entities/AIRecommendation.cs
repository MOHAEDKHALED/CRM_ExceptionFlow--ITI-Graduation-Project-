using CRM.Domain.Common;

namespace CRM.Domain.Entities;

public class AIRecommendation : Entity<int>
{
    public int ExceptionId { get; private set; }
    public string RecommendationText { get; private set; } = string.Empty;
    public decimal ConfidenceScore { get; private set; }
    public string Model { get; private set; } = string.Empty;
    public string Source { get; private set; } = string.Empty;
    public bool IsFromDatabase { get; private set; }
    public DateTime GeneratedAt { get; private set; } = DateTime.UtcNow;
    public bool? WasHelpful { get; private set; }
    
    // Navigation property (for EF Core)
    public CRM.Domain.Entities.Exception? Exception { get; private set; }
    
    private AIRecommendation() { }
    
    public AIRecommendation(
        int exceptionId,
        string recommendationText,
        decimal confidenceScore,
        string model,
        string source,
        bool isFromDatabase = false)
    {
        ExceptionId = exceptionId;
        RecommendationText = recommendationText;
        ConfidenceScore = confidenceScore;
        Model = model;
        Source = source;
        IsFromDatabase = isFromDatabase;
        GeneratedAt = DateTime.UtcNow;
    }
    
    public void MarkAsHelpful(bool helpful)
    {
        WasHelpful = helpful;
    }
}

