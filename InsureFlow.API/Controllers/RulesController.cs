using Microsoft.AspNetCore.Mvc;
using InsureFlow.API.Data;
using InsureFlow.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace InsureFlow.API.Controllers;

[Authorize(Roles="Underwriter,Admin")]

[ApiController]
[Route("rules")]
public class RulesController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public RulesController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET /rules
    [HttpGet]
    public IActionResult GetAllRules()
    {
        return Ok(_db.UnderwritingRules.ToList());
    }

    // GET /rules/{id}
    [HttpGet("{id}")]
    public IActionResult GetRuleById(int id)
    {
        var rule = _db.UnderwritingRules.FirstOrDefault(r => r.Id == id);
        if (rule == null) return NotFound("Rule not found");
        return Ok(rule);
    }

    // PUT /rules/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateRule(int id, [FromBody] UnderwritingRule updatedRule)
    {
        var rule = _db.UnderwritingRules.FirstOrDefault(r => r.Id == id);
        if (rule == null) return NotFound("Rule not found");

        rule.Field = updatedRule.Field;
        rule.Operator = updatedRule.Operator;
        rule.Value = updatedRule.Value;
        rule.RiskPoints = updatedRule.RiskPoints;

        _db.SaveChanges();
        return Ok(rule);
    }

[Authorize(Roles="Underwriter")]
[HttpPost]
public IActionResult CreateRule([FromBody] UnderwritingRule rule)
{
    _db.UnderwritingRules.Add(rule);
    _db.SaveChanges();
    return Ok(rule);
}

[Authorize(Roles="Underwriter")]
[HttpDelete("{id}")]
public IActionResult DeleteRule(int id)
{
    var rule = _db.UnderwritingRules.Find(id);
    if(rule == null) return NotFound();
    _db.UnderwritingRules.Remove(rule);
    _db.SaveChanges();
    return Ok();
}
}