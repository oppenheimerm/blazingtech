using BT.Authentication.API.Data;
using BT.Shared.DI;
using BT.Shared.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BT.Authentication.API.Services
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {

            SharedServiceContainer.AddSharedServices<ApplicationDbContext>(services, config, config["MySerilog:FileName"]!);

            //var connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            //  DI
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
