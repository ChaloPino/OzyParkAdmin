using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// El tramo asociado a un servicio.
/// </summary>
public sealed class TramoServicio
{
    /// <summary>
    /// El centro de costo.
    /// </summary>
    public CentroCosto CentroCosto { get; private set; } = default!;

    /// <summary>
    /// El tramo.
    /// </summary>
    public Tramo Tramo { get; private set; } = default!;

    /// <summary>
    /// El nombre.
    /// </summary>
    public string? Nombre { get; private set; }

    /// <summary>
    /// La cantidad de persmisos.
    /// </summary>
    public int? CantidadPermisos { get; private set; }

    internal (int CentroCostoId, int TramoId) Id => (CentroCosto.Id, Tramo.Id);

    internal static TramoServicio Create(CentroCosto centroCosto, Tramo tramo, string? nombre, int? cantidadPermisos) =>
        new() {  CentroCosto = centroCosto, Tramo = tramo, Nombre = nombre, CantidadPermisos = cantidadPermisos};

    internal void Update(string? nombre, int? cantidadPermisos)
    {
        Nombre = nombre;
        CantidadPermisos = cantidadPermisos;
    }
}