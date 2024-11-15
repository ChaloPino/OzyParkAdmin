using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Domain.EscenariosCupo;

/// <summary>
/// Métodos de extensión para la entidad <see cref="EscenarioCupo"/>.
/// </summary>
public static class EscenarioCupoExtensions
{
    /// <summary>
    /// Convierte un <see cref="EscenarioCupo"/> a un <see cref="EscenarioCupoInfo"/>.
    /// </summary>
    public static EscenarioCupoInfo ToInfo(this EscenarioCupo escenarioCupo)
    {
        return new EscenarioCupoInfo
        {
            Id = escenarioCupo.Id,
            Nombre = escenarioCupo.Nombre,
            EsActivo = escenarioCupo.EsActivo
        };
    }

    /// <summary>
    /// Convierte un <see cref="EscenarioCupo"/> a un <see cref="EscenarioCupoFullInfo"/>.
    /// </summary>
    public static EscenarioCupoFullInfo ToFullInfo(this EscenarioCupo escenarioCupo)
    {
        return new EscenarioCupoFullInfo
        {
            Id = escenarioCupo.Id,
            Nombre = escenarioCupo.Nombre,
            EsActivo = escenarioCupo.EsActivo,
            CentroCosto = new CentroCostoInfo
            {
                Id = escenarioCupo.CentroCosto.Id,
                Descripcion = escenarioCupo.CentroCosto.Descripcion
            },
            Zona = escenarioCupo.Zona is not null
                ? new ZonaInfo
                {
                    Id = escenarioCupo.Zona.Id,
                    Descripcion = escenarioCupo.Zona.Descripcion
                }
                : null,
            EsHoraInicio = escenarioCupo.EsHoraInicio,
            MinutosAntes = escenarioCupo.MinutosAntes,
            Detalles = escenarioCupo.DetallesEscenarioCupo.Select(detalle => new DetalleEscenarioCupoInfo
            {
                EscenarioCupoId = detalle.EscenarioCupoId,
                ServicioId = detalle.ServicioId,
                TopeDiario = detalle.TopeDiario,
                UsaSobreCupo = detalle.UsaSobreCupo,
                HoraMaximaVenta = detalle.HoraMaximaVenta,
                HoraMaximaRevalidacion = detalle.HoraMaximaRevalidacion,
                UsaTopeEnCupo = detalle.UsaTopeEnCupo,
                TopeFlotante = detalle.TopeFlotante
            }).ToList()
        };
    }
}
