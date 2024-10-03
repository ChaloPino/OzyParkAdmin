using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Domain.EscenariosCupo;

/// <summary>
/// La entidad del escenario de cupo.
/// </summary>
public sealed class EscenarioCupo
{
    /// <summary>
    /// El id del escenario de cupo.
    /// </summary>
    public int Id { get; private init; }

    /// <summary>
    /// El centro de costo del escenario de cupo.
    /// </summary>
    public CentroCosto CentroCosto { get; private set; } = default!;

    /// <summary>
    /// La zona asociada al escenario de cupo.
    /// </summary>
    public Zona Zona { get; private set; } = default!;

    /// <summary>
    /// El nombre del escenario de cupo.
    /// </summary>
    public string Nombre { get; private set; } = string.Empty;

    /// <summary>
    /// Si es que tiene hora de inicio.
    /// </summary>
    public bool EsHoraInicio { get; private set; }

    /// <summary>
    /// Los minutos antes que se puede presentar el cupo.
    /// </summary>
    public int MinutosAntes { get; private set; }

    /// <summary>
    /// Si el escenario de cupo está activo.
    /// </summary>
    public bool EsActivo { get; private set; } = true;

    /// <summary>
    /// Crea un nuevo escenario de cupo.
    /// </summary>
    /// <param name="id">El id del escenario de cupo.</param>
    /// <param name="centroCosto">El centro de costo asociado al escenario de cupo.</param>
    /// <param name="zona">La zona asociada al escenario de cupo.</param>
    /// <param name="nombre">El nombre del escenario de cupo.</param>
    /// <param name="esHoraInicio">Si es que tiene hora de inicio.</param>
    /// <param name="minutosAntes">Los minutos antes que se puede presentar el cupo.</param>
    /// <returns>El resultado de la creación del cupo.</returns>
    public static ResultOf<EscenarioCupo> Create(
        int id,
        CentroCosto centroCosto,
        Zona zona,
        string nombre,
        bool esHoraInicio,
        int minutosAntes) =>
        new EscenarioCupo()
        {
            Id = id,
            CentroCosto = centroCosto,
            Zona = zona,
            Nombre = nombre,
            EsHoraInicio = esHoraInicio,
            MinutosAntes = minutosAntes
        };
}
