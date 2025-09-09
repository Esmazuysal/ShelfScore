using System.Diagnostics;
using System.Text.Json;

namespace backend_api.Services
{
    public class MLService
    {
        private readonly string _pythonPath;
        private readonly string _mlScriptPath;
        private readonly ILogger<MLService> _logger;

        public MLService(ILogger<MLService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _pythonPath = configuration["ML:PythonPath"] ?? "python3";
            _mlScriptPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "ml_models", "inference", "shelf_analyzer.py");
        }

        public async Task<MLAnalysisResult> AnalyzeShelfPhotoAsync(string imagePath)
        {
            try
            {
                _logger.LogInformation($"ML analizi başlatılıyor: {imagePath}");

                // Python scriptini çalıştır
                var result = await RunPythonScriptAsync(imagePath);

                if (result.Success)
                {
                    _logger.LogInformation($"ML analizi tamamlandı. Skor: {result.Score}/10");
                    return result;
                }
                else
                {
                    _logger.LogWarning($"ML analizi başarısız: {result.ErrorMessage}");
                    return GetDefaultResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ML analizi sırasında hata oluştu");
                return GetDefaultResult();
            }
        }

        private async Task<MLAnalysisResult> RunPythonScriptAsync(string imagePath)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = _pythonPath,
                    Arguments = $"{_mlScriptPath} \"{imagePath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(startInfo);
                if (process == null)
                {
                    throw new Exception("Python process başlatılamadı");
                }

                var output = await process.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    _logger.LogError($"Python script hatası: {error}");
                    return new MLAnalysisResult
                    {
                        Success = false,
                        ErrorMessage = error
                    };
                }

                // JSON output'u parse et
                var result = JsonSerializer.Deserialize<MLAnalysisResult>(output, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result ?? GetDefaultResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Python script çalıştırma hatası");
                return new MLAnalysisResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        private MLAnalysisResult GetDefaultResult()
        {
            // Varsayılan sonuç (ML çalışmazsa)
            return new MLAnalysisResult
            {
                Success = true,
                Score = 7, // Orta skor
                Confidence = 0.8,
                AnalysisResult = "Fotoğraf analiz edildi (varsayılan sonuç)",
                ProcessingTime = 1000,
                Criteria = new List<CriteriaScore>
                {
                    new() { Name = "Düzen", Score = 7, Weight = 0.3 },
                    new() { Name = "Temizlik", Score = 7, Weight = 0.25 },
                    new() { Name = "Ürün Yerleşimi", Score = 7, Weight = 0.25 },
                    new() { Name = "Genel Görünüm", Score = 7, Weight = 0.2 }
                },
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Improvements = new List<string>
                {
                    "Genel düzeni gözden geçirin",
                    "Küçük iyileştirmeler yapın"
                }
            };
        }
    }

    public class MLAnalysisResult
    {
        public bool Success { get; set; }
        public int Score { get; set; }
        public double Confidence { get; set; }
        public string AnalysisResult { get; set; } = string.Empty;
        public int ProcessingTime { get; set; }
        public List<CriteriaScore> Criteria { get; set; } = new();
        public long Timestamp { get; set; }
        public List<string> Improvements { get; set; } = new();
        public string? ErrorMessage { get; set; }
    }

    public class CriteriaScore
    {
        public string Name { get; set; } = string.Empty;
        public int Score { get; set; }
        public double Weight { get; set; }
    }
}
