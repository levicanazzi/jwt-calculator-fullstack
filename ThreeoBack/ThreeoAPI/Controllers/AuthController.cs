using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThreeoAPI.Models;
using ThreeoAPI.Models.Messages.Requests;
using ThreeoAPI.Models.Messages.Responses;
using ThreeoAPI.Services.AuthServices;

namespace ThreeoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (IsValidUserUsernameAndPassword(model.Username, model.Password))
            {
                var response = new TokenResponse
                {
                    AccessToken = _tokenService.GenerateAccessToken(model.Username),
                    ExpiresIn = (int)(_tokenService.GetAccessTokenExpiration() - DateTime.UtcNow).TotalSeconds,
                    RefreshToken = _tokenService.GenerateRefreshToken(model.Username)
                };

                return Ok(response);
            }

            return Unauthorized(new { message = "Inválid Credentials" });
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest model)
        {
            if (!IsValidUserUsername(model.Username))
            {
                return Unauthorized(new { Message = "Error renewing the access token, authenticate again." });
            }
            
            var newAccessToken = _tokenService.RenewAccessToken(model.RefreshToken);

            if (newAccessToken.Equals("Error"))
            {
                return Unauthorized(new { Message = "Error renewing the access token, authenticate again." });
            }

            var newRefreshToken = _tokenService.GenerateRefreshToken(model.Username);


            var response = new TokenResponse
            {
                AccessToken = newAccessToken,
                ExpiresIn = (int)(_tokenService.GetAccessTokenExpiration() - DateTime.UtcNow).TotalSeconds,
                RefreshToken = newRefreshToken
            };

            return Ok(response);
        }

        private bool IsValidUserUsernameAndPassword(string username, string password)
        {
            return username == "admin" && password == "admin2024";
        }

        private bool IsValidUserUsername(string username)
        {
            return username == "admin";
        }
    }
}
