namespace CRM.Application.DTOs.Dashboard;

public class DashboardSummaryDto
{
    public int TotalCustomers { get; set; }
    public int ActiveCustomers { get; set; }
    public int TotalDeals { get; set; }
    public decimal TotalDealValue { get; set; }
    public int OpenExceptions { get; set; }
    public int ResolvedExceptions { get; set; }
    public int TotalInteractions { get; set; }
    public int TotalUsers { get; set; }
}


