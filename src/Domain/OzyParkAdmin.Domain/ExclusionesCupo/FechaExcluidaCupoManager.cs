using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.ExclusionesCupo;

/// <summary>
/// La lógica de negocios de <see cref="FechaExcluidaCupo"/>.
/// </summary>
public sealed class FechaExcluidaCupoManager : IBusinessLogic
{
    private readonly ICentroCostoRepository _centroCostoRepository;
    private readonly IFechaExcluidaCupoRepository _fechaExcluidaCuporepository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="FechaExcluidaCupoManager"/>.
    /// </summary>
    /// <param name="fechaExcluidaRepository">El <see cref="IFechaExcluidaCupoRepository"/>.</param>
    /// <param name="centroCostoRepository">El <see cref="ICentroCostoRepository"/>.</param>
    public FechaExcluidaCupoManager(IFechaExcluidaCupoRepository fechaExcluidaRepository, ICentroCostoRepository centroCostoRepository)
    {
        ArgumentNullException.ThrowIfNull(centroCostoRepository);
        ArgumentNullException.ThrowIfNull(fechaExcluidaRepository);
        _centroCostoRepository = centroCostoRepository;
        _fechaExcluidaCuporepository = fechaExcluidaRepository;
    }

    /// <summary>
    /// Crea varias fechas de exclusión de cupos.
    /// </summary>
    /// <param name="centroCostoInfo">El centro de costo.</param>
    /// <param name="canalesVenta">Los canales de venta.</param>
    /// <param name="fechaDesde">La fecha desde para generar las exclusiones.</param>
    /// <param name="fechaHasta">La fecha hasta para generar las exclusiones.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de crear varias fechas de exclusión.</returns>
    public async Task<ResultOf<IEnumerable<FechaExcluidaCupo>>> CreateFechasExcluidasAsync(
        CentroCostoInfo centroCostoInfo,
        IEnumerable<CanalVenta> canalesVenta,
        DateOnly fechaDesde,
        DateOnly fechaHasta,
        CancellationToken cancellationToken)
    {
        CentroCosto? centroCosto = await _centroCostoRepository.FindByIdAsync(centroCostoInfo.Id, cancellationToken);

        if (centroCosto is null)
        {
            return new ValidationError(nameof(FechaExcluidaCupo.CentroCosto), $"No existe el centro de costo '{centroCostoInfo.Descripcion}'.");
        }

        IEnumerable<DateOnly> fechas = [..CrearFechas(fechaDesde, fechaHasta)];

        IEnumerable<FechaExcluidaCupo> fechasExcluidasExistentes = await _fechaExcluidaCuporepository.FindFechasExcluidasAsync(
            centroCosto.Id,
            [.. canalesVenta.Select(x => x.Id)],
            [.. fechas],
            cancellationToken);

        return  CreateFechasExcluidas(
            centroCosto,
            canalesVenta,
            fechas,
            fechasExcluidasExistentes);
    }

    private static ResultOf<IEnumerable<FechaExcluidaCupo>> CreateFechasExcluidas(
        CentroCosto centroCosto,
        IEnumerable<CanalVenta> canalesVenta,
        IEnumerable<DateOnly> fechas,
        IEnumerable<FechaExcluidaCupo> fechasExistentes)
    {
        List<FechaExcluidaCupo> nuevasFechasExcluidas = [];

        var infoToAdd = from canalVenta in canalesVenta
                        from fecha in fechas
                        select (centroCosto, canalVenta, fecha);

        foreach (var info in infoToAdd)
        {
            if (!fechasExistentes.Any(x => x.CentroCosto == info.centroCosto && x.CanalVenta == info.canalVenta && x.Fecha == info.fecha))
            {
                var result = CreateFechaExcluida(info.centroCosto, info.canalVenta, info.fecha, nuevasFechasExcluidas);

                if (result.IsFailure(out Failure failure))
                {
                    return failure;
                }
            }
        }

        return nuevasFechasExcluidas;
    }

    private static SuccessOrFailure CreateFechaExcluida(CentroCosto centroCosto, CanalVenta canalVenta, DateOnly fecha, IList<FechaExcluidaCupo> nuevasFechasExcluidas)
    {
        var result = FechaExcluidaCupo.Create(centroCosto, canalVenta, fecha);

        return result.Match(
            onSuccess: fechaExcluida => AddToList(fechaExcluida, nuevasFechasExcluidas),
            onFailure: _ => _);
    }

    private static SuccessOrFailure AddToList(FechaExcluidaCupo fechaExcluida, IList<FechaExcluidaCupo> nuevasFechasExcluidas)
    {
        nuevasFechasExcluidas.Add(fechaExcluida);
        return new Success();
    }

    private static IEnumerable<DateOnly> CrearFechas(DateOnly fechaDesde, DateOnly fechaHasta)
    {
        DateOnly date = fechaDesde;

        do
        {
            yield return date;
            date = date.AddDays(1);
        } while (date <= fechaHasta);
    }
}
