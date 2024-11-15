using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
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
    public Zona? Zona { get; private set; } = default!;

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
    /// Los detalles asociados a este escenario de cupo.
    /// </summary>
    public ICollection<DetalleEscenarioCupo> DetallesEscenarioCupo { get; set; } = default!;

    /// <summary>
    /// Crea un nuevo escenario de cupo.
    /// </summary>
    /// <param name="id">El id del escenario de cupo.</param>
    /// <param name="centroCosto">El centro de costo asociado al escenario de cupo.</param>
    /// <param name="zona">La zona asociada al escenario de cupo.</param>
    /// <param name="nombre">El nombre del escenario de cupo.</param>
    /// <param name="esHoraInicio">Si es que tiene hora de inicio.</param>
    /// <param name="minutosAntes">Los minutos antes que se puede presentar el cupo.</param>
    /// <param name="esActivo">Si estará habilitado o no.</param>
    /// <param name="detalles">La lista de detalles asociados a este escenario de cupo.</param>
    /// <returns>El resultado de la creación del cupo.</returns>
    public static ResultOf<EscenarioCupo> Create(
    int id,
    CentroCosto centroCosto,
    Zona zona,
    string nombre,
    bool esHoraInicio,
    int minutosAntes,
    bool esActivo,
    IEnumerable<DetalleEscenarioCupoInfo> detalles)
    {

        // Validar detalles duplicados
        var detallesSinDuplicados = detalles
            .GroupBy(d => new { d.EscenarioCupoId, d.ServicioId })
            .Select(g => g.First())
            .ToList();

        if (detallesSinDuplicados.Count != detalles.Count())
        {
            return new ValidationError(nameof(detalles), "Existen detalles duplicados en la colección.");
        }

        // Crear instancia del escenario
        var escenario = new EscenarioCupo
        {
            Id = id,
            CentroCosto = centroCosto,
            Zona = zona,
            Nombre = nombre,
            EsHoraInicio = esHoraInicio,
            MinutosAntes = minutosAntes,
            EsActivo = esActivo,
            DetallesEscenarioCupo = detallesSinDuplicados
                .Select(x =>
                {
                    return DetalleEscenarioCupo.Create(
                        escenarioCupoId: x.EscenarioCupoId,
                        servicioId: x.ServicioId,
                        topeDiario: x.TopeDiario,
                        usaSobreCupo: x.UsaSobreCupo,
                        horaMaximaVenta: x.HoraMaximaVenta.Value,
                        horaMaximaRevalidacion: x.HoraMaximaRevalidacion.Value,
                        usaTopeEnCupo: x.UsaTopeEnCupo,
                        topeFlotante: x.TopeFlotante
                    );
                })
                .ToList()
        };

        return escenario;
    }

    public static ResultOf<EscenarioCupo> CreateOrUpdate(
    int id,
    EscenarioCupo? escenarioExistente,
    CentroCosto centroCosto,
    Zona? zona,
    string nombre,
    bool esHoraInicio,
    int minutosAntes,
    bool esActivo)
    {
        if (escenarioExistente is null)
        {
            return Create(id, centroCosto, zona!, nombre, esHoraInicio, minutosAntes, esActivo, new List<DetalleEscenarioCupoInfo>());
        }

        escenarioExistente.CentroCosto = centroCosto;
        escenarioExistente.Zona = zona;
        escenarioExistente.Nombre = nombre;
        escenarioExistente.EsHoraInicio = esHoraInicio;
        escenarioExistente.MinutosAntes = minutosAntes;
        escenarioExistente.EsActivo = esActivo;

        return escenarioExistente;
    }



    /// <summary>
    /// Actualiza el escenario de cupo con nuevos valores.
    /// </summary>
    internal async Task<ResultOf<EscenarioCupo>> UpdateAsync(
        string nombre,
        CentroCosto centroCosto,
        Zona? zona,
        bool esHoraInicio,
        int minutosAntes,
        bool esActivo,
        IEnumerable<DetalleEscenarioCupoInfo> nuevosDetalles,
        Func<(int id, CentroCosto centroCosto, Zona? zona, string nombre), IList<ValidationError>, CancellationToken, Task> validateDuplicate,
        CancellationToken cancellationToken)
    {
        List<ValidationError> errors = [];
        await validateDuplicate((Id, centroCosto, zona, nombre), errors, cancellationToken);

        CentroCosto = centroCosto;
        Zona = zona;
        Nombre = nombre;
        EsHoraInicio = esHoraInicio;
        MinutosAntes = minutosAntes;
        EsActivo = esActivo;

        // Actualizar la lista de detalles
        UpdateDetalles(nuevosDetalles);

        return this;
    }

    /// <summary>
    /// Actualiza los detalles del escenario de cupo, agregando, actualizando o eliminando según sea necesario.
    /// </summary>
    /// <param name="nuevosDetalles">La lista de nuevos detalles a establecer.</param>
    private void UpdateDetalles(IEnumerable<DetalleEscenarioCupoInfo> nuevosDetallesInfo)
    {

        var nuevosDetalles = nuevosDetallesInfo.Select(x => DetalleEscenarioCupo.Create(
                escenarioCupoId: x.EscenarioCupoId,
                servicioId: x.ServicioId,
                topeDiario: x.TopeDiario,
                usaSobreCupo: x.UsaSobreCupo,
                horaMaximaVenta: x.HoraMaximaVenta.Value,
                horaMaximaRevalidacion: x.HoraMaximaRevalidacion.Value,
                usaTopeEnCupo: x.UsaTopeEnCupo,
                topeFlotante: x.TopeFlotante
            ));

        var detallesAActualizar = nuevosDetalles.ToList();

        var detallesParaAgregar = detallesAActualizar
            .Where(nd => !DetallesEscenarioCupo.Any(de => de.ServicioId == nd.ServicioId))
            .ToList();

        var detallesParaEliminar = DetallesEscenarioCupo
            .Where(de => !detallesAActualizar.Any(nd => nd.ServicioId == de.ServicioId))
            .ToList();

        var detallesParaActualizar = detallesAActualizar
            .Where(nd => DetallesEscenarioCupo.Any(de => de.ServicioId == nd.ServicioId))
            .ToList();

        // Agregar, actualizar y eliminar detalles según corresponda
        foreach (var detalle in detallesParaAgregar)
        {
            DetallesEscenarioCupo.Add(detalle);
        }

        foreach (var detalle in detallesParaActualizar)
        {
            var detalleExistente = DetallesEscenarioCupo.First(de => de.ServicioId == detalle.ServicioId);
            detalleExistente.Update(detalle);
        }

        foreach (var detalle in detallesParaEliminar)
        {
            DetallesEscenarioCupo.Remove(detalle);
        }
    }
}
