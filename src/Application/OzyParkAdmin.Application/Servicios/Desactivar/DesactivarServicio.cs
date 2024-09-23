namespace OzyParkAdmin.Application.Servicios.Desactivar;

/// <summary>
/// Desactiva un servicio.
/// </summary>
/// <param name="ServicioId">El id del servicio.</param>
public sealed record DesactivarServicio(int ServicioId) : IServicioStateChangeable;
