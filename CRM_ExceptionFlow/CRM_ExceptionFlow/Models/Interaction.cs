using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.Models
{
    public class Interaction
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; } // Call, Email, Meeting, Note, Task

        [Required, StringLength(200)]
        public string Subject { get; set; }

        public string Notes { get; set; }

        [Required]
        public DateTime InteractionDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Customer Customer { get; set; }
        public virtual User User { get; set; }
    }
}