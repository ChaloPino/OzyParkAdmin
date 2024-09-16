namespace OzyParkAdmin.Domain.CentrosCosto;

/// <summary>
/// Horarios de apertura y cierre del centro de costo por fecha.
/// </summary>
public sealed class CentroCostoHorario
{
    /// <summary>
    /// Identificador del centro de costo.
    /// </summary>
    public int CentroCostoId { get; private set; }

    /// <summary>
    /// Fecha.
    /// </summary>
    public DateOnly Fecha { get; private set; }

    /// <summary>
    /// Hora de apertura.
    /// </summary>
    public TimeSpan? HoraApertura { get; private set; }

    /// <summary>
    /// Hora de cierre.
    /// </summary>
    public TimeSpan? HoraCierre { get; private set; }

    internal static CentroCostoHorario Crear(
        CentroCosto centroCosto,
        DateOnly fecha,
        TimeSpan? horaApertura,
        TimeSpan? horaCierre) =>
        new()
        {
            CentroCostoId = centroCosto.Id,
            Fecha = fecha,
            HoraApertura = horaApertura,
            HoraCierre = horaCierre,
        };
}