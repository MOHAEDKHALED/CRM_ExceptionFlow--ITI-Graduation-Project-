namespace CRM.Application.Common.Interfaces;

public interface IAIRecommendationService
{
    Task<AIRecommendationResponse> GetRecommendationAsync(int exceptionId, CancellationToken cancellationToken = default);
}

public class AIRecommendationResponse
{
    public string Recommendation { get; set; } = string.Empty;
    public decimal Confidence { get; set; }
    public string? Model { get; set; }
    public string? Source { get; set; }
    public bool IsFromDatabase { get; set; }
    public string? Reasoning { get; set; }
}


