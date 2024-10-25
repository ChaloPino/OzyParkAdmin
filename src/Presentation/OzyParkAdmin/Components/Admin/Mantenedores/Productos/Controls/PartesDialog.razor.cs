using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.Productos.Models;
using OzyParkAdmin.Domain.Productos;
using System.Collections.ObjectModel;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Productos.Controls;

/// <summary>
/// El modal para asignar partes a un producto.
/// </summary>
public partial class PartesDialog
{
    private MudForm form = default!;

    private ObservableCollection<ProductoParteModel> partes = [];
    private List<ProductoInfo> productosParte { get; set; } = [];

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
    /// El producto al que se le van a asignar las partes.
    /// </summary>
    [Parameter]
    public ProductoViewModel Producto { get; set; } = new();

    /// <summary>
    /// Función para cargar los productos que pueden ser partes del producto.
    /// </summary>
    [Parameter]
    public Func<int, int, Task<List<ProductoInfo>>> LoadProductosParte { get; set; } = (_, _) => Task.FromResult(new List<ProductoInfo>());

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

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    /// <inheritdoc/>
    protected override async Task OnParametersSetAsync()
    {
        if (IsOpen && Producto is not null)
        {
            Producto.Loading = true;
            Task[] tasks = [LoadProductoDetalleAsync(), LoadProductosParteAsync()];
            await Task.WhenAll(tasks);
            partes = new(Producto.Partes);
            Producto.Loading = false;
        }
    }

    private async Task LoadProductoDetalleAsync()
    {
        if (LoadProductoDetalle is not null && Producto is not null)
        {
            await LoadProductoDetalle(Producto);
        }
    }

    private async Task LoadProductosParteAsync()
    {
        if (LoadProductosParte is not null && Producto is not null)
        {
            productosParte = await LoadProductosParte(Producto.FranquiciaId, Producto.Id);
        }
    }

    private async Task<IEnumerable<ProductoInfo>> SearchProducto(string? text, CancellationToken cancellationToken)
    {
        await Task.Delay(5, cancellationToken);

        int[] partesId = partes.Where(x => x.Parte is not null).Select(x => x.Parte.Id).ToArray();

        if (string.IsNullOrWhiteSpace(text))
        {
            return productosParte.FindAll(x => !partesId.Contains(x.Id));
        }

        return productosParte.FindAll(x =>
            !partesId.Contains(x.Id) &&
            (x.Sku.Contains(text, StringComparison.OrdinalIgnoreCase) ||
            x.Nombre.Contains(text, StringComparison.OrdinalIgnoreCase)));
    }

    private static string GetProductoName(ProductoInfo? producto) =>
        producto is null ? string.Empty : $"{producto.Nombre} ({producto.Sku})";

    private void AddParte()
    {
        partes.Add(new ProductoParteModel());
    }

    private void RemoveParte(ProductoParteModel parte)
    {
        partes.Remove(parte);
    }

    private async Task CancelEditingItemAsync()
    {
        partes = [];
        await ChangeIsOpen(false);
    }

    private async Task CommitItemChangesAsync()
    {
        await form.Validate();

        if (form.IsValid)
        {
            Producto.Partes = [.. partes];
            bool result = await CommitChanges(Producto);

            if (result)
            {
                await ChangeIsOpen(false);
            }
        }
    }
}
