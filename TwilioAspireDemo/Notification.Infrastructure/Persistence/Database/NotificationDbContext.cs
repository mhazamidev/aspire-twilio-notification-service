using Microsoft.EntityFrameworkCore;
using Notification.Domain.MessageLogs.Entities;

namespace Notification.Infrastructure.Persistence.Database;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
    {
    }
    public virtual DbSet<NotificationMessage> Notifications => Set<NotificationMessage>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
