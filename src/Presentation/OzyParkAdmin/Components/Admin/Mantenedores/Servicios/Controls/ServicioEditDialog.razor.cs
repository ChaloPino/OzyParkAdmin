using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Franquicias;
using OzyParkAdmin.Domain.Plantillas;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Controls;

/// <summary>
/// Modal para editar un servicio.
/// </summary>
public partial class ServicioEditDialog
{
    private const short ZeroShort = 0;
    private const short OneShort = 1;
    private const short MaxShort = short.MaxValue;
    private const int OneInt = 1;
    private const int MaxInt = int.MaxValue;
    private const byte ZeroByte = 0;
    private const byte MaxByte = byte.MaxValue;

    private MudForm form = default!;

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
    /// El servicios que se editará.
    /// </summary>
    [Parameter]
    public ServicioViewModel Servicio { get; set; } = new();

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
    /// Los tipos de control.
    /// </summary>
    [Parameter]
    public List<TipoControl> TiposControl { get; set; } = [];

    /// <summary>
    /// Los tipos de distribución.
    /// </summary>
    [Parameter]
    public List<TipoDistribucion> TiposDistribucion { get; set; } = [];

    /// <summary>
    /// Los tipos de vigencia.
    /// </summary>
    [Parameter]
    public List<TipoVigencia> TiposVigencia { get; set; } = [];

    /// <summary>
    /// Los servicios que se pueden usar como control parental.
    /// </summary>
    [Parameter]
    public List<ServicioInfo> Servicios { get; set; } = [];

    /// <summary>
    /// Las plantillas.
    /// </summary>
    [Parameter]
    public List<Plantilla> Plantillas { get; set; } = [];

    /// <summary>
    /// Función para cargar los servicios.
    /// </summary>
    [Parameter]
    public Func<int, Task> LoadServicios { get; set; } = (_) => Task.CompletedTask;

    /// <summary>
    /// Función para guardar los cambios.
    /// </summary>
    [Parameter]
    public Func<ServicioViewModel, Task<bool>> CommitChanges { get; set; } = (_) => Task.FromResult(true);

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    private async Task FranquiciaChanged(int franquiciaId)
    {
        Servicio.FranquiciaId = franquiciaId;

        if (LoadServicios is not null)
        {
            await LoadServicios(franquiciaId);
        }
    }

    private async Task CancelEditingItemAsync()
    {
        await ChangeIsOpen(false);
    }

    private async Task CommitItemChangesAsync()
    {
        await form.Validate();

        if (!form.IsValid)
        {
            return;
        }

        if (Servicio is not null && CommitChanges is not null)
        {
            bool result = await CommitChanges(Servicio);

            if (result)
            {
                await ChangeIsOpen(false);
            }
        }
    }
}
