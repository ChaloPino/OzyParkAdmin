using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Models;
using OzyParkAdmin.Domain.Productos;
using static MudBlazor.CategoryTypes;


namespace OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Controls;

public partial class CategoriaProductoEditDialog
{
    /// <summary>
    /// La Categoria a editar.
    /// </summary>
    [Parameter]
    public CategoriaProductoViewModel CategoriaProducto { get; set; } = new();

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

    /// <summary>
    /// Al inicio se ejecuta esto para verificar si debe o no mostar el modal y hacer carga de datos si se necesitara
    /// </summary>
    /// <returns></returns>
    protected override async Task OnParametersSetAsync()
    {
        if (IsOpen && CategoriaProducto?.IsNew == false && !CategoriaProducto.Loading)
        {
            CategoriaProducto.Loading = true;
            //Aca hacer carga de datos o preprar informacion que se desplegara en el form
            CategoriaProducto.Loading = false;
        }
    }

    /// <summary>
    /// Para cerrar modal sin hacer nada
    /// </summary>
    /// <returns></returns>
    private async Task CancelEditingItemAsync()
    {
        await ChangeIsOpen(false);
        Clean();
    }

    /// <summary>
    /// En caso de necesitar limpiar variables internas cuando se cierra el modal
    /// </summary>
    private void Clean()
    {
        /*
        categorias = [];
        complementos = [];
        _complementosSelecionados = [];
        _complementosProducto = [];
        */
    }
    /// <summary>
    /// Realizar validaciones y persistir los datos del formulario
    /// </summary>
    /// <returns></returns>
    private async Task CommitItemChangesAsync()
    {
        throw new NotImplementedException();
        /*
        await form.Validate();

        if (!form.IsValid)
        {
            return;
        }

        if (Producto is not null && CommitChanges is not null)
        {
            Producto.Complementos = [.. _complementosProducto];
            bool result = await CommitChanges(Producto);

            if (result)
            {
                await ChangeIsOpen(false);
                Clean();
            }
        }
        */
    }
}
