using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.TarifasProductos.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Components.Admin.Mantenedores.TarifasProducto.Controls;

/// <summary>
/// El modal para crear varias tarifas de Productos.
/// </summary>
public partial class TarifaProductoCreateDialog
{
    private MudForm form = default!;
    private bool loading;

    private TarifaProductoCreateModel tarifaProducto = new();

    /// <summary>
    /// Opciones para el modal.
    /// </summary>
    [Parameter]
    public DialogOptions? DialogOptions { get; set; }

    /// <summary>
    /// Si el modal está abierto o no.
    /// </summary>
    [Parameter]
    public bool IsOpen { get; set; }

    /// <summary>
    /// Evento que se dispara si <see cref="IsOpen"/> cambia.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// La lista de monedas.
    /// </summary>
    [Parameter]
    public IEnumerable<Moneda> Monedas { get; set; } = [];

    /// <summary>
    /// La lista de centros de costo.
    /// </summary>
    [Parameter]
    public IEnumerable<CentroCostoInfo> CentrosCosto { get; set; } = [];

    /// <summary>
    /// La lista de canales de venta.
    /// </summary>
    [Parameter]
    public IEnumerable<CanalVenta> CanalesVenta { get; set; } = [];

    /// <summary>
    /// Una función que busca los Productos que se pueden seleccionar.
    /// </summary>
    [Parameter]
    public Func<CentroCostoInfo, string?, CancellationToken, Task<IEnumerable<ProductoInfo>>> SearchProductos { get; set; } = (_, _, _) => Task.FromResult(Enumerable.Empty<ProductoInfo>());

    /// <summary>
    /// La lista de tipos de día.
    /// </summary>
    [Parameter]
    public IEnumerable<TipoDia> TiposDia { get; set; } = [];

    /// <summary>
    /// La lista de tipos de horario.
    /// </summary>
    [Parameter]
    public IEnumerable<TipoHorario> TiposHorario { get; set; } = [];

    /// <summary>
    /// Función para guardar los cambios.
    /// </summary>
    [Parameter]
    public Func<TarifaProductoCreateModel, Task<bool>> CommitChanges { get; set; } = (_) => Task.FromResult(true);

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        tarifaProducto.CentroCosto = CentrosCosto.FirstOrDefault() ?? new();
    }

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    private Task OnChangeCentroCostoAsync(CentroCostoInfo centroCosto)
    {
        tarifaProducto.CentroCosto = centroCosto;
        tarifaProducto.Producto = default!;
        return Task.CompletedTask;
    }

    private async Task<IEnumerable<ProductoInfo>> OnSearchProductosAsync(string? searchText, CancellationToken cancellationToken)
    {
        return SearchProductos is not null
            ? await SearchProductos(tarifaProducto.CentroCosto, searchText, cancellationToken)
            : [];
    }

    private async Task CancelEditingItemAsync()
    {
        await ChangeIsOpen(false);
        tarifaProducto = new();
    }

    private async Task CommitItemChangesAsync()
    {
        await form.Validate();

        if (!form.IsValid)
        {
            return;
        }

        if (CommitChanges is not null)
        {
            loading = true;
            bool result = await CommitChanges(tarifaProducto);

            if (result)
            {
                await ChangeIsOpen(false);
            }

            loading = false;
        }
    }
}
