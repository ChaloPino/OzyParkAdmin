using MassTransit;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Application.DetallesEscenariosCuposExclusiones.List;
using OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Controls;

public partial class CreateEscenarioCupoExclusionsDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public List<ServicioInfo> Servicios { get; set; } = new();
    [Parameter] public List<CanalVenta> CanalesVenta { get; set; } = new();
    [Parameter] public List<DiaSemana> DiasSemana { get; set; } = new();
    [Parameter] public EscenarioCupoModel SelectedEscenarioCupo { get; set; } = default!;
    [Parameter] public DetalleEscenarioCupoExclusionModel? ExclusionToEdit { get; set; } = null;

    private List<DetalleEscenarioCupoExclusionFullInfo> Exclusiones { get; set; }

    private DetalleEscenarioCupoExclusionModel Model { get; set; } = new DetalleEscenarioCupoExclusionModel();
    private MudForm _form;
    private bool _valid;
    private bool IsEditMode => ExclusionToEdit != null;

    // Para selección múltiple
    private IEnumerable<ServicioInfo> ServiciosSeleccionados = [];
    private IEnumerable<CanalVenta> CanalesVentaSeleccionados = [];
    private IEnumerable<DiaSemana> DiasSemanaSeleccionados = [];

    /// <summary>
    /// Guarda los cambios realizados en el formulario de exclusión del escenario de cupo.
    /// Este método se invoca cuando se ha completado correctamente la validación del formulario.
    /// </summary>
    private async Task SaveAsync()
    {

        // Generar modelos para cada combinación seleccionada evitando duplicados
        var modelos = new List<DetalleEscenarioCupoExclusionFullInfo>();

        // Valida el formulario antes de proceder
        await _form.Validate();
        if (_valid)
        {
            if (IsEditMode && ExclusionToEdit != null)
            {
                // Actualizar la exclusión existente
                ExclusionToEdit.ServicioId = ServiciosSeleccionados.FirstOrDefault()?.Id ?? ExclusionToEdit.ServicioId;
                ExclusionToEdit.ServicioNombre = ServiciosSeleccionados.FirstOrDefault()?.Nombre ?? ExclusionToEdit.ServicioNombre;
                ExclusionToEdit.CanalVentaId = CanalesVentaSeleccionados.FirstOrDefault()?.Id ?? ExclusionToEdit.CanalVentaId;
                ExclusionToEdit.CanalVentaNombre = CanalesVentaSeleccionados.FirstOrDefault()?.Nombre ?? ExclusionToEdit.CanalVentaNombre;
                ExclusionToEdit.DiaSemanaId = DiasSemanaSeleccionados.FirstOrDefault()?.Id ?? ExclusionToEdit.DiaSemanaId;
                ExclusionToEdit.DiaSemanaNombre = DiasSemanaSeleccionados.FirstOrDefault()?.Aka ?? ExclusionToEdit.DiaSemanaNombre;
                ExclusionToEdit.HoraInicio = Model.HoraInicio;
                ExclusionToEdit.HoraFin = Model.HoraFin;
            }
            else
            {
                foreach (var servicio in ServiciosSeleccionados)
                {
                    foreach (var canal in CanalesVentaSeleccionados)
                    {
                        foreach (var dia in DiasSemanaSeleccionados)
                        {
                            if (!Exclusiones.Any(existing =>
                                existing.ServicioId == servicio.Id &&
                                existing.CanalVentaId == canal.Id &&
                                existing.DiaSemanaId == dia.Id &&
                                existing.HoraInicio == Model.HoraInicio &&
                                existing.HoraFin == Model.HoraFin))
                            {
                                modelos.Add(new DetalleEscenarioCupoExclusionFullInfo
                                {
                                    ServicioId = servicio.Id,
                                    ServicioNombre = servicio.Nombre,
                                    CanalVentaId = canal.Id,
                                    CanalVentaNombre = canal.Nombre,
                                    DiaSemanaId = dia.Id,
                                    DiaSemanaNombre = dia.Aka,
                                    HoraInicio = Model.HoraInicio,
                                    HoraFin = Model.HoraFin
                                });
                            }
                        }
                    }
                }
            }
            // Cierra el diálogo con el resultado Ok
            MudDialog.Close(DialogResult.Ok(modelos));
        }
    }

    /// <summary>
    /// Cancela la creación o edición de la exclusión y cierra el diálogo sin guardar cambios.
    /// </summary>
    private void Cancel() => MudDialog.Cancel();

    /// <summary>
    /// Inicializa el componente y establece los valores iniciales necesarios.
    /// </summary>
    protected override void OnInitialized()
    {
        if (IsEditMode && ExclusionToEdit != null)
        {
            // Configuración para edición
            ServiciosSeleccionados = Servicios.Where(s => s.Id == ExclusionToEdit.ServicioId).ToList();
            CanalesVentaSeleccionados = CanalesVenta.Where(c => c.Id == ExclusionToEdit.CanalVentaId).ToList();
            DiasSemanaSeleccionados = DiasSemana.Where(d => d.Id == ExclusionToEdit.DiaSemanaId).ToList();
            Model.HoraInicio = ExclusionToEdit.HoraInicio;
            Model.HoraFin = ExclusionToEdit.HoraFin;
        }
    }

    private async Task ListExclusionesEscenarioCupo()
    {
        ListDetalleEscenarioCupoExclsiones list = new(SelectedEscenarioCupo.Id);

        var result = await Mediator.SendRequest(list);

    }
}