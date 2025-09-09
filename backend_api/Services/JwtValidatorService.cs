using Microsoft.EntityFrameworkCore;
using backend_api.Data;
using backend_api.Common.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace backend_api.Services
{
    public class JwtValidatorService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<JwtValidatorService> _logger;

        public JwtValidatorService(ApplicationDbContext context, ILogger<JwtValidatorService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                // Token'dan username ve passwordChangeTime claim'lerini al
                var username = jwtToken.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
                var passwordChangeTimeClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "passwordChangeTime")?.Value;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(passwordChangeTimeClaim))
                {
                    _logger.LogWarning("JWT Validation failed: Missing claims for token");
                    return false;
                }

                // Kullanıcıyı veritabanından bul
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    _logger.LogWarning("JWT Validation failed: User not found: {Username}", username);
                    return false;
                }

                // Şifre değiştirilme zamanını karşılaştır
                if (DateTime.TryParse(passwordChangeTimeClaim, out var tokenPasswordChangeTime))
                {
                    // Token'daki şifre değiştirilme zamanı, kullanıcının gerçek şifre değiştirilme zamanından eskiyse token geçersiz
                    if (tokenPasswordChangeTime < user.CreatedAt)
                    {
                        _logger.LogWarning("JWT Validation failed: Token password time ({TokenTime}) < User password time ({UserTime})", 
                            tokenPasswordChangeTime, user.CreatedAt);
                        return false;
                    }
                }

                _logger.LogInformation("JWT Validation successful for user: {Username}", username);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "JWT Validation error: {Message}", ex.Message);
                return false;
            }
        }
    }
}
