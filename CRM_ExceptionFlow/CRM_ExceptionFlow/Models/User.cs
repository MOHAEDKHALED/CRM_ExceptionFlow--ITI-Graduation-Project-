using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [Required, StringLength(256)]
        public string PasswordHash { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public string Role { get; set; } // Admin, Manager, Employee, ITSupport

        [StringLength(50)]
        public string? Department { get; set; }

        public int? TeamId { get; set; } // For grouping employees under managers

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLogin { get; set; }

        // Navigation Properties
        public virtual ICollection<Exception> ReportedExceptions { get; set; }
        public virtual ICollection<Exception> AssignedExceptions { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Deal> Deals { get; set; }
        public virtual ICollection<Interaction> Interactions { get; set; }
    }
}
