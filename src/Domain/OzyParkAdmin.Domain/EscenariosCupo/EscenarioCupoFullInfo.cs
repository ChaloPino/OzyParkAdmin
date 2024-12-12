using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Domain.EscenariosCupo;

/// <summary>
/// Contiene información básica del escenario de cupo.
/// </summary>
public sealed record EscenarioCupoFullInfo
{
    /// <summary>
    /// El id del escenario de cupo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El nombre del escenario de cupo.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// El centro de costo del escenario cupo
    /// </summary>
    public CentroCostoInfo CentroCosto { get; set; } = default!;

    /// <summary>
    /// La zona en la que aplica el escenario cupo
    /// </summary>
    public ZonaInfo? Zona { get; set; } = default!;

    /// <summary>
    /// Si es hora de inicio.
    /// </summary>
    public bool EsHoraInicio { get; set; }

    /// <summary>
    /// Minutos antes.
    /// </summary>
    public int MinutosAntes { get; set; }

    /// <summary>
    /// Si el escenario de cupo está activo.
    /// </summary>
    public bool EsActivo { get; set; }

    /// <summary>
    /// Si el escenario tiene un cupo asoaciado.
    /// </summary>
    public bool TienCupoAsociado { get; set; }

}