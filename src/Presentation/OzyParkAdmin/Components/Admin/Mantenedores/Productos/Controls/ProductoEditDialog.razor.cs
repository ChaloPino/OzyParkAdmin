using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.Productos.Models;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Contabilidad;
using OzyParkAdmin.Domain.Franquicias;
using OzyParkAdmin.Domain.Productos;
using System.Collections.ObjectModel;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Productos.Controls;

/// <summary>
/// Modal para editar o crear un producto.
/// </summary>
public partial class ProductoEditDialog
{
    private const int OneInt = 1;
    private const int MaxInt = int.MaxValue;
    private List<CategoriaProductoInfo> categorias = [];
    private List<ProductoInfo> complementos = [];

    private MudForm form = default!;

    private List<ProductoInfo> _complementosSelecionados = [];
    private ObservableCollection<ProductoComplementarioModel> _complementosProducto = [];

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
    /// El producto a editar.
    /// </summary>
    [Parameter]
    public ProductoViewModel Producto { get; set; } = new();

    /// <summary>
    /// Los centros de costo.
    /// </summary>
    [Parameter]
    public List<CentroCostoInfo> CentrosCosto { get; set; } = [];

    /// <summary>
    /// Las franquicias.
    /// </summary>
    [Parameter]
    public List<FranquiciaInfo> Franquicias { get; set; } = [];

    /// <summary>
    /// Las familias.
    /// </summary>
    [Parameter]
    public List<AgrupacionContable> Familias { get; set; } = [];

    /// <summary>
    /// Los tipos de producto.
    /// </summary>
    [Parameter]
    public List<TipoProducto> TiposProducto { get; set; } = [];

    /// <summary>
    /// Función para cargar las categorías.
    /// </summary>
    [Parameter]
    public Func<int, Task<List<CategoriaProductoInfo>>> LoadCategorias { get; set; } = (_) => Task.FromResult(new List<CategoriaProductoInfo>());

    /// <summary>
    /// Función para cargar los complementos de un producto.
    /// </summary>
    [Parameter]
    public Func<int, int, Task<List<ProductoInfo>>> LoadComplementos { get; set; } = (_, _) => Task.FromResult(new List<ProductoInfo>());

    /// <summary>
    /// Función para guardar los cambios.
    /// </summary>
    [Parameter]
    public Func<ProductoViewModel, Task<bool>> CommitChanges { get; set; } = (_) => Task.FromResult(false);

    private bool PuedeAsociarComplementos => complementos.Count > 0 && !Producto.EsComplemento;

    /// <inheritdoc/>
    protected override async Task OnParametersSetAsync()
    {
        if (IsOpen && Producto?.IsNew == false && !Producto.Loading)
        {
            Producto.Loading = true;
            await OnLoadCategoriasAsync(Producto.FranquiciaId);
            await OnLoadComplementosAsync(Producto.Categoria);
            PrepareProducto();
            PrepareComplementos();
            Producto.Loading = false;
        }
    }

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    /// <summary>
    /// Prepara el producto asignando los nhombre completos de categoría y categoría despliegue.
    /// </summary>
    /// <remarks>
    /// La busqueda se hace en la clase <see cref="CategoriaProductoInfo" /> el método <see cref="CategoriaProductoInfo.Equals(CategoriaProductoInfo?)" />.
    /// </remarks>
    private void PrepareProducto()
    {
        int categoriaId = categorias.IndexOf(Producto!.Categoria);
        int categoriaDespliegueId = categorias.IndexOf(Producto!.CategoriaDespliegue);

        if (categoriaId >= 0)
        {
            Producto.Categoria.NombreCompleto = categorias[categoriaId].NombreCompleto;
        }

        if (categoriaDespliegueId >= 0)
        {
            Producto.CategoriaDespliegue.NombreCompleto = categorias[categoriaDespliegueId].NombreCompleto;
        }
    }

    private void PrepareComplementos()
    {
        _complementosProducto = [.. Producto.Complementos];
        _complementosSelecionados = (from complemento in complementos
                                     join productoComplemento in _complementosProducto on complemento.Id equals productoComplemento.Complemento.Id
                                     select complemento).ToList();
    }

    private string GetFranquicia(int franquiciaId)
    {
        return Franquicias.Find(x => x.Id == franquiciaId)?.Nombre ?? "Seleccione franquicia";
    }

    private async Task FranquiciaChanged(int franquiciaId)
    {
        Producto.FranquiciaId = franquiciaId;
        Producto.Loading = true;
        await OnLoadCategoriasAsync(franquiciaId);
        Producto.Loading = false;
    }

    private async Task OnLoadCategoriasAsync(int franquiciaId)
    {
        if (LoadCategorias is not null)
        {
            categorias = await LoadCategorias(franquiciaId);
        }
    }

    private async Task CategoriaChanged(CategoriaProductoInfo categoria)
    {
        Producto.Categoria = categoria;
        Producto.Loading = true;
        await OnLoadComplementosAsync(categoria);
        Producto.Loading = false;
    }

    private async Task OnLoadComplementosAsync(CategoriaProductoInfo categoria)
    {
        if (LoadComplementos is not null)
        {
            complementos = await LoadComplementos(categoria.Id, Producto.Id);
        }
    }

    private void EsComplementoChanged(bool esComplemento)
    {
        Producto.EsComplemento = esComplemento;
        TryCleanComplementos();
    }

    private void TryCleanComplementos()
    {
        if (Producto.EsComplemento)
        {
            _complementosProducto = [];
            _complementosSelecionados = [];
            Producto.Complementos = [];
        }
    }

    private void SelectComplementos(IReadOnlyCollection<ProductoInfo> productos)
    {
        _complementosSelecionados = [.. productos];

        List<ProductoInfo> toAdd = (from producto in _complementosSelecionados
                                    join complemento in _complementosProducto on producto.Id equals complemento.Complemento.Id into defComplementos
                                    from defComplemento in defComplementos.DefaultIfEmpty()
                                    where defComplemento is null
                                    select producto).ToList();

        List<ProductoComplementarioModel> toRemove = (from complemento in _complementosProducto
                                                      join producto in _complementosSelecionados on complemento.Complemento.Id equals producto.Id into defProductos
                                                      from defProducto in defProductos.DefaultIfEmpty()
                                                      where defProducto is null
                                                      select complemento).ToList();

        toRemove.ForEach(x => _complementosProducto.Remove(x));
        toAdd.ForEach(x => _complementosProducto.Add(new ProductoComplementarioModel { Complemento = x, Orden = _complementosProducto.Count }));
    }

    private void RemoveComplemento(CellContext<ProductoComplementarioModel> context)
    {
        var complemento = _complementosProducto.FirstOrDefault(x => x.Complemento.Id == context.Item.Complemento.Id);

        if (complemento is not null)
        {
            _complementosProducto.Remove(complemento);
            _complementosSelecionados = _complementosSelecionados.Where(x => x.Id != complemento.Complemento.Id).ToList();
        }
    }

    private async Task CancelEditingItemAsync()
    {
        await ChangeIsOpen(false);
        Clean();
    }

    private async Task CommitItemChangesAsync()
    {
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
    }

    private void Clean()
    {
        categorias = [];
        complementos = [];
        _complementosSelecionados = [];
        _complementosProducto = [];
    }
}
