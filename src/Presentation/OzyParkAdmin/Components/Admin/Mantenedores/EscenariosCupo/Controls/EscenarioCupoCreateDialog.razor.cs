using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Controls;

/// <summary>
/// Modal para crear o editar un escenario de cupo, incluyendo sus detalles y exclusiones.
/// </summary>
public partial class EscenarioCupoCreateDialog
{
    // Componente que contiene el formulario para crear el escenario de cupo.
    private CreateEscenarioCupoLayout createEscenarioCupoForm = default!;

    // Modelo del escenario de cupo que se está creando o editando.
    private EscenarioCupoModel escenarioCupo = new();

    // Propiedad para determinar si se han agregado detalles al escenario.
    private bool detallesAgregados = false;

    // Propiedad para determinar si se han agregado exclusiones al escenario.
    private bool exclusionesAgregadas = false;

    /// <summary>
    /// Indica si el modal está abierto.
    /// </summary>
    [Parameter]
    public bool IsOpen { get; set; }

    /// <summary>
    /// Evento para notificar cambios en el estado de apertura del modal.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// Función para confirmar los cambios en el escenario de cupo.
    /// </summary>
    [Parameter]
    public Func<EscenarioCupoModel, Task<bool>> CommitChanges { get; set; } = (_) => Task.FromResult(true);

    /// <summary>
    /// El modelo de vista utilizado para gestionar el estado del componente.
    /// </summary>
    [Parameter]
    public EscenarioCupoViewModel ViewModel { get; set; } = default!;

    /// <summary>
    /// Opciones del diálogo (MudBlazor).
    /// </summary>
    [Parameter]
    public DialogOptions? DialogOptions { get; set; }

    /// <summary>
    /// Lista de centros de costo disponibles para el escenario de cupo.
    /// </summary>
    [Parameter]
    public List<CentroCostoInfo> CentrosCostos { get; set; } = new();

    /// <summary>
    /// Lista de zonas disponibles para el escenario de cupo.
    /// </summary>
    [Parameter]
    public List<ZonaInfo> Zonas { get; set; } = new();

    /// <summary>
    /// Lista de servicios disponibles para los detalles y exclusiones del escenario de cupo.
    /// </summary>
    [Parameter]
    public List<ServicioInfo> Servicios { get; set; } = new();

    /// <summary>
    /// Lista de canales de venta disponibles para las exclusiones del escenario de cupo.
    /// </summary>
    [Parameter]
    public List<CanalVenta> CanalesVenta { get; set; } = new();

    /// <summary>
    /// Propiedad para determinar si el escenario de cupo es válido para ser guardado.
    /// Requiere al menos un detalle agregado y todos los campos básicos completos.
    /// </summary>
    private bool EsValidoParaGuardar =>
        createEscenarioCupoForm?.formEscenarioCupoValid == true && detallesAgregados;

    /// <summary>
    /// Cambia el estado de apertura del modal.
    /// </summary>
    /// <param name="isOpen">Si el modal está abierto o cerrado.</param>
    /// <returns>Tarea asincrónica.</returns>
    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        if (!isOpen)
        {
            escenarioCupo = new();
            detallesAgregados = false;
            exclusionesAgregadas = false;
        }
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    /// <summary>
    /// Confirma los cambios realizados en el escenario de cupo.
    /// </summary>
    /// <returns>Tarea asincrónica.</returns>
    private async Task CommitItemChangesAsync()
    {
        if (EsValidoParaGuardar)
        {
            ViewModel.Loading = true;
            bool result = await CommitChanges(escenarioCupo);

            if (result)
            {
                await ChangeIsOpen(false);
            }

            ViewModel.Loading = false;
        }
    }

    /// <summary>
    /// Cancela la edición actual del escenario de cupo y cierra el modal.
    /// </summary>
    /// <returns>Tarea asincrónica.</returns>
    private async Task CancelEditingItemAsync()
    {
        await ChangeIsOpen(false);
    }

    /// <summary>
    /// Método para manejar el evento cuando se agregan detalles.
    /// </summary>
    private void OnDetalleAgregado(List<DetalleEscenarioCupoInfo> detalles)
    {

        List<DetalleEscenarioCupoInfo> toAdd = new();

        escenarioCupo.Detalles.ForEach(e =>
        {
            if (!detalles.Contains(e))
            {
                toAdd.Add(e);
            }
        });
        if (toAdd.Count != 0)
        {
            escenarioCupo.Detalles.AddRange(toAdd);
        }
        detallesAgregados = escenarioCupo.Detalles.Any();
        StateHasChanged();
    }

    /// <summary>
    /// Método para manejar el evento cuando se agregan exclusiones.
    /// </summary>
    private void OnExclusionAgregada(List<DetalleEscenarioCupoExclusionFechaFullInfo> exclusiones)
    {
        List<DetalleEscenarioCupoExclusionFechaFullInfo> toAdd = new();

        escenarioCupo.ExclusionesFecha.ForEach(e =>
        {
            if (!exclusiones.Contains(e))
            {
                toAdd.Add(e);
            }
        });

        if (toAdd.Count != 0)
        {
            escenarioCupo.ExclusionesFecha.AddRange(toAdd);
        }
        exclusionesAgregadas = escenarioCupo.ExclusionesFecha.Any();
        StateHasChanged();
    }
}
