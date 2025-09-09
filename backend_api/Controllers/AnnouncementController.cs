using Microsoft.AspNetCore.Mvc;
using backend_api.Models;
using backend_api.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace backend_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnnouncements()
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

                // Sadece bu manager'ın duyurularını getir
                var announcements = await _context.Announcements
                    .Where(a => a.StoreName == manager.StoreName) // Store name'e göre filtrele
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    data = announcements
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnnouncement([FromBody] CreateAnnouncementRequest request)
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

                var announcement = new Announcement
                {
                    Title = request.Title,
                    Content = request.Content,
                    Priority = request.Priority,
                    Author = request.Author,
                    StoreName = manager.StoreName, // Manager'ın store'una ekle
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Announcements.Add(announcement);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Duyuru başarıyla oluşturuldu",
                    data = announcement
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnnouncement(int id, [FromBody] UpdateAnnouncementRequest request)
        {
            try
            {
                var announcement = await _context.Announcements.FindAsync(id);
                if (announcement == null)
                {
                    return NotFound(new { success = false, message = "Duyuru bulunamadı" });
                }

                announcement.Title = request.Title;
                announcement.Content = request.Content;
                announcement.Priority = request.Priority;
                announcement.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Duyuru başarıyla güncellendi",
                    data = announcement
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            try
            {
                var announcement = await _context.Announcements.FindAsync(id);
                if (announcement == null)
                {
                    return NotFound(new { success = false, message = "Duyuru bulunamadı" });
                }

                _context.Announcements.Remove(announcement);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Duyuru başarıyla silindi"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("mark-as-read/{id}")]
        public async Task<IActionResult> MarkAnnouncementAsRead(int id)
        {
            try
            {
                var username = User.FindFirst("username")?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized("Token geçersiz");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    return NotFound("Kullanıcı bulunamadı");
                }

                var announcement = await _context.Announcements.FindAsync(id);
                if (announcement == null)
                {
                    return NotFound(new { success = false, message = "Duyuru bulunamadı" });
                }

                // Kullanıcının bu duyuruyu daha önce okumadığını kontrol et
                var existingUserAnnouncement = await _context.UserAnnouncements
                    .FirstOrDefaultAsync(ua => ua.UserId == user.Id && ua.AnnouncementId == id);

                if (existingUserAnnouncement == null)
                {
                    // Yeni UserAnnouncement kaydı oluştur
                    var userAnnouncement = new UserAnnouncement
                    {
                        UserId = user.Id,
                        AnnouncementId = id,
                        IsRead = true,
                        ReadAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.UserAnnouncements.Add(userAnnouncement);
                }
                else
                {
                    // Mevcut kaydı güncelle
                    existingUserAnnouncement.IsRead = true;
                    existingUserAnnouncement.ReadAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Duyuru okundu olarak işaretlendi"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadAnnouncementCount()
        {
            try
            {
                var username = User.FindFirst("username")?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized("Token geçersiz");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    return NotFound("Kullanıcı bulunamadı");
                }

                // Kullanıcının okumadığı duyuru sayısını hesapla
                var totalAnnouncements = await _context.Announcements.CountAsync();
                var readAnnouncements = await _context.UserAnnouncements
                    .Where(ua => ua.UserId == user.Id && ua.IsRead)
                    .CountAsync();

                var unreadCount = totalAnnouncements - readAnnouncements;

                return Ok(new
                {
                    success = true,
                    data = new { unreadCount }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }

    public class CreateAnnouncementRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        [Required]
        public string Priority { get; set; } = "medium";
        
        [Required]
        public string Author { get; set; } = string.Empty;
    }

    public class UpdateAnnouncementRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        [Required]
        public string Priority { get; set; } = "medium";
    }
}
