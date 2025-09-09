using Microsoft.AspNetCore.Mvc;
using backend_api.Services.Interfaces;
using backend_api.DTOs.Responses;
using backend_api.DTOs.Requests;
using backend_api.Data;
using Microsoft.EntityFrameworkCore;

namespace backend_api.Controllers
{
    /// <summary>
    /// User controller - Sadece user işlemlerini yapar
    /// </summary>
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly ApplicationDbContext _context;

        public UserController(IUserService userService, ILogger<UserController> logger, ApplicationDbContext context)
        {
            _userService = userService;
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Kullanıcı profilini getirir
        /// </summary>
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                var username = User.FindFirst("username")?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized("Token geçersiz");
                }

                var result = await _userService.GetUserProfileAsync(username);
                
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
                _logger.LogError(ex, "Error getting user profile");
                return BadRequest(ApiResponse<object>.ErrorResponse("Profil bilgileri alınamadı"));
            }
        }

        /// <summary>
        /// Çalışan listesini getirir (Manager için)
        /// </summary>
        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var username = User.FindFirst("username")?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized("Token geçersiz");
                }

                var result = await _userService.GetEmployeesAsync(username);
                
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
                _logger.LogError(ex, "Error getting employees");
                return BadRequest(ApiResponse<object>.ErrorResponse("Çalışan listesi alınamadı"));
            }
        }

        /// <summary>
        /// Çalışan bilgilerini günceller
        /// </summary>
        [HttpPut("employee/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateProfileRequest request)
        {
            try
            {
                var result = await _userService.UpdateEmployeeAsync(id, request);
                
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
                _logger.LogError(ex, "Error updating employee: {EmployeeId}", id);
                return BadRequest(ApiResponse<object>.ErrorResponse("Çalışan güncellenemedi"));
            }
        }

        /// <summary>
        /// Çalışanı siler
        /// </summary>
        [HttpDelete("employee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var result = await _userService.DeleteEmployeeAsync(id);
                
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
                _logger.LogError(ex, "Error deleting employee: {EmployeeId}", id);
                return BadRequest(ApiResponse<object>.ErrorResponse("Çalışan silinemedi"));
            }
        }

        /// <summary>
        /// Employee'nin manager bilgilerini getirir
        /// </summary>
        [HttpGet("manager-info")]
        public async Task<IActionResult> GetManagerInfo()
        {
            var username = User.FindFirst("username")?.Value;
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized("Token geçersiz");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    return Unauthorized("Kullanıcı bulunamadı");
                }

                // Employee ise manager bilgilerini getir
                if (user.Role == "Employee" && !string.IsNullOrEmpty(user.StoreName))
                {
                    var manager = await _context.Users
                        .FirstOrDefaultAsync(u => u.Role == "Manager" && u.StoreName == user.StoreName);
                    
                    if (manager != null)
                    {
                        var managerInfo = new
                        {
                            id = manager.Id,
                            username = manager.Username,
                            firstName = manager.FirstName,
                            lastName = manager.LastName,
                            email = manager.Email,
                            phone = manager.StorePhone,
                            storeName = manager.StoreName,
                            storeAddress = manager.StoreAddress,
                            storePhone = manager.StorePhone
                        };

                        return Ok(new { success = true, data = managerInfo });
                    }
                    else
                    {
                        return NotFound(new { success = false, message = "Manager bulunamadı" });
                    }
                }
                else
                {
                    return BadRequest(new { success = false, message = "Sadece Employee'ler manager bilgilerini alabilir" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting manager info for user: {Username}", username ?? "unknown");
                return BadRequest(new { success = false, message = "Manager bilgileri alınamadı: " + ex.Message });
            }
        }
    }
} 
