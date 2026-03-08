namespace InsureFlow.API.Models;

public class Application
{
    public int Id { get; set; }

    public string BusinessName { get; set; }

    public string BusinessType { get; set; }

    public decimal AnnualRevenue { get; set; }

    public int Employees { get; set; }

    public int YearsInBusiness { get; set; }

    public int ClaimsHistory { get; set; }

    public int RiskScore { get; set; }

    public string Decision { get; set; }
    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}