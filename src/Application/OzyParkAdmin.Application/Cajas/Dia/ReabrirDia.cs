using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Application.Cajas.Dia;

/// <summary>
/// Reapertura el día de una caja.
/// </summary>
/// <param name="DiaId">El id de la caja a reaperturar.</param>
public sealed record ReabrirDia(Guid DiaId) : ICommand<AperturaCajaInfo>;
