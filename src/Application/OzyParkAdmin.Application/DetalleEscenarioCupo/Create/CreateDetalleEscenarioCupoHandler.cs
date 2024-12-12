using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.DetalleEscenarioCupo.Create;
public sealed class CreateDetalleEscenarioCupoHandler : CommandHandler<CreateDetalleEscenarioCupo>
{
    public CreateDetalleEscenarioCupoHandler(ILogger<CreateDetalleEscenarioCupoHandler> logger) : base(logger)
    {
    }

    protected override Task<SuccessOrFailure> ExecuteAsync(CreateDetalleEscenarioCupo command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
