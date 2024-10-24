using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.CuposFecha.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CuposFecha.Controls;

/// <summary>
/// Modal para eliminar varios cupos por fecha.
/// </summary>
public partial class DeleteCuposFechaDialog
{
    private MudForm form = default!;
    private CuposFechaDeleteModel cuposFecha = new();

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
    public Func<CuposFechaDeleteModel, Task<bool>> CommitChanges { get; set; } = (_) => Task.FromResult(true);

    /// <summary>
    /// Las opciones para el modal.
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
        cuposFecha = new();
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    private async Task CommitItemChangesAsync()
    {
        if (cuposFecha is not null && CommitChanges is not null)
        {
            await form.Validate();

            if (form.IsValid)
            {
                cuposFecha.Loading = true;
                bool result = await CommitChanges(cuposFecha);

                if (result)
                {
                    await ChangeIsOpen(false);
                }

                cuposFecha.Loading = false;
            }
        }
    }

    private async Task CancelEditingItemAsync()
    {
        await ChangeIsOpen(false);
    }
}
