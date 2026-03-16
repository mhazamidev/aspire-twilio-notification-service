using MediatR;
using Notification.Infrastructure.Persistence.UOW;

namespace Notification.Application.Pipelines;

public class TransactionBehaviour<TRequest, TResponse>(INotificationUnitofWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await next();

            await unitOfWork.CommitAsync(cancellationToken);

            return response;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }
}