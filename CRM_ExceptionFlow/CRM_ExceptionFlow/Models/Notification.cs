using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Type { get; set; } // Deal, Task, Exception, Lead, System

        [Required, StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public int UserId { get; set; }

        public bool IsRead { get; set; } = false;

        [StringLength(200)]
        public string? ActionUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReadAt { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
    }
}
