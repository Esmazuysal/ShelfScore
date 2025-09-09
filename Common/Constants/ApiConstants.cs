namespace backend_api.Common.Constants
{
    /// <summary>
    /// API ile ilgili sabit değerler
    /// </summary>
    public static class ApiConstants
    {
        /// <summary>
        /// API route prefix
        /// </summary>
        public const string ApiRoutePrefix = "api";
        
        /// <summary>
        /// JWT token expiration time in minutes
        /// </summary>
        public const int JwtExpirationMinutes = 1440; // 24 saat
        
        /// <summary>
        /// Default page size for pagination
        /// </summary>
        public const int DefaultPageSize = 20;
        
        /// <summary>
        /// Maximum page size for pagination
        /// </summary>
        public const int MaxPageSize = 100;
        
        /// <summary>
        /// File upload maximum size in bytes (10MB)
        /// </summary>
        public const long MaxFileSize = 10 * 1024 * 1024;
        
        /// <summary>
        /// Allowed image file extensions
        /// </summary>
        public static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
    }
}
