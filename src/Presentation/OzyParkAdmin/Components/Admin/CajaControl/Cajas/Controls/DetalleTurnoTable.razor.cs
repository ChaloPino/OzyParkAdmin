using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Components.Admin.CajaControl.Cajas.Controls;

/// <summary>
/// La tabla del detalle de turnos.
/// </summary>
public partial class DetalleTurnoTable
{
    /// <summary>
    /// El detalle del turno.
    /// </summary>
    [Parameter]
    public IEnumerable<DetalleTurnoInfo> Detalle { get; set; } = [];

    /// <summary>
    /// Si se paginará.
    /// </summary>
    [Parameter]
    public bool Paginate { get; set; } = true;

    /// <summary>
    /// Si se sumarizará el monto.
    /// </summary>
    [Parameter]
    public bool SummarizeMonto { get; set; }

    private AggregateDefinition<DetalleTurnoInfo>? MontoAggregateDefinition =>
        SummarizeMonto
        ? new()
        {
            Type = AggregateType.Custom,
            CustomAggregate = Summarize
        }
        : null;

    private string Summarize(IEnumerable<DetalleTurnoInfo> detalles)
    {
        decimal total = Detalle.Sum(x => x.Monto);

        if (Paginate)
        {
            decimal totalPaginado = detalles.Sum(x => x.Monto);

            if (totalPaginado != total)
            {
                return $"{totalPaginado:C0} ({total:C0})";
            }
        }

        return $"{total:C0}";
    }
}
