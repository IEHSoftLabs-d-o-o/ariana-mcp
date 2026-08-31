using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Integrations.AraianLab;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ariana_Mcp.Controllers;

[ApiController]
[Route("system")]
[Tags("Auth")]
public sealed class SystemController(SystemService systemService, IArianaLabTokenService tokenService)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var validation = await tokenService.ValidateAuthorizationHeaderAsync(
            Request.Headers.Authorization.ToString());
        if (validation.Status == ArianaLabTokenStatus.Expired)
        {
            return Unauthorized(new { errorMessage = "The Bearer token has expired. Call POST /login again." });
        }

        if (!validation.IsValid)
        {
            return Unauthorized(new { errorMessage = "Missing or invalid Bearer token." });
        }

        var json = await systemService.GetSystemInfoAsync(cancellationToken);
        return Content(json, "application/json");
    }
}
