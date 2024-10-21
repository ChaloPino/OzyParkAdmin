using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Ventas;

namespace OzyParkAdmin.Domain.Tickets;

/// <summary>
/// Contiene la lógica de negocio del <see cref="Ticket"/>
/// </summary>
public sealed class TicketManager : IBusinessLogic
{
    private readonly ITicketRepository _repository;
    private readonly IUsuarioRepository _usuarioRepository;
    /// <summary>
    /// Crea una nueva instancia de <see cref="TicketManager"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ITicketRepository"/>.</param>
    /// <param name="ventaRepository">El <see cref="IVentaRepository"/>.</param>
    /// <param name="usuarioRepository">El <see cref="IUsuarioRepository"/>.</param>
    public TicketManager(
        ITicketRepository repository,
        IVentaRepository ventaRepository,
        IUsuarioRepository usuarioRepository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(usuarioRepository);
        _repository = repository;
        _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Marca la reimpresión de un ticket.
    /// </summary>
    /// <param name="ticketId">El id del ticket a reimprimir</param>
    /// <param name="usuarioInfo">El usuario que solicita la reimpresión.</param>
    /// <param name="ip">La ip desde donde se realizó la solicitud de reimpresión.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de la reimpresión del ticket.</returns>
    public async Task<ResultOf<Ticket>> ReprintTicketAsync(string ticketId, UsuarioInfo usuarioInfo, string ip, CancellationToken cancellationToken)
    {
        Ticket? ticket = await _repository.FindByIdAsync(ticketId, cancellationToken);

        if (ticket is null)
        {
            return new NotFound();
        }

        Usuario? usuario = await _usuarioRepository.FindByIdAsync(usuarioInfo.Id, cancellationToken);

        if (usuario is null)
        {
            return new NotFound();
        }

        ticket.MarkReprint(usuario, ip);

        return ticket;
    }
}
