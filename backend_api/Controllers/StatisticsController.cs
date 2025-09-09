using Microsoft.AspNetCore.Mvc;
using backend_api.Models;
using backend_api.Data;
using Microsoft.EntityFrameworkCore;

namespace backend_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStatistics()
        {
            try
            {
                var username = User.FindFirst("username")?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized("Token geçersiz");
                }

                // Manager'ı bul
                var manager = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (manager == null)
                {
                    return Unauthorized("Kullanıcı bulunamadı");
                }

                // Sadece bu manager'ın çalışan sayısı
                var totalEmployees = await _context.Users
                    .Where(u => u.Role == "Employee" && u.StoreName == manager.StoreName)
                    .CountAsync();

                // Photos tablosundan gerçek veriler çek - Geçici olarak devre dışı
                var totalPhotos = 0;
                var averageScore = 0.0;
                var highestScore = 0;
                var marketScore = 0.0;

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        totalEmployees,
                        totalPhotos,
                        averageScore = Math.Round(averageScore, 1),
                        highestScore = highestScore,
                        marketScore = Math.Round(marketScore, 1)
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("employee-stats")]
        public async Task<IActionResult> GetEmployeeStatistics()
        {
            try
            {
                var employees = await _context.Users
                    .Where(u => u.Role == "Employee")
                    .ToListAsync();

                var employeeStats = new List<object>();

                foreach (var emp in employees)
                {
                    // Photos tablosundan gerçek veriler çek - Geçici olarak devre dışı
                    var userPhotos = new List<object>();
                    var photoCount = 0;
                    var avgScore = 0.0;

                    employeeStats.Add(new
                    {
                        emp.Id,
                        emp.FirstName,
                        emp.LastName,
                        emp.DepartmentName,
                        emp.StoreName,
                        emp.CreatedAt,
                        emp.LastLoginAt,
                        PhotoCount = photoCount,
                        AverageScore = avgScore,
                        LastActivity = emp.LastLoginAt ?? emp.CreatedAt
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = employeeStats
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
