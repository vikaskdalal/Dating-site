using DotNetCoreAngular.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetCoreAngular.Models.Mapping
{
    public class ConnectionMap : IEntityTypeConfiguration<Connection>
    {
        public void Configure(EntityTypeBuilder<Connection> builder)
        {
            builder.ToTable("Connections", "SignalR");
            builder.HasKey(q => q.ConnectionId);
            builder.Ignore(i => i.Id);
        }
    }
}
