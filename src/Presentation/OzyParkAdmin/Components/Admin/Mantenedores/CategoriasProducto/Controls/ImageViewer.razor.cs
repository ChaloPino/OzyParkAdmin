using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Models;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Controls;

public partial class ImageViewer
{
    private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mud-width-full mud-height-full";
    private string _dragClass = DefaultDragClass;

    private MudFileUpload<IBrowserFile>? fileUpload;
    private int _anchoOriginal;
    private int _altoOriginal;
    private int _ancho;
    private int _alto;
    private float _aspectRatio;
    private bool _mantenerProporcion = true;

    /// <summary>
    /// El catálogo de imagen a editar.
    /// </summary>
    [Parameter]
    public CatalogoImagenModel Imagen { get; set; } = new();

    /// <summary>
    /// El tipo del catálogo de imagen.
    /// </summary>
    [Parameter]
    public string TipoCatalogo { get; set; } = "Categorias";

    /// <summary>
    /// La clase de estilo.
    /// </summary>
    [Parameter]
    public string Class { get; set; } = string.Empty;

    /// <summary>
    /// El título.
    /// </summary>
    [Parameter]
    public string Title { get; set; } = "Imagen";

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        SetSize();
    }

    private void SetDragClass()
        => _dragClass = $"{DefaultDragClass} mud-border-primary";

    private void ClearDragClass()
        => _dragClass = DefaultDragClass;

    private Task OpenFilePickerAsync()
        => fileUpload?.OpenFilePickerAsync() ?? Task.CompletedTask;

    private async Task SetFile(IBrowserFile? file)
    {
        await Imagen.SetFile(file, TipoCatalogo);
        SetSize();
    }

    private void SetSize()
    {
        if (!Imagen.HasSize)
        {
            (int ancho, int alto) = ImagenService.ConseguirAnchoAlto(Imagen.Base64, Imagen.MimeType);
            Imagen.SetSize(ancho, alto);
            _anchoOriginal = _ancho = ancho;
            _altoOriginal = _alto = alto;
            _aspectRatio = (float)_anchoOriginal / _altoOriginal;
        }
    }

    private async Task Resize()
    {
        Imagen.Base64 = await ImagenService.RedimensionarImagen(Imagen.Base64, Imagen.MimeType, _ancho, _alto);
    }

    private async Task Reset()
    {
        _ancho = _anchoOriginal;
        _alto = _altoOriginal;
        await Resize();
    }

    private async Task AnchoChanged(int ancho)
    {
        _ancho = ancho;

        if (_mantenerProporcion)
        {
            _alto = (int)(ancho / _aspectRatio);
        }

        await Resize();
    }

    private async Task AltoChanged(int alto)
    {
        _alto = alto;

        if (_mantenerProporcion)
        {
            _ancho = (int)(alto * _aspectRatio);
        }

        await Resize();
    }

}
