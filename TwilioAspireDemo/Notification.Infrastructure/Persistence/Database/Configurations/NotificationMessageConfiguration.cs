using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.Enums;
using Notification.Domain.MessageLogs.ValueObjects;

namespace Notification.Infrastructure.Persistence.Database.Configurations;

internal class NotificationMessageConfiguration : IEntityTypeConfiguration<NotificationMessage>
{
    public void Configure(EntityTypeBuilder<NotificationMessage> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x =>
            x.Value,
            x => new NotificationMessageId(x))
            .IsRequired();

        builder.Property(x => x.Content)
            .HasMaxLength(1024)
            .IsUnicode(true)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion(new EnumToStringConverter<MessageStatus>())
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Recipient)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Channel)
            .HasConversion(new EnumToStringConverter<MessageChannel>())
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}
