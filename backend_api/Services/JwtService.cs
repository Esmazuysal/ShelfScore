using backend_api.Models;
using backend_api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend_api.Services
{
    /// <summary>
    /// JWT service implementation
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtService> _logger;

        public JwtService(IConfiguration configuration, ILogger<JwtService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GenerateToken(User user)
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "default-secret-key");
                var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "1440");

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("username", user.Username),
                        new Claim("role", user.Role),
                        new Claim("userId", user.Id.ToString()),
                        new Claim("passwordChangeTime", user.CreatedAt.ToString("O"))
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature
                    )
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                _logger.LogInformation("JWT token generated for user: {Username}", user.Username);
                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token for user: {Username}", user.Username);
                throw;
            }
        }

        public Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var username = jwtToken.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
                var passwordChangeTimeClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "passwordChangeTime")?.Value;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(passwordChangeTimeClaim))
                {
                    _logger.LogWarning("JWT Validation failed: Missing claims for token");
                    return Task.FromResult(false);
                }

                _logger.LogInformation("JWT token validated successfully for user: {Username}", username);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "JWT token validation error");
                return Task.FromResult(false);
            }
        }

        public string? GetUsernameFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                return jwtToken.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting username from JWT token");
                return null;
            }
        }
    }
}
