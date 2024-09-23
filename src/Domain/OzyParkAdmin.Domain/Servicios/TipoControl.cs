namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// El tipo de control de un servicio.
/// </summary>
/// <param name="Id">El id del tipo de control.</param>
/// <param name="Aka">El aka del tipo de control.</param>
/// <param name="EsActivo">Si el tipo de control está activo.</param>
public sealed record TipoControl(int Id, string Aka, bool EsActivo);
