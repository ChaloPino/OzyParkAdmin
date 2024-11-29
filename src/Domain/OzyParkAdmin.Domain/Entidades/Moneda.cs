namespace OzyParkAdmin.Domain.Entidades;

/// <summary>
/// La moneda.
/// </summary>
/// <param name="Id">El id de la moneda.</param>
/// <param name="Abreviacion">La abreviación de la moneda.</param>
/// <param name="Aka">El aka de la moneda.</param>
/// <param name="Nombre">El nombre de la moneda.</param>
/// <param name="Descripcion">La descripción de la moneda.</param>
/// <param name="Precision">La precisión de la moneda.</param>
public sealed record Moneda(int Id, string Abreviacion, string Aka, string Nombre, string Descripcion, byte Precision);
