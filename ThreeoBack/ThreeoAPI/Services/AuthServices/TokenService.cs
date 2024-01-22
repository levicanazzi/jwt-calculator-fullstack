using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ThreeoAPI.Services.AuthServices
{
    public class TokenService : ITokenService 
    {
        private readonly IConfiguration _configuration;
        private readonly double _accessTokenExpirationMinutes;
        private readonly double _refreshTokenExpirationMinutes;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _accessTokenExpirationMinutes = double.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"]);
            _refreshTokenExpirationMinutes = double.Parse(_configuration["Jwt:RefreshTokenExpirationMinutes"]);
        }

        public string GenerateAccessToken(string userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:AccessTokenKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userId),
            new Claim("TokenType", "access-token")
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(string userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:RefreshTokenKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userId),
            new Claim("TokenType", "refresh-token")
        };

            var refreshToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(_refreshTokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        }

        public string RenewAccessToken(string refreshToken)
        {

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var refreshTokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:RefreshTokenKey"])),
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"]
                };

                SecurityToken validatedToken;
                var principal = handler.ValidateToken(refreshToken, refreshTokenValidationParameters, out validatedToken);

                var userId = principal.FindFirst(ClaimTypes.Name).Value;

                var newAccessToken = GenerateAccessToken(userId);

                return newAccessToken;
            }
            catch (SecurityTokenValidationException ex)
            {
                return "Error";
            }
        }

        public DateTime GetAccessTokenExpiration()
        {
            return DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes);
        }
    }
}
