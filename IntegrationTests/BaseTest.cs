using AutoMapper;
using DotNetCoreAngular.DAL;
using DotNetCoreAngular.Helpers;
using DotNetCoreAngular.Interfaces;
using DotNetCoreAngular.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;

namespace IntegrationTests
{
    public class BaseTest : IDisposable
    {
        protected virtual TransactionScope TransactionScope => new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        protected ServiceProvider ServiceProvider { get; private set; }

        protected DatabaseContext DatabaseContext => ServiceProvider.GetService<DatabaseContext>();

        protected IUnitOfWork UnitOfWork => ServiceProvider.GetService<IUnitOfWork>();

        protected ITokenService TokenService => ServiceProvider.GetService<ITokenService>();

        protected IPhotoService PhotoService => ServiceProvider.GetService<IPhotoService>();
        
        protected IMapper Mapper => ServiceProvider.GetService<IMapper>();

        public BaseTest()
        {
            RegisterDependency();
        }

        private void RegisterDependency()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddDbContext<DatabaseContext>(options => options.UseSqlServer("Server=.\\SQLEXPRESS;Database=DotNetCoreAngular_local;Trusted_Connection=True;MultipleActiveResultSets=true"),
                    ServiceLifetime.Transient);

            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<ITokenService, TokenService>();
            serviceCollection.AddScoped<IPhotoService, PhotoService>();
            serviceCollection.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            serviceCollection.AddTransient<IConfiguration>(sp =>
            {
                IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                configurationBuilder.AddJsonFile("appsettings.json");
                return configurationBuilder.Build();
            });

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
