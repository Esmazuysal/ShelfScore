using backend_api.DTOs.Responses;

namespace backend_api.Services.Interfaces
{
    /// <summary>
    /// User service interface
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Kullanıcı profilini getirir
        /// </summary>
        Task<ApiResponse<object>> GetUserProfileAsync(string username);

        /// <summary>
        /// Çalışan listesini getirir
        /// </summary>
        Task<ApiResponse<object>> GetEmployeesAsync(string managerUsername);

        /// <summary>
        /// Çalışan bilgilerini günceller
        /// </summary>
        Task<ApiResponse<object>> UpdateEmployeeAsync(int employeeId, object employeeData);

        /// <summary>
        /// Çalışanı siler
        /// </summary>
        Task<ApiResponse<object>> DeleteEmployeeAsync(int employeeId);
    }
}
