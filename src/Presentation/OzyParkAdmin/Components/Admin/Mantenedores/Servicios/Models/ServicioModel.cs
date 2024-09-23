namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;

/// <summary>
/// Modelo del servicio.
/// </summary>
/// <param name="Id">El id del servicio.</param>
/// <param name="Aka">El aka del servicio.</param>
/// <param name="Nombre">El nombre del servicio.</param>
public record ServicioModel(int Id, string Aka, string Nombre);
