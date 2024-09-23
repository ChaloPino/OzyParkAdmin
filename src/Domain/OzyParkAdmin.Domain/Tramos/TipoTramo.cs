namespace OzyParkAdmin.Domain.Tramos;

/// <summary>
/// El tipo del tramo.
/// </summary>
/// <param name="Id">El id del tipo de tramo.</param>
/// <param name="Aka">El aka del tipo de tramo.</param>
/// <param name="Descripcion">La descripción del tipo de tramo.</param>
/// <param name="EsActivo">Si el tipo de tramo está activo.</param>
public sealed record TipoTramo(int Id, string Aka, string Descripcion, bool EsActivo);
