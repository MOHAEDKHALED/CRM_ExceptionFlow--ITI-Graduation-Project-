using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.Models
{
    public class Exception
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string ProjectId { get; set; }

        [Required, StringLength(50)]
        public string Module { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string StackTrace { get; set; }

        [Required]
        public string Status { get; set; } = "Open";

        [Required]
        public string Priority { get; set; } = "Medium";

        [Required]
        public int ReportedByUserId { get; set; }

        public int? AssignedToUserId { get; set; }

        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ResolvedAt { get; set; }

        public string ResolutionNotes { get; set; }

        // Navigation Properties
        public virtual User ReportedByUser { get; set; }
        public virtual User AssignedToUser { get; set; }
        public virtual ICollection<AIRecommendation> AIRecommendations { get; set; }
        public virtual ICollection<ExceptionHistory> History { get; set; }
    }
}

