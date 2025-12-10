namespace CRM_ExceptionFlow.Services
{
    public interface IAIRecommendationService
    {
        Task<AIRecommendationResponse> GetRecommendationAsync(int exceptionId);
    }

    public class AIRecommendationResponse
    {
        public string Recommendation { get; set; }
        public decimal Confidence { get; set; }
        public string Model { get; set; }
        public string Source { get; set; }
        public bool IsFromDatabase { get; set; }
        public string Reasoning { get; set; }
        public int SimilarExceptionsFound { get; set; }
    }
}