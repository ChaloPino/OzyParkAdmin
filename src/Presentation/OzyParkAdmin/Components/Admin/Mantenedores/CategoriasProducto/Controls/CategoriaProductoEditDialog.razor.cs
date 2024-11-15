using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Models;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.Franquicias;
using OzyParkAdmin.Domain.Productos;
using System.Runtime.CompilerServices;
using static MudBlazor.CategoryTypes;


namespace OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Controls;

public partial class CategoriaProductoEditDialog
{
    private const int OneInt = 1;
    private const int MaxInt = int.MaxValue;

    private MudForm form = default!;
    private List<CategoriaProductoInfo> categorias = [];

    /// <summary>
    /// La Categoria a editar.
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
    /// Las franquicias para llenar el MudSelect del Modal Vienen como parametro del index.razor.
    /// </summary>
    [Parameter]
    public List<FranquiciaInfo> Franquicias { get; set; } = [];

    /// <summary>
    /// Función para cargar las categorías.
    /// </summary>
    [Parameter]
    public Func<int, Task<List<CategoriaProductoInfo>>> LoadCategorias { get; set; } = (_) => Task.FromResult(new List<CategoriaProductoInfo>());

    /// <summary>
    /// Al inicio se ejecuta esto para verificar si debe o no mostar el modal y hacer carga de datos si se necesitara
    /// </summary>
    /// <returns></returns>
    protected override async Task OnParametersSetAsync()
    {
        if (IsOpen && CategoriaProducto?.IsNew == false && !CategoriaProducto.Loading)
        {
            CategoriaProducto.Loading = true;
            await OnLoadCategoriasAsync(CategoriaProducto.FranquiciaId);
            PrepareCategoriaProducto();
            CategoriaProducto.Loading = false;
        }
    }

    /// <summary>
    /// Carga el Nombre completo del producto que se esta editando
    /// </summary>
    private void PrepareCategoriaProducto() 
    {
        string? nombreCompleto = categorias.Where(w => w.Id == CategoriaProducto!.Padre.Id).Select(s => s.NombreCompleto).FirstOrDefault();
        if (!string.IsNullOrEmpty(nombreCompleto))
        {
            CategoriaProducto.Padre.NombreCompleto = nombreCompleto;
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
        categorias = [];
    }
    /// <summary>
    /// Realizar validaciones y persistir los datos del formulario
    /// </summary>
    /// <returns></returns>
    private async Task CommitItemChangesAsync()
    {

        await form.Validate();

        if (!form.IsValid)
        {
            return;
        }

        if (CategoriaProducto is not null && CommitChanges is not null)
        {
            bool result = await CommitChanges(CategoriaProducto);

            if (result)
            {
                await ChangeIsOpen(false);
                Clean();
            }
        }
    }

    private string GetFranquicia(int franquiciaId)
    {
        return Franquicias.Find(x => x.Id == franquiciaId)?.Nombre ?? "Seleccione franquicia";
    }

    private async Task FranquiciaChanged(int franquiciaId)
    {
        CategoriaProducto.FranquiciaId = franquiciaId;
        CategoriaProducto.Loading = true;
        await OnLoadCategoriasAsync(franquiciaId);
        CategoriaProducto.Loading = false;
    }

    private async Task OnLoadCategoriasAsync(int franquiciaId)
    {
        if (LoadCategorias is not null)
        {
            categorias = await LoadCategorias(franquiciaId);
        }
    }
}
