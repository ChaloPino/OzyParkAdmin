namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// La agrupación contable.
/// </summary>
/// <param name="Id">El id de la agrupación contable.</param>
/// <param name="FranquiciaId">El id de la franquicia.</param>
/// <param name="Aka">El aka de la agrupación contable.</param>
/// <param name="Nombre">La descripción de la agrupación contable.</param>
/// <param name="EsActivo">Si la agrupación contable está activa.</param>
public sealed record AgrupacionContable(int Id, int FranquiciaId, string Aka, string Nombre, bool EsActivo);
