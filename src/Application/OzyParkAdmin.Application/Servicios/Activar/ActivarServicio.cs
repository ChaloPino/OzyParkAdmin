namespace OzyParkAdmin.Application.Servicios.Activar;

/// <summary>
/// Activa el servicio.
/// </summary>
/// <param name="ServicioId">El id del servicio.</param>
public sealed record ActivarServicio(int ServicioId) : IServicioStateChangeable;
