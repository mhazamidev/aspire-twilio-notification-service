using Domain.SeedWork;
using Microsoft.Extensions.Logging;
using Notification.Infrastructure.Persistence.Database;

namespace Notification.Infrastructure.Persistence.UOW;

public class UnitOfWork : IUnitOfWork
{
    protected readonly NotificationDbContext dbContext;
    private readonly ILogger<NotificationDbContext> logger;

    public UnitOfWork(NotificationDbContext dbContext, ILogger<NotificationDbContext> logger)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        this.logger = logger;
    }

    public int Commit()
    {
        try
        {
            return dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while committing UnitOfWork");
            throw;
        }
    }
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while committing UnitOfWork");
            throw;
        }
    }


    public Task RollbackAsync()
    {
        dbContext.ChangeTracker.Clear();
        return Task.CompletedTask;
    }
}
