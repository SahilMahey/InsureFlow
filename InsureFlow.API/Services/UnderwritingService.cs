using InsureFlow.API.Models;
using InsureFlow.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InsureFlow.API.Services;

public class UnderwritingService
{
    private readonly ApplicationDbContext _db;

    public UnderwritingService(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public (int score, string decision) EvaluateRisk(Application application)
{
    var rules = _db.UnderwritingRules.ToList();

    int totalRisk = 0;

    foreach (var rule in rules)
    {
        if (rule.Field == "AnnualRevenue")
        {
            if (application.AnnualRevenue > int.Parse(rule.Value))
                totalRisk += rule.RiskPoints;
        }

        if (rule.Field == "ClaimsHistory")
        {
            if (application.ClaimsHistory > int.Parse(rule.Value))
                totalRisk += rule.RiskPoints;
        }

        if (rule.Field == "BusinessType")
        {
            if (application.BusinessType == rule.Value)
                totalRisk += rule.RiskPoints;
        }

        if (rule.Field == "YearsInBusiness")
        {
            if (application.YearsInBusiness < int.Parse(rule.Value))
                totalRisk += rule.RiskPoints;
        }
    }

    string decision;

    if (totalRisk <= 30)
        decision = "Approved";
    else if (totalRisk <= 60)
        decision = "Review";
    else
        decision = "Rejected";

    return (totalRisk, decision);
}
}