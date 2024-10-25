using Heron.MudCalendar;
using MassTransit;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Application.CanalesVenta.List;
using OzyParkAdmin.Application.CentrosCosto.List;
using OzyParkAdmin.Application.Cupos.Search;
using OzyParkAdmin.Application.Servicios.List;
using OzyParkAdmin.Application.Zonas.List;
using OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Zonas;
using System.Security.Claims;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Controls;

/// <summary>
/// El modal del calendario de cupos.
/// </summary>
public partial class CalendarDialog
{
    private readonly CalendarioSearchModel search = new();
    private readonly Dictionary<CalendarItem, CupoHoraInfo> cupoEvents = [];

    private MudForm form = default!;
    private ClaimsPrincipal user = default!;
    private List<CalendarItem> events = [];
    private bool loading;
    private List<CentroCostoInfo> centrosCosto = [];
    private List<CanalVenta> canalesVenta = [];
    private List<ServicioInfo> servicios = [];
    private List<ZonaInfo> zonas = [];
    private DateRange dateRange = default!;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    /// <summary>
    /// Indicador para saber si el modal está abierto o cerrado.
    /// </summary>
    [Parameter]
    public bool IsOpen { get; set; }

    /// <summary>
    /// Evento que se dispara cuando <see cref="IsOpen"/> cambia.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// Las opciones para el modal.
    /// </summary>
    [Parameter]
    public DialogOptions? DialogOptions { get; set; }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        user = (await AuthenticationState).User;
        dateRange = new CalendarDateRange(DateTime.Today, CalendarView.Week, DayOfWeek.Monday);
    }

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;

        if (isOpen)
        {
            await InitializeAsync();
        }

        await IsOpenChanged.InvokeAsync(isOpen);
    }

    /// <summary>
    /// Muestra el calendario.
    /// </summary>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    public Task ShowAsync() =>
        ChangeIsOpen(true);

    private async Task InitializeAsync()
    {
        loading = true;
        Task[] tasks = [LoadCentrosCostoAsync(), LoadCanalesVentaAsync(), LoadZonasAsync()];
        await Task.WhenAll(tasks);
        loading = false;
    }

    private async Task LoadCentrosCostoAsync()
    {
        var result = await Mediator.SendRequest(new ListCentrosCosto(user));
        result.Switch(
            onSuccess: list => centrosCosto = list,
            onFailure: failure => Snackbar.AddFailure(failure, "cargar los centros de costo"));
    }

    private async Task LoadCanalesVentaAsync()
    {
        var result = await Mediator.SendRequest(new ListCanalesVenta());
        result.Switch(
            onSuccess: list => canalesVenta = list,
            onFailure: failure => Snackbar.AddFailure(failure, "cargar los canales de venta"));
    }

    private async Task LoadZonasAsync()
    {
        var result = await Mediator.SendRequest(new ListZonas());
        result.Switch(
            onSuccess: list => zonas = list,
            onFailure: failure => Snackbar.AddFailure(failure, "cargar las zonas"));
    }

    private async Task CentroCostoChangedAsync(CentroCostoInfo centroCosto)
    {
        search.CentroCosto = centroCosto;
        var result = await Mediator.SendRequest(new ListServiciosPorCentroCosto(centroCosto.Id));
        result.Switch(
            onSuccess: list => servicios = list,
            onFailure: failure => Snackbar.AddFailure(failure, "cargar los servicios"));
    }

    private async Task SearchAsync()
    {
        await form.Validate();
        if (form.IsValid)
        {
            loading = true;
            var result = await Mediator.SendRequest(new SearchCalendario(
                    search.CanalVenta,
                    search.Alcance,
                    search.Servicio,
                    search.ZonaOrigen,
                    dateRange.Start,
                    dateRange.End
                ));

            cupoEvents.Clear();

            result.Switch(
                onSuccess: items => events = items.SelectMany(x => x.Horario, (fecha, horario) =>
                    {
                        var item = new CalendarItem
                        {
                            Start = fecha.Fecha.Date.Add(horario.HoraInicio),
                            End = fecha.Fecha.Date.Add(horario.HoraFin),
                            Text = horario.HayCupo ? $"Disponible ({horario.Disponible} de {horario.Total})" : "No disponible",
                            AllDay = false,
                        };

                        cupoEvents.Add(item, horario);
                        return item;
                    }).ToList(),
                onFailure: failure => Snackbar.AddFailure(failure, "buscar cupos"));

            loading = false;
        }
    }

    private async Task OnDateChangesAsync(DateRange range)
    {
        dateRange = range;
        await SearchAsync();
    }
}
