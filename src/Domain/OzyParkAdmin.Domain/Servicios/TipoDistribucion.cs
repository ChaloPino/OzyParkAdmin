namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// El tipo de distribución de un servicio.
/// </summary>
/// <param name="Id">El id del tipo de distribución.</param>
/// <param name="Descripcion">La descripción del tipo de distribución.</param>
/// <param name="EsActivo">Si el tipo de distribución está activo.</param>
public sealed record TipoDistribucion(int Id, string Descripcion, bool EsActivo);
