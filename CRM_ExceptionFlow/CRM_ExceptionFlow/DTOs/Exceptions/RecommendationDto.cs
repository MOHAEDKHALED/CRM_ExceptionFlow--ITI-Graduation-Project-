namespace CRM_ExceptionFlow.DTOs.Exceptions
{
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
}

