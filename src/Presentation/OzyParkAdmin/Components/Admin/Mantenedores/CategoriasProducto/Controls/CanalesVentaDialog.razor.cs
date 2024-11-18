using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using static MudBlazor.CategoryTypes;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Controls;

public partial class CanalesVentaDialog
{
    private IEnumerable<CanalVenta> canalesVentaSelected = []; //Listado donde quedarán los canales seleccionados con <MudSelect>

    /// <summary>
    /// La Categoria a agregar Canales de venta
    /// </summary>
    [Parameter]
    public CategoriaProductoViewModel CategoriaProducto { get; set; } = new();

    #region Parametros básicos para interactuar con el Modal 
    /// <summary>
    /// Función para guardar los cambios.
    /// </summary>
    [Parameter]
    public Func<CategoriaProductoViewModel, Task<bool>> CommitChanges { get; set; } = (_) => Task.FromResult(false);

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
    /// Para mostrar o no el Modal
    /// </summary>
    /// <param name="isOpen"></param>
    /// <returns></returns>
    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        await IsOpenChanged.InvokeAsync(isOpen);
    }
    #endregion

    /// <summary>
    /// Listado de Canales de venta
    /// </summary>
    [Parameter]
    public IEnumerable<CanalVenta> CanalesVenta { get; set; } = []; //Listado que es cargado desde index y transmitido aca mediante parametros


    /// <summary>
    /// Cargar el detalle de la categoria de producto para obtener los canales configurados para esta categoria de prodcuto.
    /// </summary>
    [Parameter]
    public Func<CategoriaProductoViewModel, Task> LoadCategoriaProductoDetalle { get; set; } = (_) => Task.CompletedTask;

    /// <summary>
    /// Al inicio se ejecuta esto para verificar si debe o no mostar el modal y hacer carga de datos si se necesitara
    /// </summary>
    /// <returns></returns>
    protected override async Task OnParametersSetAsync()
    {
        if (IsOpen && CategoriaProducto is not null)
        {
            CategoriaProducto.Loading = true;
            if (LoadCategoriaProductoDetalle is not null)
            {
                await LoadCategoriaProductoDetalle(CategoriaProducto);
            }
            CategoriaProducto.Loading = false;
            //Carga los canales de venta que tiene la categoriaProducto que se trajo desde BD
            canalesVentaSelected = new List<CanalVenta>(CategoriaProducto.CanalesVenta);
        }
    }

    /// <summary>
    /// Para cerrar modal sin hacer nada
    /// </summary>
    /// <returns></returns>
    private async Task CancelEditingItemAsync()
    {
        await ChangeIsOpen(false);
        canalesVentaSelected = [];
    }

    /// <summary>
    /// Realizar validaciones y persistir los datos del formulario
    /// </summary>
    /// <returns></returns>
    private async Task CommitItemChangesAsync()
    {
        CategoriaProducto.CanalesVenta = [.. canalesVentaSelected];
        bool result = await CommitChanges(CategoriaProducto);

        if (result)
        {
            await ChangeIsOpen(false);
        }
    }
}
