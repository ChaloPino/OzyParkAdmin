using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Franquicias;

namespace OzyParkAdmin.Domain.PuntosVenta;

/// <summary>
/// Entidad punto de venta.
/// </summary>
/// <param name="Id">El id del punto de venta.</param>
/// <param name="FranquiciaId">El id de la franquicia.</param>
/// <param name="Aka">El aka del punto de venta.</param>
/// <param name="Nombre">El nombre del punto de venta.</param>
/// <param name="EsActivo">Si el punto de venta está activo.</param>
public sealed record PuntoVenta(int Id, int FranquiciaId, string Aka, string Nombre, bool EsActivo);