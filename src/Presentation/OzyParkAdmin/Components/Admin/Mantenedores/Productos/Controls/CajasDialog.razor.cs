using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.Productos.Models;
using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Productos.Controls;

/// <summary>
/// El modal para asignar cajas a los productos.
/// </summary>
public partial class CajasDialog
{
    private IEnumerable<CajaInfo> cajas = [];

    /// <summary>
    /// Las opciones del modal.
    /// </summary>
    [Parameter]
    public DialogOptions? DialogOptions { get; set; }

    /// <summary>
    /// Si el modal está abierto o no.
    /// </summary>
    [Parameter]
    public bool IsOpen { get; set; }

    /// <summary>
    /// Evento que se dispara cuando <see cref="IsOpen"/> cambia.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// El producto a modificar
    /// </summary>
    [Parameter]
    public ProductoViewModel Producto { get; set; } = new();

    /// <summary>
    /// Las cajas.
    /// </summary>
    [Parameter]
    public IEnumerable<CajaInfo> Cajas { get; set; } = [];

    /// <summary>
    /// Función para cargar el detalle del producto.
    /// </summary>
    [Parameter]
    public Func<ProductoViewModel, Task> LoadProductoDetalle { get; set; } = (_) => Task.CompletedTask;

    /// <summary>
    /// Función para guardar los cambios.
    /// </summary>
    [Parameter]
    public Func<ProductoViewModel, Task<bool>> CommitChanges { get; set; } = (_) => Task.FromResult(true);

    /// <inheritdoc/>
    protected override async Task OnParametersSetAsync()
    {
        if (IsOpen && Producto is not null)
        {
            Producto.Loading = true;
            await LoadProductoDetalleAsync();
            Producto.Loading = false;
            cajas = new List<CajaInfo>(Producto.Cajas);
        }
    }

    private async Task LoadProductoDetalleAsync()
    {
        if (LoadProductoDetalle is not null && Producto is not null)
        {
            await LoadProductoDetalle(Producto);
        }
    }

    private static string GetCajasNames(List<string> cajas) => cajas.Count <= 3
        ? string.Join(", ", cajas)
        : $"{cajas.Count} cajas asignadas";

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    private async Task CancelEditingItemAsync()
    {
        cajas = [];
        await ChangeIsOpen(false);
    }

    private async Task CommitItemChangesAsync()
    {
        Producto.Cajas = [.. cajas];
        bool result = await CommitChanges(Producto);

        if (result)
        {
            await ChangeIsOpen(false);
        }

    }
}
