using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notification.Infrastructure.Persistence.Database.Configurations;

internal class ProcessedMessageConfiguration : IEntityTypeConfiguration<ProcessedMessage>
{
    public void Configure(EntityTypeBuilder<ProcessedMessage> builder)
    {
        builder.HasKey(x => x.MessageId);

        builder.Property(x => x.ProcessedAt)
            .IsRequired();
    }
}
