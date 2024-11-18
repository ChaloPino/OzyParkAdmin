using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Models;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Controls;

public partial class CanalesVentaDialog
{
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
}
