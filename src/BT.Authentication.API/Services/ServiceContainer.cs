using BT.Authentication.API.Data;
using BT.Authentication.API.Repositories;
using BT.Shared.DI;
using BT.Shared.Domain;
using BT.Shared.Services.AuthService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BT.Authentication.API.Services
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {

            SharedServiceContainer.AddSharedServices<ApplicationDbContext>(services, config, config["MySerilog:FileName"]!);

            //var connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            //  Register JWT Service
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = config["Authentication:Issuer"],
                    ValidAudience = config["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Authentication:Key"]!))
                };
            });


            //  DI
            services.AddScoped<IJWTUtilities, JWTUtilities>();
            services.AddScoped<IAccount, AccountRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            // Registe middleware
            //  Global Exception: hadles errors
            //  Restrict client access to  API Gateway
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
