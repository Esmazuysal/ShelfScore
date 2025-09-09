using backend_api.Models;

namespace backend_api.Services.Interfaces
{
    /// <summary>
    /// JWT service interface
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// JWT token oluşturur
        /// </summary>
        string GenerateToken(User user);

        /// <summary>
        /// Token'ı doğrular
        /// </summary>
        Task<bool> ValidateTokenAsync(string token);

        /// <summary>
        /// Token'dan username'i çıkarır
        /// </summary>
        string? GetUsernameFromToken(string token);
    }
}
