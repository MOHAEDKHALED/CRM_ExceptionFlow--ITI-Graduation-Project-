namespace CRM_ExceptionFlow.DTOs.Interactions
{
    public class InteractionDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime InteractionDate { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string? UserFullName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

