using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Controls;

/// <summary>
/// El modal para crear cupos.
/// </summary>
public partial class CreateCuposDialog
{
    private MudForm form = default!;
    private CuposModel cupos = new();

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
    /// La función para guardar los cambios.
    /// </summary>
    [Parameter]
    public Func<CuposModel, Task<bool>> CommitChanges { get; set; } = (_) => Task.FromResult(true);

    /// <summary>
    /// Las opciones del diálogo.
    /// </summary>
    [Parameter]
    public DialogOptions? DialogOptions { get; set; }

    /// <summary>
    /// Los escenarios de cupo.
    /// </summary>
    [Parameter]
    public List<EscenarioCupoInfo> EscenariosCupo { get; set; } = [];

    /// <summary>
    /// Los canales de venta.
    /// </summary>
    [Parameter]
    public List<CanalVenta> CanalesVenta { get; set; } = [];

    /// <summary>
    /// Los días de semana.
    /// </summary>
    [Parameter]
    public List<DiaSemana> DiasSemana { get; set; } = [];

    private static string CanalesVentaDescriptions(List<string> canalesVenta) => canalesVenta switch
    {
        { Count: 0 } => "Seleccione canales de venta",
        { Count: 1 } => canalesVenta[0],
        _ => $"{canalesVenta.Count} canales de venta seleccionados",
    };

    private static string DiasSemanaDescriptions(List<string> diasSemana) => diasSemana switch
    {
        { Count: 0 } => "Seleccione días de semana",
        { Count: 1 } => diasSemana[0],
        _ => $"{diasSemana.Count} días de semana seleccionados",
    };

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        cupos = new();
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    private async Task CommitItemChangesAsync()
    {
        await form.Validate();

        if (form.IsValid)
        {
            cupos.Loading = true;
            bool result = await CommitChanges(cupos);

            if (result)
            {
                await ChangeIsOpen(false);
            }

            cupos.Loading = false;
        }
    }

    private async Task CancelEditingItemAsync()
    {
        await ChangeIsOpen(false);
    }
}
