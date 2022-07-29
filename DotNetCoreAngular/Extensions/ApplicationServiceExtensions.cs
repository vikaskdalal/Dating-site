using DotNetCoreAngular.DAL;
using DotNetCoreAngular.Helpers;
using DotNetCoreAngular.Interfaces;
using DotNetCoreAngular.Services;
using DotNetCoreAngular.SignalR;

namespace DotNetCoreAngular.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<PresenceTracker>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            return services;
        }
    }
}
