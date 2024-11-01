using BT.Products.API.Data;
using BT.Products.API.Interface;
using BT.Products.API.Repositories;
using BT.Shared.DI;

namespace BT.Products.API.Services
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            //  DB connectivity
            //  Auth scheme
            SharedServiceContainer.AddSharedServices<ProductDataContext>(services, config, config["MySerilog:FileName"]!);

            // DI
            services.AddScoped<IProduct, ProductRepository>();
            services.AddScoped<ICategory, CategoryRepository>();

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
