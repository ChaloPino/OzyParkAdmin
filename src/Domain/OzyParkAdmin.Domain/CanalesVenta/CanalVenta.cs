namespace OzyParkAdmin.Domain.CanalesVenta;

/// <summary>
/// Entidad canal de venta.
/// </summary>
/// <param name="Id">El id del canal de venta.</param>
/// <param name="Aka">El aka del canal de venta.</param>
/// <param name="Nombre">El nombre del canal de venta.</param>
/// <param name="Texto">El texto a desplegarse.</param>
/// <param name="EsActivo">Si el canal de venta está activo.</param>
public sealed record CanalVenta(int Id, string Aka, string Nombre, string? Texto, bool EsActivo);
