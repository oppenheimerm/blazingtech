using BT.Shared.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BT.Shared.DI
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection services,
            IConfiguration config, string fileName) where TContext : DbContext
        {
            //  Add generic database context
            services.AddDbContext<TContext>(option => option.UseSqlServer(
                config.GetConnectionString("BlazingTechConnectionString"), sqloptions =>
                sqloptions.EnableRetryOnFailure()
                ));

            //  Configure serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text",
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day).CreateLogger();

            // Add jwt authentication scheme
            //  05/10/2024
            //  We'll be using bearer tokens
            //JWTAuthenticationScheme.AddJWTAuthenticationScheme(services, config);

            return services;
        }


        /// <summary>
        /// Setup global <see cref="GlobalException"/> middleware and restrict client access
        /// with <see cref="APIGatewayListener"/>
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            // Use our global exception
            app.UseMiddleware<GlobalException>();

            //  Register our middleware to block direct API service communication
            //app.UseMiddleware<APIGatewayListener>();

            return app;
        }
    }
}
