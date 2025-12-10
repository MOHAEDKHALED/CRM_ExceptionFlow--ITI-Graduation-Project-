using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.DTOs.Deals
{
    public class DealUpsertRequest
    {
        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public string Stage { get; set; } = "Prospecting";

        [Required]
        public string Priority { get; set; } = "Medium";

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int AssignedToUserId { get; set; }

        public DateTime? ExpectedCloseDate { get; set; }
    }
}

