using Microsoft.AspNetCore.Mvc;
using backend_api.Models;
using backend_api.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace backend_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Store bilgilerini getir
        [HttpGet("info")]
        public async Task<IActionResult> GetStoreInfo()
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

                // Manager'ın store'unu bul
                var store = await _context.Stores.FirstOrDefaultAsync(s => s.Name == manager.StoreName);
                
                if (store == null)
                {
                    // Store bulunamazsa manager'ın bilgilerinden oluştur
                    store = new Store
                    {
                        Name = manager.StoreName,
                        Description = $"Market: {manager.StoreName}",
                        Address = manager.StoreAddress ?? "Adres bilgisi yok",
                        Phone = manager.StorePhone ?? "Telefon bilgisi yok",
                        ManagerId = manager.Id, // Manager ID'yi ekle
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    
                    _context.Stores.Add(store);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { success = true, data = store });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Store bilgilerini güncelle
        [HttpPut("info")]
        public async Task<IActionResult> UpdateStoreInfo([FromBody] StoreUpdateDto dto)
        {
            try
            {
                // Sadece Manager rolündeki kullanıcılar market bilgilerini güncelleyebilir
                var username = User.FindFirst("username")?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { success = false, message = "Token geçersiz" });
                }

                var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (currentUser == null)
                {
                    return NotFound(new { success = false, message = "Kullanıcı bulunamadı" });
                }

                if (currentUser.Role != "Manager")
                {
                    return BadRequest(new { success = false, message = "Sadece Manager rolündeki kullanıcılar market bilgilerini güncelleyebilir" });
                }

                var store = await _context.Stores.FirstOrDefaultAsync(s => s.Name == currentUser.StoreName);
                
                if (store == null)
                {
                    return NotFound(new { success = false, message = "Store bulunamadı" });
                }

                // Market adı benzersizlik kontrolü (sadece değiştiyse)
                if (store.Name != dto.Name)
                {
                    if (await _context.Users.AnyAsync(u => u.StoreName == dto.Name))
                    {
                        return BadRequest(new { success = false, message = "Bu market adı zaten kullanılıyor" });
                    }
                }

                // Telefon format kontrolü (eğer telefon girilmişse)
                if (!string.IsNullOrEmpty(dto.Phone))
                {
                    var phoneRegex = new System.Text.RegularExpressions.Regex(@"^[0-9+\-\s()]{10,15}$");
                    if (!phoneRegex.IsMatch(dto.Phone))
                    {
                        return BadRequest(new { success = false, message = "Geçersiz telefon formatı. Örnek: 0533 123 45 67 veya +90 533 123 45 67" });
                    }

                    // Telefon benzersizlik kontrolü (sadece değiştiyse)
                    if (store.Phone != dto.Phone && await _context.Users.AnyAsync(u => u.StorePhone == dto.Phone))
                    {
                        return BadRequest(new { success = false, message = "Bu telefon numarası zaten kullanılıyor" });
                    }
                }

                // Mevcut market adını al (kullanıcıları güncellemek için)
                var oldStoreName = store.Name;

                store.Name = dto.Name;
                store.Description = dto.Description;
                store.Address = dto.Address;
                store.Phone = dto.Phone;
                store.UpdatedAt = DateTime.UtcNow;

                // Bu markette çalışan tüm kullanıcıların market bilgilerini güncelle
                var storeUsers = await _context.Users
                    .Where(u => u.StoreName == oldStoreName)
                    .ToListAsync();

                foreach (var user in storeUsers)
                {
                    // Sadece market bilgilerini güncelle
                    user.StoreName = dto.Name;
                    user.StoreAddress = dto.Address ?? string.Empty;
                    user.StorePhone = dto.Phone ?? string.Empty;
                }

                await _context.SaveChangesAsync();

                // Güncellenen kullanıcı sayısını log'la
                Console.WriteLine($"✅ Store güncellendi: {dto.Name}");
                Console.WriteLine($"✅ Toplam {storeUsers.Count} kullanıcının market bilgileri güncellendi");

                return Ok(new { 
                    success = true, 
                    message = $"Market bilgileri başarıyla güncellendi. {storeUsers.Count} çalışanın bilgileri güncellendi.", 
                    data = store,
                    updatedUsersCount = storeUsers.Count
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Market adı müsaitlik kontrolü
        [HttpGet("check-name")]
        public async Task<IActionResult> CheckStoreNameAvailability([FromQuery] string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest(new { success = false, message = "Market adı boş olamaz" });
                }

                // Market adı benzersizlik kontrolü
                var isAvailable = !await _context.Users.AnyAsync(u => u.StoreName == name.Trim());
                
                return Ok(new { 
                    success = true, 
                    data = new { 
                        name = name.Trim(),
                        isAvailable = isAvailable 
                    } 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }

    public class StoreUpdateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}
