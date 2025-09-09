using backend_api.DTOs.Requests;
using backend_api.DTOs.Responses;

namespace backend_api.Services.Interfaces
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Kullanıcı girişi yapar
        /// </summary>
        Task<ApiResponse<object>> LoginAsync(LoginRequest request);

        /// <summary>
        /// Manager hesabı oluşturur
        /// </summary>
        Task<ApiResponse<object>> RegisterManagerAsync(RegisterRequest request);

        /// <summary>
        /// Çalışan hesabı oluşturur
        /// </summary>
        Task<ApiResponse<object>> CreateEmployeeAsync(CreateEmployeeRequest request, string managerUsername);

        /// <summary>
        /// Çalışan hesabını siler
        /// </summary>
        Task<ApiResponse<object>> DeleteEmployeeAsync(int employeeId);
    }
}
