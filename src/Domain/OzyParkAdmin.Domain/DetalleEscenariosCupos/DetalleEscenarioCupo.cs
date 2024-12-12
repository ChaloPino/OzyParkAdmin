using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Domain.DetallesEscenariosCupos;

/// <summary>
/// Entidad que representa un detalle del escenario de cupo.
/// </summary>
public sealed class DetalleEscenarioCupo
{
    /// <summary>
    /// ID del escenario de cupo asociado.
    /// </summary>
    public int EscenarioCupoId { get; private set; }

    /// <summary>
    /// Escenario cupo asociado.
    /// </summary>
    public EscenarioCupo EscenarioCupo { get; private set; }

    /// <summary>
    /// ID del servicio asociado.
    /// </summary>
    public int ServicioId { get; private set; }

    /// <summary>
    /// Servicio asociado.
    /// </summary>
    public Servicio Servicio { get; private set; }

    /// <summary>
    /// Tope diario permitido.
    /// </summary>
    public int? TopeDiario { get; private set; }

    /// <summary>
    /// Indica si usa sobre cupo.
    /// </summary>
    public bool UsaSobreCupo { get; private set; }

    /// <summary>
    /// Hora máxima de venta.
    /// </summary>
    public TimeSpan HoraMaximaVenta { get; private set; }

    /// <summary>
    /// Hora máxima de revalidación.
    /// </summary>
    public TimeSpan HoraMaximaRevalidacion { get; private set; }

    /// <summary>
    /// Indica si utiliza tope en cupo.
    /// </summary>
    public bool UsaTopeEnCupo { get; private set; }

    /// <summary>
    /// Indica si el tope es flotante.
    /// </summary>
    public bool TopeFlotante { get; private set; }

    /// <summary>
    /// Método estático para crear un nuevo detalle del escenario de cupo.
    /// </summary>
    public static DetalleEscenarioCupo Create(
        int escenarioCupoId,
        int servicioId,
        int? topeDiario,
        bool usaSobreCupo,
        TimeSpan horaMaximaVenta,
        TimeSpan horaMaximaRevalidacion,
        bool usaTopeEnCupo,
        bool topeFlotante)
    {
        return new DetalleEscenarioCupo
        {
            EscenarioCupoId = escenarioCupoId,
            ServicioId = servicioId,
            TopeDiario = topeDiario,
            UsaSobreCupo = usaSobreCupo,
            HoraMaximaVenta = horaMaximaVenta,
            HoraMaximaRevalidacion = horaMaximaRevalidacion,
            UsaTopeEnCupo = usaTopeEnCupo,
            TopeFlotante = topeFlotante
        };
    }

    /// <summary>
    /// Método para actualizar los valores de un detalle del escenario de cupo.
    /// </summary>
    public void Update(
        int escenarioCupoId,
        int? topeDiario,
        bool usaSobreCupo,
        TimeSpan horaMaximaVenta,
        TimeSpan horaMaximaRevalidacion,
        bool usaTopeEnCupo,
        bool topeFlotante)
    {
        EscenarioCupoId = escenarioCupoId;
        TopeDiario = topeDiario;
        UsaSobreCupo = usaSobreCupo;
        HoraMaximaVenta = horaMaximaVenta;
        HoraMaximaRevalidacion = horaMaximaRevalidacion;
        UsaTopeEnCupo = usaTopeEnCupo;
        TopeFlotante = topeFlotante;
    }

    /// <summary>
    /// Método para actualizar los valores de un detalle del escenario de cupo.
    /// </summary>
    public void Update(
       DetalleEscenarioCupo detalle)
    {
        EscenarioCupoId = detalle.EscenarioCupoId;
        TopeDiario = detalle.TopeDiario;
        UsaSobreCupo = detalle.UsaSobreCupo;
        HoraMaximaVenta = detalle.HoraMaximaVenta;
        HoraMaximaRevalidacion = detalle.HoraMaximaRevalidacion;
        UsaTopeEnCupo = detalle.UsaTopeEnCupo;
        TopeFlotante = detalle.TopeFlotante;
    }

    public void UpdateEscenarioId(int escenarioCupoId)
    {
        EscenarioCupoId = escenarioCupoId;
    }
}
