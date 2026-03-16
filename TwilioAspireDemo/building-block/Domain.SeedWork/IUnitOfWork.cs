namespace Domain.SeedWork;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
    int Commit();
    Task RollbackAsync();
}
