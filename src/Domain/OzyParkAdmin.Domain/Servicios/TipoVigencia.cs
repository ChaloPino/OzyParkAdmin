namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// El tipo de vigencia de un servicio.
/// </summary>
/// <param name="Id">El id del tipo de vigencia.</param>
/// <param name="Aka">El aka del tipo de vigencia.</param>
/// <param name="Descripcion">La descripcion del tipo de vigencia.</param>
/// <param name="EsActivo">Si el tipo de vigencia está activo.</param>
public sealed record TipoVigencia(int Id, string Aka, string Descripcion, bool EsActivo);
