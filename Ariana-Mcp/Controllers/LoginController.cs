using Ariana_Mcp.integrations.Models;
using Ariana_Mcp.integrations.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ariana_Mcp.Controllers;

[ApiController]
[Route("login")]
[Tags("Auth")]
public sealed class LoginController(AuthService authService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await authService.LoginAsync(request.User, request.Password, cancellationToken);
            if (result.LoginToken is null)
                return BadRequest(result);

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new LoginResponse { ErrorMessage = "Login failed because ArianaLab could not be reached." });
        }
    }
}
