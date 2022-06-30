using DotNetCoreAngular.Helpers;
using DotNetCoreAngular.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography;
using System.Text;

namespace DotNetCoreAngular.Models.Mapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "User");
            builder.HasKey(q => q.Id);

            var hmac = new HMACSHA512();
            builder.HasData
                (
                    new User
                    {
                        Id = 1,
                        Email = "vikaskdalal@gmail.com",
                        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Password")),
                        PasswordSalt = hmac.Key,
                        DateOfBirth = DateTime.Now,
                        Name = "Vikas",
                        Username = UserHelper.CreateUsername("vikaskdalal@gmail.com")
                    }
                );
        }
    }
}
