using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.Models
{
    public class AIRecommendation
    {
        public int Id { get; set; }

        [Required]
        public int ExceptionId { get; set; }

        [Required]
        public string RecommendationText { get; set; }

        [Required, Range(0, 1)]
        public decimal ConfidenceScore { get; set; }

        [Required, StringLength(50)]
        public string Model { get; set; }

        [Required]
        public string Source { get; set; } // Database, AI_Model, Hybrid

        public bool IsFromDatabase { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        public bool? WasHelpful { get; set; }

        // Navigation Properties
        public virtual Exception Exception { get; set; }
    }
}