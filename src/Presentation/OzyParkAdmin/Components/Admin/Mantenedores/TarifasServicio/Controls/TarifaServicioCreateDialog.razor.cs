using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.TarifasServicio.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Components.Admin.Mantenedores.TarifasServicio.Controls;

/// <summary>
/// El modal para crear varias tarifas de servicios.
/// </summary>
public partial class TarifaServicioCreateDialog
{
    private MudForm form = default!;
    private bool loading;

    private TarifaServicioCreateModel tarifaServicio = new();

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
    /// Una función que busca los servicios que se pueden seleccionar.
    /// </summary>
    [Parameter]
    public Func<CentroCostoInfo, string?, CancellationToken, Task<IEnumerable<ServicioWithDetailInfo>>> SearchServicios { get; set; } = (_ , _, _) => Task.FromResult(Enumerable.Empty<ServicioWithDetailInfo>());

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
    /// La lista de tipos de segmentación.
    /// </summary>
    [Parameter]
    public IEnumerable<TipoSegmentacion> TiposSegmentacion { get; set; } = [];

    /// <summary>
    /// Función para guardar los cambios.
    /// </summary>
    [Parameter]
    public Func<TarifaServicioCreateModel, Task<bool>> CommitChanges { get; set; } = (_) => Task.FromResult(true);

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        tarifaServicio.CentroCosto = CentrosCosto.FirstOrDefault() ?? new();
    }

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    private Task OnChangeCentroCostoAsync(CentroCostoInfo centroCosto)
    {
        tarifaServicio.CentroCosto = centroCosto;
        tarifaServicio.Servicio = default!;
        tarifaServicio.Tramos = [];
        tarifaServicio.GruposEtarios = [];
        return Task.CompletedTask;
    }

    private async Task<IEnumerable<ServicioWithDetailInfo>> OnSearchServiciosAsync(string? searchText, CancellationToken cancellationToken)
    {
        return SearchServicios is not null
            ? await SearchServicios(tarifaServicio.CentroCosto, searchText, cancellationToken)
            : [];
    }

    private async Task CancelEditingItemAsync()
    {
        await ChangeIsOpen(false);
        tarifaServicio = new();
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
            bool result = await CommitChanges(tarifaServicio);

            if (result)
            {
                await ChangeIsOpen(false);
            }

            loading = false;
        }
    }
}
