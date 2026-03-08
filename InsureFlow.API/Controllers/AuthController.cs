using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using InsureFlow.API.Data;
using InsureFlow.API.Models;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _db;
private readonly IConfiguration _config;
    public AuthController(ApplicationDbContext db, IConfiguration config)
{
    _db = db;
    _config = config;
}
  [HttpPost("login")]
public IActionResult Login([FromBody] LoginRequest request)
{
    var u = _db.Users.FirstOrDefault(x => x.Username == request.Username);

    if (u == null || !BCrypt.Net.BCrypt.Verify(request.Password, u.PasswordHash))
    {
        return Unauthorized("Invalid credentials");
    }

    var tokenHandler = new JwtSecurityTokenHandler();
var tokenKey = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, u.Username),
            new Claim(ClaimTypes.Role, u.Role)
        }),
        Expires = DateTime.UtcNow.AddHours(5),
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(tokenKey),
            SecurityAlgorithms.HmacSha256Signature
        )
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);

    return Ok(new
    {
        token = tokenHandler.WriteToken(token),
        role = u.Role
    });
}
}