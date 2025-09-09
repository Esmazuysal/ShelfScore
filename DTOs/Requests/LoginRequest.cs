using System.ComponentModel.DataAnnotations;

namespace backend_api.DTOs.Requests
{
    /// <summary>
    /// Kullanıcı girişi için request DTO
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Kullanıcı adı
        /// </summary>
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        [StringLength(50, ErrorMessage = "Kullanıcı adı en fazla 50 karakter olabilir")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Şifre
        /// </summary>
        [Required(ErrorMessage = "Şifre zorunludur")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Password { get; set; } = string.Empty;
    }
}
