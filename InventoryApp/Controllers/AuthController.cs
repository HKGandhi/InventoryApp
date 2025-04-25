using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryApp.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private static Dictionary<string, string> _refreshTokens = new(); // In-memory, ideally DB

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Validate user (dummy example)
            if (request.Username != "admin" || request.Password != "admin")
                return Unauthorized();

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, request.Username)
        };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            _refreshTokens[request.Username] = refreshToken;

            return Ok(new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("refresh")]
        public IActionResult Refresh(TokenModel tokenModel)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenModel.AccessToken);
            var username = principal.Identity.Name;

            if (!_refreshTokens.TryGetValue(username, out var savedRefreshToken) || savedRefreshToken != tokenModel.RefreshToken)
            {
                return Unauthorized();
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            _refreshTokens[username] = newRefreshToken;

            return Ok(new TokenModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("revoke")]
        public IActionResult Revoke([FromBody] string username)
        {
            _refreshTokens.Remove(username);
            return NoContent();
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
