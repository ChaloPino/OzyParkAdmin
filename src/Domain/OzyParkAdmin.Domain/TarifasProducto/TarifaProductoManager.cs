using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.TarifasProducto;

/// <summary>
/// Contiene las lógicas de negocio de <see cref="TarifaProducto"/>.
/// </summary>
public sealed class TarifaProductoManager : IBusinessLogic
{
    private readonly ITarifaProductoRepository _tarifaProductoRepository;
    private readonly IProductoRepository _ProductoRepository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="TarifaProductoManager"/>.
    /// </summary>
    /// <param name="tarifaProductoRepository">El <see cref="ITarifaProductoRepository"/>.</param>
    /// <param name="ProductoRepository">El <see cref="IProductoRepository"/>.</param>
    public TarifaProductoManager(
        ITarifaProductoRepository tarifaProductoRepository,
        IProductoRepository ProductoRepository)
    {
        ArgumentNullException.ThrowIfNull(tarifaProductoRepository);
        ArgumentNullException.ThrowIfNull(ProductoRepository);

        _tarifaProductoRepository = tarifaProductoRepository;
        _ProductoRepository = ProductoRepository;
    }

    /// <summary>
    /// Crea varias tarifas de Producto.
    /// </summary>
    /// <param name="inicioVigencia">El inicio de vigencia de la tarifa.</param>
    /// <param name="moneda">La moneda de la tarifa.</param>
    /// <param name="ProductoInfo">El Producto de la tarifa.</param>
    /// <param name="canalesVenta">Lista de canales de venta.</param>
    /// <param name="tiposDia">Lista de tipos de día.</param>
    /// <param name="tiposHorario">Lista de tipos de horario.</param>
    /// <param name="valorAfecto">El valor afecto de la tarifa.</param>
    /// <param name="valorExento">El valor exento de la tarifa.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>
    /// El resultado de la creación de las tarifas de Producto.
    /// </returns>
    public async Task<ResultOf<IEnumerable<TarifaProducto>>> CreateAsync(
        DateTime inicioVigencia,
        Moneda moneda,
        ProductoInfo ProductoInfo,
        IEnumerable<CanalVenta> canalesVenta,
        IEnumerable<TipoDia> tiposDia,
        IEnumerable<TipoHorario> tiposHorario,
        decimal valorAfecto,
        decimal valorExento,
        CancellationToken cancellationToken)
    {
        Producto? Producto = await _ProductoRepository.FindByIdAsync(ProductoInfo.Id, cancellationToken).ConfigureAwait(false);

        if (Producto is null)
        {
            return new ValidationError(nameof(TarifaProducto.Producto), "El Producto no existe");
        }


        var tarifasToCreate = (from canalVenta in canalesVenta
                               from tipoDia in tiposDia
                               from tipoHorario in tiposHorario
                               select (inicioVigencia, moneda, Producto, canalVenta, tipoDia, tipoHorario))
                               .ToList();

        var existentes = await _tarifaProductoRepository.FindByPrimaryKeysAsync(tarifasToCreate, cancellationToken).ConfigureAwait(false);

        tarifasToCreate = tarifasToCreate.Except(existentes).ToList();
        List<TarifaProducto> newTarifas = [];
        foreach (var create in tarifasToCreate)
        {
            newTarifas.Add(TarifaProducto.Create(
                create.inicioVigencia,
                create.moneda,
                create.Producto,
                create.canalVenta,
                create.tipoDia,
                create.tipoHorario,
                valorAfecto,
                valorExento));
        }

        return newTarifas;
    }

    /// <summary>
    /// Actualiza una tarifa de Producto.
    /// </summary>
    /// <param name="inicioVigencia">El inicio de vigencia de la tarifa a actualizar.</param>
    /// <param name="moneda">La momeda de la tarifa a actualizar.</param>
    /// <param name="Producto">El Producto de la tarifa a actualizar.</param>
    /// <param name="tramo">El tramo de la tarifa a actualizar.</param>
    /// <param name="canalVenta">El canal de venta de la tarifa a actualizar.</param>
    /// <param name="tipoDia">El tipo de día de la tarifa a actualizar.</param>
    /// <param name="tipoHorario">El tipo de horario de la tarifa a actualizar.</param>
    /// <param name="valorAfecto">El valor afecto de la tarifa.</param>
    /// <param name="valorExento">El valor exento de la tarifa.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>
    /// El resultado de la actualización de la tarifa de Producto.
    /// </returns>
    public async Task<ResultOf<TarifaProducto>> UpdateAsync(
        DateTime inicioVigencia,
        Moneda moneda,
        ProductoInfo Producto,
        CanalVenta canalVenta,
        TipoDia tipoDia,
        TipoHorario tipoHorario,
        decimal valorAfecto,
        decimal valorExento,
        CancellationToken cancellationToken)
    {
        TarifaProducto? tarifa = await _tarifaProductoRepository.FindByPrimaryKeyAsync(
            inicioVigencia,
            moneda.Id,
            Producto.Id,
            canalVenta.Id,
            tipoDia.Id,
            tipoHorario.Id,
            cancellationToken);

        if (tarifa is null)
        {
            return new NotFound();
        }

        tarifa.Update(valorAfecto, valorExento);

        return tarifa;
    }

}
