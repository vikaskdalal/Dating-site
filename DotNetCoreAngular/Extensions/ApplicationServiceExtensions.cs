using DotNetCoreAngular.DAL;
using DotNetCoreAngular.Helpers;
using DotNetCoreAngular.Interfaces;
using DotNetCoreAngular.Services;

namespace DotNetCoreAngular.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            return services;
        }
    }
}
