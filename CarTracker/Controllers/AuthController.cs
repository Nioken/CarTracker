using System.Security.Claims;
using CarTracker.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CarTracker.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authCodeService;

    public AuthController(IAuthService authCodeService)
    {
        _authCodeService = authCodeService;
    }

    public record LoginRequest(string Code);

    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] LoginRequest request)
    {
        var telegramId = _authCodeService.ValidateCode(request.Code);
        if (telegramId == null) return BadRequest("Неверный код");
        
        var claims = new List<Claim>
        {
            new("TelegramId", telegramId.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return Ok(new { message = "Успешно!" });
    }
    
[HttpGet("me")]
public IActionResult GetMe()
{
    if (!User.Identity.IsAuthenticated) 
    {
        return Unauthorized();
    }

    var telegramIdString = User.FindFirstValue("TelegramId");

    if (long.TryParse(telegramIdString, out var telegramId))
    {
        return Ok(new { id = telegramId });
    }

    return Ok(new { Message = "Telegram ID not found in session" });
}
}