using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_api.Models;
using backend_api.Data;
using backend_api.Services;
using System.ComponentModel.DataAnnotations;

namespace backend_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly MLService _mlService;

        public PhotoController(ApplicationDbContext context, IWebHostEnvironment environment, MLService mlService)
        {
            _context = context;
            _environment = environment;
            _mlService = mlService;
        }

        // GET: api/photo - Kullanıcının fotoğraflarını getir
        [HttpGet]
        public async Task<IActionResult> GetPhotos()
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

                var photos = await _context.Photos
                    .Where(p => p.UserId == user.Id)
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new
                    {
                        id = p.Id,
                        fileName = p.FileName,
                        filePath = p.FilePath,
                        description = p.Description,
                        createdAt = p.CreatedAt,
                        departmentName = p.DepartmentName,
                        score = p.Score,
                        analysisResult = p.AnalysisResult,
                        isProcessed = p.IsProcessed
                    })
                    .ToListAsync();

                return Ok(new { 
                    success = true, 
                    data = photos 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Fotoğraflar alınamadı: " + ex.Message 
                });
            }
        }

        // GET: api/photo/all-employees - Tüm çalışanların fotoğraflarını getir (Manager için)
        [HttpGet("all-employees")]
        public async Task<IActionResult> GetAllEmployeePhotos()
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

                // Sadece Manager'lar bu endpoint'i kullanabilir
                if (user.Role != "Manager")
                {
                    return Forbid("Bu işlem için yetkiniz yok");
                }

                var photos = await _context.Photos
                    .Include(p => p.User)
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new
                    {
                        id = p.Id,
                        fileName = p.FileName,
                        filePath = p.FilePath,
                        description = p.Description,
                        createdAt = p.CreatedAt,
                        departmentName = p.DepartmentName,
                        score = p.Score,
                        analysisResult = p.AnalysisResult,
                        isProcessed = p.IsProcessed,
                        employeeName = $"{p.User.FirstName} {p.User.LastName}",
                        employeeUsername = p.User.Username
                    })
                    .ToListAsync();

                return Ok(new { 
                    success = true, 
                    data = photos 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Fotoğraflar alınamadı: " + ex.Message 
                });
            }
        }

        // GET: api/photo/employee/{employeeId} - Belirli bir çalışanın fotoğraflarını getir (Manager için)
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetEmployeePhotos(int employeeId)
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

                // Sadece Manager'lar bu endpoint'i kullanabilir
                if (user.Role != "Manager")
                {
                    return Forbid("Bu işlem için yetkiniz yok");
                }

                var photos = await _context.Photos
                    .Where(p => p.UserId == employeeId)
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new
                    {
                        id = p.Id,
                        fileName = p.FileName,
                        filePath = p.FilePath,
                        description = p.Description,
                        createdAt = p.CreatedAt,
                        departmentName = p.DepartmentName,
                        score = p.Score,
                        analysisResult = p.AnalysisResult,
                        isProcessed = p.IsProcessed
                    })
                    .ToListAsync();

                return Ok(new { 
                    success = true, 
                    data = photos 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Çalışan fotoğrafları alınamadı: " + ex.Message 
                });
            }
        }

        // GET: api/photo/{id} - Belirli bir fotoğrafı getir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhoto(int id)
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

                var photo = await _context.Photos
                    .FirstOrDefaultAsync(p => p.Id == id && p.UserId == user.Id);

                if (photo == null)
                {
                    return NotFound("Fotoğraf bulunamadı");
                }

                var result = new
                {
                    id = photo.Id,
                    fileName = photo.FileName,
                    filePath = photo.FilePath,
                    description = photo.Description,
                    createdAt = photo.CreatedAt,
                    departmentName = photo.DepartmentName,
                    score = photo.Score,
                    analysisResult = photo.AnalysisResult,
                    isProcessed = photo.IsProcessed
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
                    message = "Fotoğraf alınamadı: " + ex.Message 
                });
            }
        }

        // POST: api/photo - Yeni fotoğraf yükle
        [HttpPost]
        public async Task<IActionResult> UploadPhoto([FromForm] UploadPhotoRequest request)
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

                        if (request.ImageFile == null || request.ImageFile.Length == 0)
        {
            return BadRequest(new { 
                success = false, 
                message = "Fotoğraf dosyası gerekli" 
            });
        }

        // Debug log
        Console.WriteLine($"Photo upload attempt - File: {request.ImageFile.FileName}, Size: {request.ImageFile.Length}, ContentType: {request.ImageFile.ContentType}");
        Console.WriteLine($"Request fields: {string.Join(", ", request.GetType().GetProperties().Select(p => $"{p.Name}={p.GetValue(request)}"))}");
        Console.WriteLine($"Form data count: {Request.Form.Count}");
        foreach (var field in Request.Form)
        {
            Console.WriteLine($"Form field: {field.Key} = {field.Value}");
        }
        
        // Dosya uzantısı debug
        var fileExtension = Path.GetExtension(request.ImageFile.FileName);
        Console.WriteLine($"File extension: '{fileExtension}' (Length: {fileExtension?.Length ?? 0})");
        Console.WriteLine($"File name analysis: '{request.ImageFile.FileName}' -> Extension: '{fileExtension}'");

                        // Dosya uzantısını kontrol et - Geçici olarak kaldırıldı
        Console.WriteLine($"File extension check: '{fileExtension}'");
        
        // Geçici olarak tüm dosya türlerini kabul et
        // if (!allowedExtensions.Contains(fileExtension))
        // {
        //     return BadRequest(new { 
        //         success = false, 
        //         message = "Sadece JPG, PNG ve GIF dosyaları kabul edilir" 
        //     });
        // }

                // Dosya boyutunu kontrol et (5MB)
                if (request.ImageFile.Length > 5 * 1024 * 1024)
                {
                    return BadRequest(new { 
                        success = false, 
                        message = "Dosya boyutu 5MB'dan büyük olamaz" 
                    });
                }

                // Benzersiz dosya adı oluştur
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                
                // Uploads klasörü yoksa oluştur
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, fileName);

                // Dosyayı kaydet
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                // ML Analizi yap
                Console.WriteLine("🤖 ML analizi başlatılıyor...");
                var mlResult = await _mlService.AnalyzeShelfPhotoAsync(filePath);
                Console.WriteLine($"🤖 ML analizi tamamlandı. Skor: {mlResult.Score}/10");

                // Database'e kaydet
                var photo = new Photo
                {
                    FileName = request.ImageFile.FileName,
                    FilePath = $"/uploads/{fileName}",
                    Description = request.Description,
                    UserId = user.Id,
                    DepartmentName = user.DepartmentName,
                    CreatedAt = DateTime.UtcNow,
                    Score = mlResult.Score,
                    AnalysisResult = mlResult.AnalysisResult,
                    IsProcessed = mlResult.Success
                };

                _context.Photos.Add(photo);
                try
                {
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"✅ Photo saved to database successfully. ID: {photo.Id}");
                }
                catch (Exception dbEx)
                {
                    Console.WriteLine($"❌ Database save error: {dbEx.Message}");
                    Console.WriteLine($"❌ Inner exception: {dbEx.InnerException?.Message}");
                    Console.WriteLine($"❌ Stack trace: {dbEx.StackTrace}");
                    throw;
                }

                var result = new
                {
                    id = photo.Id,
                    fileName = photo.FileName,
                    filePath = photo.FilePath,
                    description = photo.Description,
                    createdAt = photo.CreatedAt,
                    departmentName = photo.DepartmentName,
                    score = photo.Score,
                    analysisResult = photo.AnalysisResult,
                    isProcessed = photo.IsProcessed
                };

                return Ok(new { 
                    success = true, 
                    message = "Fotoğraf başarıyla yüklendi",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Fotoğraf yüklenemedi: " + ex.Message 
                });
            }
        }

        // DELETE: api/photo/{id} - Fotoğraf sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int id)
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

                var photo = await _context.Photos
                    .FirstOrDefaultAsync(p => p.Id == id && p.UserId == user.Id);

                if (photo == null)
                {
                    return NotFound("Fotoğraf bulunamadı");
                }

                // Dosyayı fiziksel olarak sil
                var filePath = Path.Combine(_environment.WebRootPath, photo.FilePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Database'den sil
                _context.Photos.Remove(photo);
                await _context.SaveChangesAsync();

                return Ok(new { 
                    success = true, 
                    message = "Fotoğraf başarıyla silindi" 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Fotoğraf silinemedi: " + ex.Message 
                });
            }
        }
    }

    // DTOs
    public class UploadPhotoRequest
    {
        [Required]
        public IFormFile ImageFile { get; set; } = null!;
        
        public string? Description { get; set; }
    }
}
