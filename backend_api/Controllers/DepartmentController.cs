using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_api.Models;
using backend_api.Data;
using System.ComponentModel.DataAnnotations;
using BCrypt.Net;

namespace backend_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DepartmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/department - Manager'ın departmanlarını getir
    [HttpGet]
    public async Task<IActionResult> GetDepartments()
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
            if (manager == null || manager.Role != "Manager")
            {
                return Unauthorized("Manager yetkisi gerekli");
            }

            // Sadece bu manager'ın departmanlarını getir
            var departments = await _context.Departments
                .Where(d => d.StoreName == manager.StoreName) // Store name'e göre filtrele
                .Select(d => new
                {
                    id = d.Id,
                    name = d.Name,
                    description = d.Description,
                    createdAt = d.CreatedAt,
                    userCount = _context.Users.Count(u => u.DepartmentName == d.Name && u.StoreName == manager.StoreName),
                    averageScore = 0.0 // Şimdilik 0, sonra photo score'ları eklenecek
                })
                .ToListAsync();

            return Ok(new { 
                success = true, 
                data = departments 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "Departmanlar alınamadı: " + ex.Message 
            });
        }
    }

    // GET: api/department/{id} - Belirli bir departmanı getir
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDepartment(int id)
    {
        try
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                return NotFound(new { 
                    success = false, 
                    message = "Departman bulunamadı" 
                });
            }

            var userCount = await _context.Users
                .CountAsync(u => u.DepartmentName == department.Name);

            var result = new
            {
                id = department.Id,
                name = department.Name,
                description = department.Description,
                createdAt = department.CreatedAt,
                userCount = userCount,
                averageScore = 0.0
            };

            return Ok(new { 
                success = true, 
                data = result 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "Departman alınamadı: " + ex.Message 
            });
        }
    }

    // POST: api/department - Yeni departman ekle
    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentRequest request)
    {
        try
        {
            // Token'dan username'i al
            var username = User.FindFirst("username")?.Value;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Token geçersiz");
            }

            // Manager yetkisi kontrolü
            var manager = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Role == "Manager");
            if (manager == null)
            {
                return Unauthorized("Manager yetkisi gerekli");
            }

            // Departman adı kontrolü (aynı store içinde)
            if (await _context.Departments.AnyAsync(d => d.Name == request.Name && d.StoreName == manager.StoreName))
            {
                return BadRequest(new { 
                    success = false, 
                    message = "Bu departman adı zaten kullanılıyor" 
                });
            }

            var department = new Department
            {
                Name = request.Name,
                Description = request.Description,
                StoreName = manager.StoreName, // Manager'ın store'unu ata
                CreatedAt = DateTime.UtcNow
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            var result = new
            {
                id = department.Id,
                name = department.Name,
                description = department.Description,
                createdAt = department.CreatedAt,
                userCount = 0,
                averageScore = 0.0
            };

            return Ok(new { 
                success = true, 
                message = "Departman başarıyla oluşturuldu",
                data = result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "Departman oluşturulamadı: " + ex.Message 
            });
        }
    }

    // PUT: api/department/{id} - Departman güncelle
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentRequest request)
    {
        try
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                return NotFound(new { 
                    success = false, 
                    message = "Departman bulunamadı" 
                });
            }

            // Departman adı değişiyorsa, yeni adın benzersiz olduğunu kontrol et
            if (request.Name != department.Name && 
                await _context.Departments.AnyAsync(d => d.Name == request.Name))
            {
                return BadRequest(new { 
                    success = false, 
                    message = "Bu departman adı zaten kullanılıyor" 
                });
            }

            department.Name = request.Name;
            department.Description = request.Description;

            await _context.SaveChangesAsync();

            var result = new
            {
                id = department.Id,
                name = department.Name,
                description = department.Description,
                createdAt = department.CreatedAt,
                userCount = await _context.Users.CountAsync(u => u.DepartmentName == department.Name),
                averageScore = 0.0
            };

            return Ok(new { 
                success = true, 
                message = "Departman başarıyla güncellendi",
                data = result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "Departman güncellenemedi: " + ex.Message 
            });
        }
    }

    // DELETE: api/department/{id} - Departman sil
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        try
        {
            // Token'dan username'i al
            var username = User.FindFirst("username")?.Value;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Token geçersiz");
            }

            // Manager yetkisi kontrolü
            var manager = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Role == "Manager");
            if (manager == null)
            {
                return Unauthorized("Manager yetkisi gerekli");
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                return NotFound(new { 
                    success = false, 
                    message = "Departman bulunamadı" 
                });
            }

            // Departmanda çalışan var mı kontrol et
            var userCount = await _context.Users
                .CountAsync(u => u.DepartmentName == department.Name);

            if (userCount > 0)
            {
                return BadRequest(new { 
                    success = false, 
                    message = "Bu departmanda çalışan bulunduğu için silinemez" 
                });
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return Ok(new { 
                success = true, 
                message = "Departman başarıyla silindi" 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "Departman silinemedi: " + ex.Message 
            });
        }
    }
}

// DTOs
public class CreateDepartmentRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
}

public class UpdateDepartmentRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
}
