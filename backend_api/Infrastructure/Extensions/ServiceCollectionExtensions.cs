using backend_api.Services;
using backend_api.Services.Interfaces;
using backend_api.Repositories;
using backend_api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace backend_api.Infrastructure.Extensions
{
    /// <summary>
    /// Service collection extension method'ları
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// JWT authentication servislerini ekler
        /// </summary>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "default-secret-key-for-development");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                // Custom token validation
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var jwtValidatorService = context.HttpContext.RequestServices.GetRequiredService<JwtValidatorService>();
                        var token = context.SecurityToken as JwtSecurityToken;
                        
                        if (token != null)
                        {
                            var isValid = await jwtValidatorService.ValidateTokenAsync(token.RawData);
                            if (!isValid)
                            {
                                context.Fail("Token geçersiz - Şifre değiştirilmiş");
                            }
                        }
                    }
                };
            });

            return services;
        }

        /// <summary>
        /// CORS servislerini ekler
        /// </summary>
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            return services;
        }

        /// <summary>
        /// Application servislerini ekler
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<JwtValidatorService>();
            
            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            
            return services;
        }
    }
}
