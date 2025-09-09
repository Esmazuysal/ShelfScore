using System.ComponentModel.DataAnnotations;

namespace backend_api.DTOs.Requests
{
    /// <summary>
    /// Çalışan oluşturma için request DTO
    /// </summary>
    public class CreateEmployeeRequest
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

        /// <summary>
        /// Ad
        /// </summary>
        [Required(ErrorMessage = "Ad zorunludur")]
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Soyad
        /// </summary>
        [Required(ErrorMessage = "Soyad zorunludur")]
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        [StringLength(100, ErrorMessage = "Email en fazla 100 karakter olabilir")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Market adı
        /// </summary>
        [Required(ErrorMessage = "Market adı zorunludur")]
        [StringLength(100, ErrorMessage = "Market adı en fazla 100 karakter olabilir")]
        public string StoreName { get; set; } = string.Empty;

        /// <summary>
        /// Departman adı
        /// </summary>
        [StringLength(50, ErrorMessage = "Departman adı en fazla 50 karakter olabilir")]
        public string? DepartmentName { get; set; }
    }
}
