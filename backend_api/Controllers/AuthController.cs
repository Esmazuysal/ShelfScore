using Microsoft.AspNetCore.Mvc;
using backend_api.DTOs.Requests;
using backend_api.DTOs.Responses;
using backend_api.Services.Interfaces;

namespace backend_api.Controllers
{
    /// <summary>
    /// Authentication controller - Sadece authentication işlemlerini yapar
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Kullanıcı girişi yapar
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _authService.LoginAsync(request);
                
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error for user: {Username}", request.Username);
                return BadRequest(ApiResponse<object>.ErrorResponse("Giriş yapılırken hata oluştu"));
            }
        }

        /// <summary>
        /// Manager hesabı oluşturur
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var result = await _authService.RegisterManagerAsync(request);
                
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration error for user: {Username}", request.Username);
                return BadRequest(ApiResponse<object>.ErrorResponse("Kayıt yapılırken hata oluştu"));
            }
        }

        /// <summary>
        /// Çalışan hesabı oluşturur
        /// </summary>
        [HttpPost("create-employee")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest request)
        {
            try
            {
                var username = User.FindFirst("username")?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized("Token geçersiz");
                }

                var result = await _authService.CreateEmployeeAsync(request, username);
                
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Employee creation error for user: {Username}", request.Username);
                return BadRequest(ApiResponse<object>.ErrorResponse("Çalışan hesabı oluşturulurken hata oluştu"));
            }
        }

        /// <summary>
        /// Çalışan hesabını siler
        /// </summary>
        [HttpDelete("employee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var result = await _authService.DeleteEmployeeAsync(id);
                
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Employee deletion error for id: {Id}", id);
                return BadRequest(ApiResponse<object>.ErrorResponse("Çalışan silinirken hata oluştu"));
            }
        }
    }
}
