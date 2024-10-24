using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Application.Cajas.Find;

/// <summary>
/// Busca el detalle de la apertura de caja.
/// </summary>
/// <param name="AperturaCajaId">El id de la apertura de caja a buscar.</param>
public sealed record FindAperturaCajaDetalle(Guid AperturaCajaId) : IQuery<AperturaCajaDetalleInfo>;
