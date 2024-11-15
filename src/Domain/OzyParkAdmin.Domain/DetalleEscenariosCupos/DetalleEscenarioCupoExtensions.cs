namespace OzyParkAdmin.Domain.DetallesEscenariosCupos;
public static class DetalleEscenarioCupoExtensions
{
    public static DetalleEscenarioCupoInfo ToInfo(this DetalleEscenarioCupo detalle) => new DetalleEscenarioCupoInfo
    {
        EscenarioCupoId = detalle.EscenarioCupoId,
        ServicioId = detalle.ServicioId,
        TopeDiario = detalle.TopeDiario,
        UsaSobreCupo = detalle.UsaSobreCupo,
        HoraMaximaVenta = detalle.HoraMaximaVenta,
        HoraMaximaRevalidacion = detalle.HoraMaximaRevalidacion,
        UsaTopeEnCupo = detalle.UsaTopeEnCupo,
        TopeFlotante = detalle.TopeFlotante
    };
}
