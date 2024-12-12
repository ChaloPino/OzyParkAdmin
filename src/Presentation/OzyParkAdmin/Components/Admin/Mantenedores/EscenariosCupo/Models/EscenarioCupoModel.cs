using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
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
    /// Si el escenario de cupo puede ser eliminado o no, depende de si tiene cupos asociados.
    /// </summary>
    public bool PuedeSerEliminado { get; set; } = true;



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
    }
    /// <summary>
    /// Devuelve true cuándo se han modificado detalles <see cref="DetalleEscenarioCupo"/>.
    /// </summary>
    public bool DetallesModificados { get; set; }

    /// <summary>
    /// Devuelve true si se han modificado las exclusiones por fecha <see cref="DetalleEscenarioCupoExclusion"/>.
    /// </summary>
    public bool ExclusionesPorFechasModificadas { get; set; }
}
