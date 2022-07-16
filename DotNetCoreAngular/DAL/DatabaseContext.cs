using Microsoft.EntityFrameworkCore;
using DotNetCoreAngular.Models.Entity;
using DotNetCoreAngular.Models.Mapping;

namespace DotNetCoreAngular.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new UserLikeMap());
            modelBuilder.ApplyConfiguration(new MessageMap());
            modelBuilder.ApplyConfiguration(new PhotoMap());
        }
    }
}
