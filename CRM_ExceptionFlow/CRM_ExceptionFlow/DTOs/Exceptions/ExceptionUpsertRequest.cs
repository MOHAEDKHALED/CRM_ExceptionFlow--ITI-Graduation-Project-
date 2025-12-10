using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.DTOs.Exceptions
{
    public class ExceptionUpsertRequest
    {
        [Required, StringLength(50)]
        public string ProjectId { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Module { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public string? StackTrace { get; set; }

        [Required]
        public string Status { get; set; } = "Open";

        [Required]
        public string Priority { get; set; } = "Medium";

        [Required]
        public int ReportedByUserId { get; set; }

        public int? AssignedToUserId { get; set; }

        public string? ResolutionNotes { get; set; }
    }
}

