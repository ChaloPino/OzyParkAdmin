namespace OzyParkAdmin.Domain.Plantillas;

/// <summary>
/// La entidad impresora.
/// </summary>
/// <param name="Id">El id de la impresora.</param>
/// <param name="Aka">El aka de la impresora.</param>
/// <param name="Nombre">El nombre de la impresora.</param>
/// <param name="EsActivo">Si la impresora está activa.</param>
public sealed record Impresora(int Id, string Aka, string Nombre, bool EsActivo);
