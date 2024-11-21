using MassTransit;
using MassTransit.Mediator;
using System.Runtime.ExceptionServices;

namespace OzyParkAdmin.Application.Shared;
public static class MediatorExtensions
{
    /// <summary>
    /// Sends a request, with the specified response type, and awaits the response.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request">The request message</param>
    /// <param name="cancellationToken"></param>
    /// <param name="timeout"></param>
    /// <typeparam name="T">The response type</typeparam>
    /// <returns>The response object</returns>
    public static async Task<T> SendRequestWithCancellation<T>(
        this IMediator mediator,
        Request<T> request,
        CancellationToken cancellationToken = default,
        RequestTimeout timeout = default)
        where T : class
    {
        try
        {
            return await mediator.SendRequest(request, cancellationToken, timeout);
        }
        catch (OperationCanceledException exception)
        {
            if (exception.InnerException != null)
            {
                var dispatchInfo = ExceptionDispatchInfo.Capture(exception.InnerException);

                dispatchInfo.Throw();
            }

            throw;
        }
    }
}
