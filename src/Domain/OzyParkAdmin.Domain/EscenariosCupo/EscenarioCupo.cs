using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Domain.EscenariosCupo;

/// <summary>
/// La entidad del escenario de cupo.
/// </summary>
public sealed class EscenarioCupo
{
    // Propiedades del escenario de cupo
    public int Id { get; private init; }
    public CentroCosto CentroCosto { get; private set; } = default!;
    public Zona? Zona { get; private set; } = default!;
    public string Nombre { get; private set; } = string.Empty;
    public bool EsHoraInicio { get; private set; }
    public int MinutosAntes { get; private set; }
    public bool EsActivo { get; private set; } = true;
    public ICollection<DetalleEscenarioCupo> DetallesEscenarioCupo { get; set; } = default!;
    public ICollection<DetalleEscenarioCupoExclusionFecha> ExclusionesPorFecha { get; set; } = default!;
    public ICollection<DetalleEscenarioCupoExclusion> Exclusiones { get; set; } = default!;

    /// <summary>
    /// Crea un nuevo escenario de cupo.
    /// </summary>
    public static ResultOf<EscenarioCupo> Create(
        int id,
        CentroCosto centroCosto,
        Zona? zona,
        string nombre,
        bool esHoraInicio,
        int minutosAntes,
        bool esActivo,
        IEnumerable<DetalleEscenarioCupoInfo> detalles,
        IEnumerable<DetalleEscenarioCupoExclusionFechaFullInfo> exclusionesFecha
        )
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
                .Select(x => DetalleEscenarioCupo.Create(
                    escenarioCupoId: id,
                    servicioId: x.ServicioId,
                    topeDiario: x.TopeDiario,
                    usaSobreCupo: x.UsaSobreCupo,
                    horaMaximaVenta: x.HoraMaximaVenta!.Value,
                    horaMaximaRevalidacion: x.HoraMaximaRevalidacion!.Value,
                    usaTopeEnCupo: x.UsaTopeEnCupo,
                    topeFlotante: x.TopeFlotante
                ))
                .ToList(),
            ExclusionesPorFecha = exclusionesFecha
                .Select(x => DetalleEscenarioCupoExclusionFecha.Create(
                    escenarioCupoId: id,
                    servicioId: x.ServicioId,
                    fechaExclusion: x.FechaExclusion!.Value,
                    horaInicio: x.HoraInicio,
                    horaFin: x.HoraFin,
                    canalVentaId: x.CanalVentaId
                ))
                .ToList()
        };

        return escenario;
    }

    /// <summary>
    /// Crea o actualiza un escenario de cupo.
    /// </summary>
    public static ResultOf<EscenarioCupo> CreateOrUpdate(
        int id,
        CentroCosto centroCosto,
        Zona? zona,
        string nombre,
        bool esHoraInicio,
        int minutosAntes,
        bool esActivo,
        IEnumerable<DetalleEscenarioCupoInfo> detalles,
        IEnumerable<DetalleEscenarioCupoExclusionFechaFullInfo> exclusionesFecha,
        IEnumerable<DetalleEscenarioCupoExclusionFullInfo> exclusiones,
        EscenarioCupo? escenarioExistente = null)
    {
        // Si no existe, se debe crear un nuevo EscenarioCupo
        if (escenarioExistente is null)
        {
            return Create(id, centroCosto, zona, nombre, esHoraInicio, minutosAntes, esActivo, detalles, exclusionesFecha);
        }

        // Si ya existe, actualizar el escenarioExistente
        escenarioExistente.CentroCosto = centroCosto;
        escenarioExistente.Zona = zona;
        escenarioExistente.Nombre = nombre;
        escenarioExistente.EsHoraInicio = esHoraInicio;
        escenarioExistente.MinutosAntes = minutosAntes;
        escenarioExistente.EsActivo = esActivo;

        escenarioExistente.UpdateDetalles(detalles);
        escenarioExistente.UpdateExclusionesFecha(exclusionesFecha);
        escenarioExistente.UpdateExclusiones(exclusiones);

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
        IEnumerable<DetalleEscenarioCupoExclusionFechaFullInfo> nuevasExclusiones,
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
        // Actualizar la lista de exclusionesFecha
        UpdateExclusionesFecha(nuevasExclusiones);

        return this;
    }

    /// <summary>
    /// Actualiza los detalles del escenario de cupo, agregando, actualizando o eliminando según sea necesario.
    /// </summary>
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

    /// <summary>
    /// Actualiza las exclusionesFecha del escenario de cupo.
    /// </summary>
    private void UpdateExclusionesFecha(IEnumerable<DetalleEscenarioCupoExclusionFechaFullInfo> nuevasExclusionesInfo)
    {
        var nuevasExclusiones = nuevasExclusionesInfo.Select(x => DetalleEscenarioCupoExclusionFecha.Create(
                escenarioCupoId: x.EscenarioCupoId,
                servicioId: x.ServicioId,
                fechaExclusion: x.FechaExclusion,
                horaInicio: x.HoraInicio,
                horaFin: x.HoraFin,
                canalVentaId: x.CanalVentaId
            ));

        var exclusionesAActualizar = nuevasExclusiones.ToList();

        var exclusionesParaAgregar = exclusionesAActualizar
            .Where(ne => !ExclusionesPorFecha.Any(ee => ee.ServicioId == ne.ServicioId && ee.FechaExclusion == ne.FechaExclusion))
            .ToList();

        var exclusionesParaEliminar = ExclusionesPorFecha
            .Where(ee => !exclusionesAActualizar.Any(ne => ne.ServicioId == ee.ServicioId && ne.FechaExclusion == ee.FechaExclusion))
            .ToList();

        var exclusionesParaActualizar = exclusionesAActualizar
            .Where(ne => ExclusionesPorFecha.Any(ee => ee.ServicioId == ne.ServicioId && ne.FechaExclusion == ne.FechaExclusion))
            .ToList();

        // Agregar, actualizar y eliminar exclusionesFecha según corresponda
        foreach (var exclusion in exclusionesParaAgregar)
        {
            ExclusionesPorFecha.Add(exclusion);
        }

        foreach (var exclusion in exclusionesParaActualizar)
        {
            var exclusionExistente = ExclusionesPorFecha.First(ee => ee.ServicioId == exclusion.ServicioId && ee.FechaExclusion == exclusion.FechaExclusion);
            exclusionExistente.Update(exclusion);
        }

        foreach (var exclusion in exclusionesParaEliminar)
        {
            ExclusionesPorFecha.Remove(exclusion);
        }
    }

    /// <summary>
    /// Actualiza las exclusionesFecha del escenario de cupo.
    /// </summary>
    private void UpdateExclusiones(IEnumerable<DetalleEscenarioCupoExclusionFullInfo> nuevasExclusionesInfo)
    {
        var nuevasExclusiones = nuevasExclusionesInfo.Select(x => DetalleEscenarioCupoExclusion.Create(
                escenarioCupoId: x.EscenarioCupoId,
                servicioId: x.ServicioId,
                diaSemanaId: x.DiaSemanaId,
                horaInicio: x.HoraInicio,
                horaFin: x.HoraFin,
                canalVentaId: x.CanalVentaId
            ));

        var exclusionesAActualizar = nuevasExclusiones.ToList();

        var exclusionesParaAgregar = exclusionesAActualizar
            .Where(ne => !Exclusiones.Any(ee => ee.ServicioId == ne.ServicioId && ee.DiaSemanaId == ne.DiaSemanaId && ee.HoraInicio == ne.HoraInicio))
            .ToList();

        var exclusionesParaEliminar = Exclusiones
            .Where(ee => !exclusionesAActualizar.Any(ne => ne.ServicioId == ee.ServicioId && ne.DiaSemanaId == ee.DiaSemanaId && ee.HoraInicio == ne.HoraInicio))
            .ToList();

        var exclusionesParaActualizar = exclusionesAActualizar
            .Where(ne => Exclusiones.Any(ee => ee.ServicioId == ne.ServicioId && ee.DiaSemanaId == ne.DiaSemanaId && ee.HoraInicio == ne.HoraInicio))
            .ToList();

        // Agregar, actualizar y eliminar exclusionesFecha según corresponda
        foreach (var exclusion in exclusionesParaAgregar)
        {
            Exclusiones.Add(exclusion);
        }

        foreach (var exclusion in exclusionesParaActualizar)
        {
            var exclusionExistente = Exclusiones.First(ee => ee.ServicioId == exclusion.ServicioId && ee.DiaSemanaId == exclusion.DiaSemanaId && ee.HoraInicio == exclusion.HoraInicio);
            exclusionExistente.Update(exclusion);
        }

        foreach (var exclusion in exclusionesParaEliminar)
        {
            Exclusiones.Remove(exclusion);
        }
    }
}
