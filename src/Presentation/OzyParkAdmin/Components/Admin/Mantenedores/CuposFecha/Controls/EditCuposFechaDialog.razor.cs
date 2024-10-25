using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.CuposFecha.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CuposFecha.Controls;

/// <summary>
/// Modal para editar varios cupos por fecha.
/// </summary>
public partial class EditCuposFechaDialog
{
    private CuposFechaEditModel cuposFeha = new();
    private MudForm form = default!;

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
    public Func<CuposFechaEditModel, Task<bool>> CommitChanges { get; set; } = (_) => Task.FromResult(true);

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

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        cuposFeha = new();
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    private async Task CommitItemChangesAsync()
    {
        await form.Validate();

        if (form.IsValid)
        {
            cuposFeha.Loading = true;
            bool result = await CommitChanges(cuposFeha);

            if (result)
            {
                await ChangeIsOpen(false);
            }

            cuposFeha.Loading = false;
        }
    }

    private async Task CancelEditingItemAsync()
    {
        await ChangeIsOpen(false);
    }
}
