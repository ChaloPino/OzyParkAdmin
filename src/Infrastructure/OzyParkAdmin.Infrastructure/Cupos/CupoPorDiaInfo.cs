namespace OzyParkAdmin.Infrastructure.Cupos;

internal sealed class CupoPorDiaInfo
{
    public DateTime Fecha { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public int CupoTotal { get; set; }
    public int CupoDisponible { get; set; }
}