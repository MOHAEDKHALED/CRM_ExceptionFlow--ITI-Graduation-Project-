namespace CRM_ExceptionFlow.DTOs.Deals
{
    public class DealDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public string Stage { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public int AssignedToUserId { get; set; }
        public string? AssignedTo { get; set; }
        public DateTime? ExpectedCloseDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

