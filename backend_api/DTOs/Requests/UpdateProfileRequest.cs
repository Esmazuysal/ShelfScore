using System.ComponentModel.DataAnnotations;

namespace backend_api.DTOs.Requests
{
    /// <summary>
    /// Profil güncelleme için request DTO
    /// </summary>
    public class UpdateProfileRequest
    {
        /// <summary>
        /// Ad
        /// </summary>
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Soyad
        /// </summary>
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir")]
        public string? LastName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        [StringLength(100, ErrorMessage = "Email en fazla 100 karakter olabilir")]
        public string? Email { get; set; }

        /// <summary>
        /// Market adresi
        /// </summary>
        [StringLength(200, ErrorMessage = "Market adresi en fazla 200 karakter olabilir")]
        public string? StoreAddress { get; set; }

        /// <summary>
        /// Market telefonu
        /// </summary>
        [StringLength(20, ErrorMessage = "Market telefonu en fazla 20 karakter olabilir")]
        public string? StorePhone { get; set; }

        /// <summary>
        /// Departman adı
        /// </summary>
        [StringLength(50, ErrorMessage = "Departman adı en fazla 50 karakter olabilir")]
        public string? DepartmentName { get; set; }
    }
}
