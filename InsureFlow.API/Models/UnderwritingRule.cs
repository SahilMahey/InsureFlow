namespace InsureFlow.API.Models;

public class UnderwritingRule
{
    public int Id { get; set; }

    // Field in Application to check
    public string Field { get; set; }

    // Operator: >, <, ==, !=
    public string Operator { get; set; }

    // Value to compare against
    public string Value { get; set; }

    // Risk points to add if rule matches
    public int RiskPoints { get; set; }
}