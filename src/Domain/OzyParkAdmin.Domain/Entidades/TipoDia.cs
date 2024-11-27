namespace OzyParkAdmin.Domain.Entidades;

/// <summary>
/// El tipo de día.
/// </summary>
/// <param name="Id">El id del tipo de día.</param>
/// <param name="Aka">El aka del tipo de día.</param>
/// <param name="Descripcion">La descripción del tipo de día.</param>
/// <param name="EsActivo">Si está activo.</param>
/// <param name="EsDiaHabil">Si es un día hábil.</param>
public sealed record TipoDia(int Id, string Aka, string Descripcion, bool EsActivo, bool EsDiaHabil);
