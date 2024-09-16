namespace OzyParkAdmin.Domain.CentrosCosto;

/// <summary>
/// Entidad centro de costo.
/// </summary>
public sealed class CentroCosto
{
    private readonly List<EmisorCentroCosto> _emisores = [];
    private readonly List<CentroCostoHorario> _horarios = [];
    /// <summary>
    /// Identificador único del centro de costo.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Aka del centro de costo.
    /// </summary>
    public string Aka { get; private set; } = string.Empty;

    /// <summary>
    /// Descripción del centro de costo.
    /// </summary>
    public string Descripcion { get; private set; } = string.Empty;

    /// <summary>
    /// Si el centro de costo es emisor de boletas electrónicas.
    /// </summary>
    public bool EsEmisorElectronico { get; private set; }

    /// <summary>
    /// Si el centro de costo está activo o no.
    /// </summary>
    public bool EsActivo { get; private set; }

    /// <summary>
    /// Nombre comerciual del centro de costo.
    /// </summary>
    public string NombreComercial { get; private set; } = string.Empty;

    /// <summary>
    /// Hora de apertura del centro de costo.
    /// </summary>
    public TimeSpan HoraApertura { get; private set; }

    /// <summary>
    /// Hora de cierre del centro de costo.
    /// </summary>
    public TimeSpan HoraCierre { get; private set; }

    /// <summary>
    /// Cantidad de días que se pueden mostrar para los cupos.
    /// </summary>
    public int CantidadDiasCupo { get; private set; }

    /// <summary>
    /// Si usa segmentación nacional y extranjero para la tarifación.
    /// </summary>
    public bool UsaSegmentacion { get; private set; }

    /// <summary>
    /// Si soporta un solo guía para los tickets.
    /// </summary>
    public bool SoportaGuiaUnico { get; private set; }

    /// <summary>
    /// Emisores del centro de costo.
    /// </summary>
    public IEnumerable<EmisorCentroCosto> Emisores => _emisores;

    /// <summary>
    /// Horarios del centro de costo.
    /// </summary>
    public IEnumerable<CentroCostoHorario> Horarios => _horarios;

    /// <summary>
    /// Emisores en uso del centro de costo.
    /// </summary>
    public IEnumerable<EmisorCentroCosto> EmisoresEnUso
        => _emisores.Where(e => e.EnUso);

    /// <summary>
    /// Crea un nuevo centro de costo.
    /// </summary>
    /// <param name="id">Identificador único.</param>
    /// <param name="aka">Aka.</param>
    /// <param name="descripcion">Descripción.</param>
    /// <param name="esEmisorElectronico">Si es emisor de boletas electrónicas.</param>
    /// <param name="nombreComercial">Nombre comercial.</param>
    /// <param name="horaApertura">Hora de apertura.</param>
    /// <param name="horaCierre">Hora de cierre.</param>
    /// <param name="cantidadDiasCupo">Cantidad de diás para cupo.</param>
    /// <param name="usaSegmentacion">Si usa segementación.</param>
    /// <param name="soportaGuiaUnico">Si soporta guía único.</param>
    /// <returns>Un nuevo <see cref="CentroCosto"/>.</returns>
    public static CentroCosto Crear(
        int id,
        string aka,
        string descripcion,
        bool esEmisorElectronico,
        string nombreComercial,
        TimeSpan horaApertura,
        TimeSpan horaCierre,
        int cantidadDiasCupo,
        bool usaSegmentacion,
        bool soportaGuiaUnico) =>
        new()
        {
            Id = id,
            Aka = aka,
            Descripcion = descripcion,
            EsEmisorElectronico = esEmisorElectronico,
            NombreComercial = nombreComercial,
            HoraApertura = horaApertura,
            HoraCierre = horaCierre,
            CantidadDiasCupo = cantidadDiasCupo,
            UsaSegmentacion = usaSegmentacion,
            SoportaGuiaUnico = soportaGuiaUnico,
            EsActivo = true,
        };

    /// <summary>
    /// Consigue la hora de apertura del centro de costo dependiendo de <paramref name="fecha"/>.
    /// </summary>
    /// <param name="fecha">La fecha para la cual se determinará la hora de apertura.</param>
    /// <returns>La hora de apertura determinada para <paramref name="fecha"/>.</returns>
    /// <remarks>
    /// La hora de apertura se determina dependiendo si es que la <paramref name="fecha"/> solicidada 
    /// existe como parte de los <see cref="Horarios"/> del centro de costo, en caso contrario devolverá la <see cref="HoraApertura"/>.
    /// </remarks>
    public TimeSpan ConseguirHoraApertura(DateOnly fecha)
    {
        return Horarios?.FirstOrDefault(x => x.Fecha == fecha)?.HoraApertura ?? HoraApertura;
    }

    /// <summary>
    /// Consigue la hora de cierre del centro de costo dependiendo de <paramref name="fecha"/>.
    /// </summary>
    /// <param name="fecha">La fecha para la cual se determinará hora de cierre.</param>
    /// <returns>La hora de cierre determinada para <paramref name="fecha"/>.</returns>
    /// <remarks>
    /// La hora de cierre se determina dependiendo si es que la <paramref name="fecha"/> solicidada 
    /// existe como parte de los <see cref="Horarios"/> del centro de costo, en caso contrario devolverá la <see cref="HoraCierre"/>.
    /// </remarks>
    public TimeSpan ConseguirHoraCierre(DateOnly fecha)
    {
        return Horarios?.FirstOrDefault(x => x.Fecha == fecha)?.HoraCierre ?? HoraCierre;
    }
}
