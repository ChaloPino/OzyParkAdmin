namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// El sentido que tiene una zona ruta.
/// </summary>
/// <param name="Id">El id del sentido.</param>
/// <param name="Aka">El aka del sentido.</param>
/// <param name="Descripcion">La descripción del sentido.</param>
/// <param name="EsActivo">Si el sentido está activo.</param>
public sealed record Sentido(int Id, string Aka, string Descripcion, bool EsActivo);
