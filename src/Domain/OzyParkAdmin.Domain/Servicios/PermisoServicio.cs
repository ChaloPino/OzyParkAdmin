using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// El permiso que se tiene por servicio.
/// </summary>
public class PermisoServicio
{
    /// <summary>
    /// El tramo asociado.
    /// </summary>
    public Tramo Tramo { get; private set; } = default!;

    /// <summary>
    /// El centro de costo asociado.
    /// </summary>
    public CentroCosto CentroCosto { get; private set; } = default!;

    internal (int TramoId, int CentroCostoId) Id => (Tramo.Id, CentroCosto.Id);

    internal static PermisoServicio Create(Tramo tramo, CentroCosto centroCosto) =>
        new() {  Tramo = tramo, CentroCosto = centroCosto};
}