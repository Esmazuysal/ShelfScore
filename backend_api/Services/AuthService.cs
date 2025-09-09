using backend_api.DTOs.Requests;
using backend_api.DTOs.Responses;
using backend_api.Models;
using backend_api.Repositories.Interfaces;
using backend_api.Services.Interfaces;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace backend_api.Services
{
    /// <summary>
    /// Authentication service implementation
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepository,
            IStoreRepository storeRepository,
            IJwtService jwtService,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _storeRepository = storeRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<ApiResponse<object>> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(request.Username);
                if (user == null)
                {
                    return ApiResponse<object>.ErrorResponse("Kullanıcı adı veya şifre hatalı");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                {
                    return ApiResponse<object>.ErrorResponse("Kullanıcı adı veya şifre hatalı");
                }

                var token = _jwtService.GenerateToken(user);
                
                _logger.LogInformation("User logged in successfully: {Username}", user.Username);
                
                return ApiResponse<object>.SuccessResponse(new { token, user });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error for user: {Username}", request.Username);
                return ApiResponse<object>.ErrorResponse("Giriş yapılırken hata oluştu");
            }
        }

        public async Task<ApiResponse<object>> RegisterManagerAsync(RegisterRequest request)
        {
            try
            {
                // Validation
                if (request.Role != "Manager")
                {
                    return ApiResponse<object>.ErrorResponse("Sadece Manager hesabı oluşturulabilir");
                }

                if (!await _userRepository.IsUsernameUniqueAsync(request.Username))
                {
                    return ApiResponse<object>.ErrorResponse("Bu kullanıcı adı zaten kullanılıyor");
                }

                if (!await _userRepository.IsEmailUniqueAsync(request.Email))
                {
                    return ApiResponse<object>.ErrorResponse("Bu email adresi zaten kullanılıyor");
                }

                if (!await _userRepository.IsStoreNameUniqueAsync(request.StoreName))
                {
                    return ApiResponse<object>.ErrorResponse("Bu market adı zaten kullanılıyor");
                }

                // Create store first
                var store = new Store
                {
                    Name = request.StoreName,
                    Description = $"Market: {request.StoreName}",
                    Address = request.StoreAddress ?? string.Empty,
                    Phone = request.StorePhone ?? string.Empty,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdStore = await _storeRepository.CreateAsync(store);

                // Create user
                var user = new User
                {
                    Username = request.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    FirstName = request.FirstName ?? string.Empty,
                    LastName = request.LastName ?? string.Empty,
                    Email = request.Email,
                    Role = request.Role,
                    StoreName = request.StoreName,
                    StoreAddress = request.StoreAddress ?? string.Empty,
                    StorePhone = request.StorePhone ?? string.Empty,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdUser = await _userRepository.AddAsync(user);
                
                _logger.LogInformation("Manager and store registered successfully: {Username}, Store: {StoreName}", user.Username, store.Name);
                
                return ApiResponse<object>.SuccessResponse(new { user = createdUser, store = createdStore }, "Manager hesabı ve market başarıyla oluşturuldu");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Manager registration error for: {Username}", request.Username);
                return ApiResponse<object>.ErrorResponse("Manager hesabı oluşturulurken hata oluştu");
            }
        }

        public async Task<ApiResponse<object>> CreateEmployeeAsync(CreateEmployeeRequest request, string managerUsername)
        {
            try
            {
                // Manager'ı bul ve store'unu al
                var manager = await _userRepository.GetByUsernameAsync(managerUsername);
                if (manager == null || manager.Role != "Manager")
                {
                    return ApiResponse<object>.ErrorResponse("Manager yetkisi gerekli");
                }

                if (!await _userRepository.IsUsernameUniqueAsync(request.Username))
                {
                    return ApiResponse<object>.ErrorResponse("Bu kullanıcı adı zaten kullanılıyor");
                }

                var user = new User
                {
                    Username = request.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    FirstName = request.FirstName ?? string.Empty,
                    LastName = request.LastName ?? string.Empty,
                    Email = request.Email,
                    Role = "Employee",
                    StoreName = manager.StoreName, // Manager'ın store'unu kullan
                    DepartmentName = request.DepartmentName ?? string.Empty,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdUser = await _userRepository.AddAsync(user);
                
                _logger.LogInformation("Employee created successfully: {Username} for store: {StoreName}", user.Username, manager.StoreName);
                
                return ApiResponse<object>.SuccessResponse(createdUser, "Çalışan hesabı başarıyla oluşturuldu");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Employee creation error for: {Username}", request.Username);
                return ApiResponse<object>.ErrorResponse("Çalışan hesabı oluşturulurken hata oluştu");
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

                if (employee.Role != "Employee")
                {
                    return ApiResponse<object>.ErrorResponse("Sadece çalışan hesapları silinebilir");
                }

                await _userRepository.DeleteAsync(employeeId);
                
                _logger.LogInformation("Employee deleted successfully: {Username}", employee.Username);
                
                return ApiResponse<object>.SuccessResponse(new { message = "Çalışan başarıyla silindi" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Employee deletion error for id: {EmployeeId}", employeeId);
                return ApiResponse<object>.ErrorResponse("Çalışan silinirken hata oluştu");
            }
        }
    }
}
