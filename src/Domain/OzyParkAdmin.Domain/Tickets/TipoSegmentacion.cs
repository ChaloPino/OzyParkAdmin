namespace OzyParkAdmin.Domain.Tickets;

/// <summary>
/// El tipo de segmentación.
/// </summary>
/// <param name="Id">El id del tipo de segmentación.</param>
/// <param name="Aka">El aka del tipo de segmentación.</param>
/// <param name="Descripcion">La descripción del tipo de segmentación.</param>
/// <param name="EsActivo">Si el tipo de segmentación está activo.</param>
public sealed record TipoSegmentacion(int Id, string Aka, string Descripcion, bool EsActivo);
