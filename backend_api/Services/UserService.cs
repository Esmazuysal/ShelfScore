using backend_api.DTOs.Responses;
using backend_api.Models;
using backend_api.Repositories.Interfaces;
using backend_api.Services.Interfaces;

namespace backend_api.Services
{
    /// <summary>
    /// User service implementation
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<ApiResponse<object>> GetUserProfileAsync(string username)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(username);
                if (user == null)
                {
                    return ApiResponse<object>.ErrorResponse("Kullanıcı bulunamadı");
                }

                var profileData = new
                {
                    id = user.Id,
                    username = user.Username,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    email = user.Email,
                    role = user.Role,
                    storeName = user.StoreName,
                    storeAddress = user.StoreAddress,
                    storePhone = user.StorePhone,
                    departmentName = user.DepartmentName,
                    createdAt = user.CreatedAt,
                    lastLoginAt = user.LastLoginAt,
                    isActive = user.IsActive
                };

                return ApiResponse<object>.SuccessResponse(profileData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile for: {Username}", username);
                return ApiResponse<object>.ErrorResponse("Profil bilgileri alınamadı");
            }
        }

        public async Task<ApiResponse<object>> GetEmployeesAsync(string managerUsername)
        {
            try
            {
                var manager = await _userRepository.GetByUsernameAsync(managerUsername);
                if (manager == null || manager.Role != "Manager")
                {
                    return ApiResponse<object>.ErrorResponse("Manager yetkisi gerekli");
                }

                // Sadece bu manager'ın çalışanlarını getir
                var employees = await _userRepository.GetEmployeesByManagerAsync(manager.StoreName);
                var employeeData = employees.Select(u => new
                {
                    id = u.Id,
                    username = u.Username,
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    email = u.Email,
                    departmentName = u.DepartmentName,
                    storeName = u.StoreName,
                    isActive = u.IsActive,
                    createdAt = u.CreatedAt,
                    lastLoginAt = u.LastLoginAt
                });

                return ApiResponse<object>.SuccessResponse(employeeData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employees for manager: {Username}", managerUsername);
                return ApiResponse<object>.ErrorResponse("Çalışan listesi alınamadı");
            }
        }

        public async Task<ApiResponse<object>> UpdateEmployeeAsync(int employeeId, object employeeData)
        {
            try
            {
                var employee = await _userRepository.GetByIdAsync(employeeId);
                if (employee == null)
                {
                    return ApiResponse<object>.ErrorResponse("Çalışan bulunamadı");
                }

                // Update logic would go here
                var updatedEmployee = await _userRepository.UpdateAsync(employee);
                
                return ApiResponse<object>.SuccessResponse(updatedEmployee, "Çalışan bilgileri güncellendi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee: {EmployeeId}", employeeId);
                return ApiResponse<object>.ErrorResponse("Çalışan güncellenemedi");
            }
        }

        public async Task<ApiResponse<object>> DeleteEmployeeAsync(int employeeId)
        {
            try
            {
                var employee = await _userRepository.GetByIdAsync(employeeId);
                if (employee == null)
                {
                    return ApiResponse<object>.ErrorResponse("Çalışan bulunamadı");
                }

                await _userRepository.DeleteAsync(employeeId);
                
                return ApiResponse<object>.SuccessResponse(new { message = "Çalışan silindi" }, "Çalışan silindi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee: {EmployeeId}", employeeId);
                return ApiResponse<object>.ErrorResponse("Çalışan silinemedi");
            }
        }
    }
}
