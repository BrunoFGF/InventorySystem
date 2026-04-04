using IS.Application.Interfaces;
using IS.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IS.Application.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISupplierService, SupplierService>();

            return services;
        }
    }
}