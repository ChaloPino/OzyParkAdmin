using OzyParkAdmin.Domain.CentrosCosto;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// El centro de costo asociado a un servicio.
/// </summary>
public sealed class CentroCostoServicio
{
    /// <summary>
    /// El centro de costo.
    /// </summary>
    public CentroCosto CentroCosto { get; private set; } = default!;

    /// <summary>
    /// El nombre del servicio en la asociación con el centro de costo.
    /// </summary>
    public string? Nombre { get; private set; }

    internal static CentroCostoServicio Create(CentroCosto centroCosto, string? nombre) =>
        new() { CentroCosto = centroCosto, Nombre = nombre };

    internal void Update(string? nombre) =>
        Nombre = nombre;
}