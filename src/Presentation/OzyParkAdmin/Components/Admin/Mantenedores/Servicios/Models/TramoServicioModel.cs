using OzyParkAdmin.Components.Admin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;

/// <summary>
/// El tramo asociado a un servicio.
/// </summary>
public sealed record TramoServicioModel
{
    /// <summary>
    /// El centro de costo asociado.
    /// </summary>
    public CentroCostoModel CentroCosto { get; set; } = default!;

    /// <summary>
    /// El tramo asociado.
    /// </summary>
    public TramoModel Tramo { get; set; } = default!;

    /// <summary>
    /// El nombre que sobrescribe al nombre del servicio en esta asociación.
    /// </summary>
    public string? Nombre { get; set; }

    /// <summary>
    /// La cantidad máxima de permisos que puede tener esta asociación.
    /// </summary>
    public int? CantidadPersmisos { get; set; }
}