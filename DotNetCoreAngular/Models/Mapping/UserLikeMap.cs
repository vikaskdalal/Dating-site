using DotNetCoreAngular.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetCoreAngular.Models.Mapping
{
    public class UserLikeMap : IEntityTypeConfiguration<UserLike>
    {
        public void Configure(EntityTypeBuilder<UserLike> builder)
        {
            builder.ToTable("UserLikes", "User");
            builder.HasKey(q => new {q.SourceUserId, q.LikedUserId});
            builder.Ignore(i => i.Id);

            builder.HasOne(s => s.SourceUser)
                   .WithMany(s => s.LikedUsers)
                   .HasForeignKey(f => f.SourceUserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(s => s.LikedUser)
                   .WithMany(s => s.LikedByUsers)
                   .HasForeignKey(f => f.LikedUserId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
