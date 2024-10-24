using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Components.Admin.CajaControl.Cajas.Controls;

/// <summary>
/// La tabla de pasajeros y guías
/// </summary>
public partial class PasajerosGuiasTable
{
    /// <summary>
    /// Los servicios.
    /// </summary>
    [Parameter]
    public IEnumerable<ServicioDiaInfo> Servicios { get; set; } = [];

    /// <summary>
    /// Si se paginará o no la grilla.
    /// </summary>
    [Parameter]
    public bool Paginate { get; set; } = true;

    private AggregateDefinition<ServicioDiaInfo> PasajerosAggregateDefinition => new()
    {
        Type = AggregateType.Custom,
        CustomAggregate = (items) => Summarize(items, x => x.Cantidad),
    };

    private AggregateDefinition<ServicioDiaInfo> GuiasAggregateDefinition => new()
    {
        Type = AggregateType.Custom,
        CustomAggregate = (items) => Summarize(items, x => x.Guias),
    };

    private string Summarize(IEnumerable<ServicioDiaInfo> servicios, Func<ServicioDiaInfo, int> selector)
    {
        int total = Servicios.Sum(selector);

        if (Paginate)
        {
            decimal totalPaginado = servicios.Sum(selector);

            if (totalPaginado != total)
            {
                return $"{totalPaginado} ({total})";
            }
        }

        return $"{total}";
    }
}
