using DotNetCoreAngular.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetCoreAngular.Models.Mapping
{
    public class GroupMap : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("Groups", "SignalR");
            builder.HasKey(q => q.Name);
            builder.Ignore(i => i.Id);
        }
    }
}
