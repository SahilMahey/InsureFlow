namespace InsureFlow.API.Models;

public class Policy
{
    public int Id { get; set; }

    public string PolicyNumber { get; set; }

    public int ApplicationId { get; set; }
public Application Application { get; set; }
    public decimal Premium { get; set; }

    public decimal CoverageAmount { get; set; }

    public DateTime StartDate { get; set; }


    public DateTime EndDate { get; set; }

    public DateTime IssuedOn { get; set; } = DateTime.UtcNow;

}