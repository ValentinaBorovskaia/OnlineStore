using AuthorizationServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Security.Claims;
using System.Text;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IdentityServerSettings settings;

    public AuthController(IOptions<IdentityServerSettings> settings)
    {
        this.settings = settings.Value;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel login)
    {
        var user = Users.ListUsers.FirstOrDefault(obj=> obj.Name == login.Username 
            && obj.Password == login.Password);

        if(user == null)
        {
            return Unauthorized();
        }

        var jwtSecretKey = Encoding.UTF8.GetBytes(settings.JwtSecretKey);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim(ClaimTypes.Role, Enum.GetName(user.Role))
            }),

            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = settings.JwtIssuer,
            Audience = settings.JwtAudience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtSecretKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return Ok(new { token = jwtToken });
    }

    [Authorize]
    [HttpGet("protected-resource")]
    public IActionResult ProtectedResource()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Ok(new { message = "Protected resource accessed by user: " + userId });
    }
}
