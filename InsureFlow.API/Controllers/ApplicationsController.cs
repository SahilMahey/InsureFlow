using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InsureFlow.API.Data;
using InsureFlow.API.Models;
using InsureFlow.API.Services;

namespace InsureFlow.API.Controllers;

[Authorize(Roles = "Agent,Admin")]
[ApiController]
[Route("applications")]
public class ApplicationsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UnderwritingService _underwritingService;
    private readonly PremiumService _premiumService;
    private readonly PolicyService _policyService;

    public ApplicationsController(
        ApplicationDbContext context,
        UnderwritingService underwritingService,
        PremiumService premiumService,
        PolicyService policyService)
    {
        _context = context;
        _underwritingService = underwritingService;
        _premiumService = premiumService;
        _policyService = policyService;
    }

    [HttpPost]
    public IActionResult CreateApplication([FromBody] Application application)
    {
        var result = _underwritingService.EvaluateRisk(application);

        application.RiskScore = result.score;
        application.Decision = result.decision;
        application.Status = "Evaluated";

        _context.Applications.Add(application);
        _context.SaveChanges();

        return Ok(application);
    }

    [HttpGet]
    public IActionResult GetApplications()
    {
        var apps = _context.Applications.ToList();
        return Ok(apps);
    }

    [HttpGet("{id}/premium")]
    public IActionResult CalculatePremium(int id)
    {
        var application = _context.Applications.FirstOrDefault(a => a.Id == id);

        if (application == null)
            return NotFound("Application not found");

        if (application.Decision == "Rejected")
            return BadRequest("Rejected applications cannot get premium");

        var premium = _premiumService.CalculatePremium(application);

        return Ok(new
        {
            ApplicationId = application.Id,
            RiskScore = application.RiskScore,
            Premium = premium
        });
    }

    [Authorize(Roles = "Agent,Admin")]
    [HttpPost("{id}/issue")]
    public IActionResult IssuePolicy(int id)
    {
        var app = _context.Applications.Find(id);
        if (app == null) return NotFound();

        if (app.Decision != "Approved")
            return BadRequest("Only approved applications can be issued");

      var premium = _premiumService.CalculatePremium(app);

var policy = _policyService.CreatePolicy(app, premium);

        _context.Policies.Add(policy);
        app.Status = "Issued";

        _context.SaveChanges();

        return Ok(policy);
    }
}