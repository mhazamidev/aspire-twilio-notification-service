using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notification.Infrastructure.Persistence.Database.Configurations;

internal class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Payload)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(x => x.RoutingKey)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.IsProcessed)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}
