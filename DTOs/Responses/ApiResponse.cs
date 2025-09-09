namespace backend_api.DTOs.Responses
{
    /// <summary>
    /// Genel API response DTO
    /// </summary>
    /// <typeparam name="T">Response data type</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// İşlem başarılı mı?
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Response mesajı
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Response data
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Hata detayları
        /// </summary>
        public object? Errors { get; set; }

        /// <summary>
        /// Başarılı response oluşturur
        /// </summary>
        public static ApiResponse<T> SuccessResponse(T data, string message = "İşlem başarılı")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Hata response oluşturur
        /// </summary>
        public static ApiResponse<T> ErrorResponse(string message, object? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }

    /// <summary>
    /// Data olmayan API response
    /// </summary>
    public class ApiResponse : ApiResponse<object>
    {
        /// <summary>
        /// Başarılı response oluşturur
        /// </summary>
        public static ApiResponse SuccessResponse(string message = "İşlem başarılı")
        {
            return new ApiResponse
            {
                Success = true,
                Message = message
            };
        }

        /// <summary>
        /// Hata response oluşturur
        /// </summary>
        public static ApiResponse ErrorResponse(string message, object? errors = null)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }
}
