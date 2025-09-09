using Microsoft.EntityFrameworkCore;
using backend_api.Data;
using backend_api.Infrastructure.Extensions;
using backend_api.Infrastructure.Middleware;
using backend_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// CORS Policy
builder.Services.AddCorsPolicy();

// Application Services
builder.Services.AddApplicationServices();

// ML Service
builder.Services.AddScoped<MLService>();





// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // HTTPS redirect kaldırıldı - Development için

app.UseCors("AllowAll");

// Exception handling middleware
app.UseMiddleware<ExceptionMiddleware>();

// Static files for photo uploads
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Create database on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

app.Run();

