using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.DTOs.Interactions
{
    public class InteractionRequest
    {
        [Required]
        [RegularExpression("Call|Email|Meeting|Note|Task")]
        public string Type { get; set; } = "Call";

        [Required, StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        public string? Notes { get; set; }

        [Required]
        public DateTime InteractionDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}

