namespace OzyParkAdmin.Domain.Tickets;
/// <summary>
/// El tipo de un pasajero.
/// </summary>
/// <param name="Id">El id del tipo de pasajero.</param>
/// <param name="Aka">El aka del tipo de pasajero.</param>
/// <param name="Descripcion">La descripción del tipo de pasajero.</param>
/// <param name="EsActivo">Si el tipo de pasajero está activo.</param>
public sealed record TipoPasajero(int Id, string Aka, string Descripcion, bool EsActivo);
