using InsureFlow.API.Models;

namespace InsureFlow.API.Services;

public class PremiumService
{
    public decimal CalculatePremium(Application application)
    {
        decimal basePrice = 500;
        decimal multiplier = 25;

        decimal premium = basePrice + (application.RiskScore * multiplier);

        return premium;
    }
}