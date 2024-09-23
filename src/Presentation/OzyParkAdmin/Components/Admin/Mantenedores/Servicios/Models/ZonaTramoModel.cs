namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;

/// <summary>
/// La zona por tramo asociada al servicio.
/// </summary>
public sealed record ZonaTramoModel
{
    /// <summary>
    /// El tramo.
    /// </summary>
    public TramoModel Tramo { get; set; } = default!;

    /// <summary>
    /// La zona.
    /// </summary>
    public ZonaModel Zona { get; set; } = default!;

    /// <summary>
    /// Si la zona para este tramo es de retorno.
    /// </summary>
    public bool EsRetorno { get; set; }

    /// <summary>
    /// Si la zona para este tramo es de combinación.
    /// </summary>
    public bool EsCombinacion { get; set; }

    /// <summary>
    /// El orden en que la zona se muestra para el tramo.
    /// </summary>
    public int Orden { get; set; }

    /// <summary>
    /// Si la zona para este tramo está activo.
    /// </summary>
    public bool EsActivo { get; set; }
}