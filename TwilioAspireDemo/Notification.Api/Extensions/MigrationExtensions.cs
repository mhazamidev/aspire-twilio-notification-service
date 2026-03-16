using Microsoft.EntityFrameworkCore;
using Notification.Infrastructure.Persistence.Database;

namespace Identity.API.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

        await db.Database.MigrateAsync();
    }
}
