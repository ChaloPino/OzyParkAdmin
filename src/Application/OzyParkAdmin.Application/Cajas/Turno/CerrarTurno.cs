using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Cajas;
using System.Collections.Immutable;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Cajas.Turno;

/// <summary>
/// Cierra el turno de una caja.
/// </summary>
/// <param name="DiaId">El id del día.</param>
/// <param name="Id">El id del turno.</param>
/// <param name="User">El usuario que realiza el cierre.</param>
/// <param name="RegularizacionEfectivo">El monto regularizado de efectivo.</param>
/// <param name="RegularizacionMontoTransbank">El monto regularizado de transban.</param>
/// <param name="Comentario">El comentario de cierre.</param>
/// <param name="Movimientos">Los detalles de movimientos de la caja.</param>
public sealed record CerrarTurno(
    Guid DiaId,
    Guid Id,
    ClaimsPrincipal User,
    decimal RegularizacionEfectivo,
    decimal RegularizacionMontoTransbank,
    string Comentario,
    ImmutableArray<DetalleTurnoInfo> Movimientos) : ICommand<TurnoCajaInfo>;
