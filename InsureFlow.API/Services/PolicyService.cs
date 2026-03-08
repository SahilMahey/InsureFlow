using InsureFlow.API.Models;

namespace InsureFlow.API.Services;

public class PolicyService
{
    public Policy CreatePolicy(Application application, decimal premium)
    {
        var policy = new Policy
        {
            PolicyNumber = GeneratePolicyNumber(),
            ApplicationId = application.Id,
            Premium = premium,
            CoverageAmount = 1000000,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddYears(1)
        };

        return policy;
    }

    private string GeneratePolicyNumber()
    {
        var year = DateTime.UtcNow.Year;
        var random = new Random().Next(1000, 9999);

        return $"POL-{year}-{random}";
    }
}