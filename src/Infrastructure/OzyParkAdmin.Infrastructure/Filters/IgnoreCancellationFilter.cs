using MassTransit;
using Microsoft.Extensions.Logging;

namespace OzyParkAdmin.Infrastructure.Filters;

/// <summary>
/// Filtro para ignorar todas las excepciones de <see cref="OperationCanceledException"/>.
/// </summary>
public class IgnoreCancellationFilter<T> : IFilter<SendContext<T>>
    where T : class
{
    private readonly ILogger<IgnoreCancellationFilter<T>> _logger;

    /// <summary>
    /// Crea una nueva instancia de <see cref="IgnoreCancellationFilter{T}"/>.
    /// </summary>
    public IgnoreCancellationFilter(ILogger<IgnoreCancellationFilter<T>> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    /// <inheritdoc/>
    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("IgnoreCancellationFilter");
    }

    /// <inheritdoc/>
    public async Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
    {
        try
        {
            await next.Send(context);
        }
        catch (OperationCanceledException exception)
        {
            _logger.LogDebug(exception, "Request canceled for {MessageType}", typeof(T).Name);
        }
    }
}
