using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;

/// <summary>
/// Modelo de Escenario SelectedEscenarioCupo.
/// </summary>
public class EscenarioCupoModel
{
    /// <summary>
    /// El id del escenario de cupo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El centro de costo del escenario de cupo.
    /// </summary>
    public CentroCostoInfo CentroCosto { get; set; } = default!;

    /// <summary>
    /// La zona asociada al escenario de cupo.
    /// </summary>
    public ZonaInfo? Zona { get; set; } = null!;

    /// <summary>
    /// La descripción de la zona asociada al escenario de cupo.
    /// </summary>
    public string ZonaDescripcion => Zona?.Descripcion ?? "Sin Zona";

    /// <summary>
    /// El nombre del escenario de cupo.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Si es que tiene hora de inicio.
    /// </summary>
    public bool EsHoraInicio { get; set; }

    /// <summary>
    /// Los minutos antes que se puede presentar el cupo.
    /// </summary>
    public int MinutosAntes { get; set; }

    /// <summary>
    /// Si el escenario de cupo está activo.
    /// </summary>
    public bool EsActivo { get; set; } = true;

    /// <summary>
    /// Lista de detalles asociados al escenario de cupo.
    /// </summary>
    public List<DetalleEscenarioCupoInfo> Detalles { get; set; } = new();

    /// <summary>
    /// Actualiza el modelo con la información completa de <see cref="EscenarioCupoFullInfo"/>.
    /// </summary>
    public void Save(EscenarioCupoFullInfo info)
    {
        Nombre = info.Nombre;
        EsHoraInicio = info.EsHoraInicio;
        MinutosAntes = info.MinutosAntes;
        EsActivo = info.EsActivo;
        CentroCosto = info.CentroCosto;
        Zona = info.Zona;
        Detalles = info.Detalles?.ToList() ?? new List<DetalleEscenarioCupoInfo>();
    }

    /// <summary>
    /// Actualiza las propiedades del modelo con otro modelo <see cref="EscenarioCupoModel"/>.
    /// </summary>
    public void Update(EscenarioCupoModel model)
    {
        Nombre = model.Nombre;
        EsHoraInicio = model.EsHoraInicio;
        MinutosAntes = model.MinutosAntes;
        EsActivo = model.EsActivo;
        CentroCosto = model.CentroCosto;
        Zona = model.Zona;
        Detalles = model.Detalles;
    }
}
