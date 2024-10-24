using Microsoft.AspNetCore.Components;
using MudBlazor.Interfaces;
using MudBlazor;
using OzyParkAdmin.Components.Admin.CajaControl.Cajas.Models;
using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Components.Admin.CajaControl.Cajas.Controls;

/// <summary>
/// El filtro del estado del día.
/// </summary>
public partial class EstadoDiaFilter
{
    private EstadoDia? estadoDia;

    [CascadingParameter]
    private MudDataGrid<AperturaCajaViewModel>? DataGrid { get; set; }

    private MudDataGrid<AperturaCajaViewModel>? CurrentDataGrid => DataGrid ?? FilterContext?.FilterDefinition?.Column?.DataGrid;

    /// <summary>
    /// El <see cref="FilterContext{T}"/>.
    /// </summary>
    [Parameter]
    public FilterContext<AperturaCajaViewModel>? FilterContext { get; set; }

    private static string[] Operators => [FilterOperator.Enum.Is, FilterOperator.Enum.IsNot];

    private string? Operator => FilterContext?.FilterDefinition?.Operator ?? Operators.FirstOrDefault();

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (FilterContext?.FilterDefinition is not null)
        {
            estadoDia = (EstadoDia?)FilterContext.FilterDefinition.Value;
        }
    }

    private string ChosenOperatorStyle(string o)
    {
        return o == Operator ? "color:var(--mud-palette-primary-text);background-color:var(--mud-palette-primary)" : "";
    }

    private async Task ChangeOperatorAsync(string o)
    {
        if (FilterContext?.FilterDefinition is not null)
        {
            FilterContext.FilterDefinition.Operator = o;
            await ApplyFilterAsync(FilterContext.FilterDefinition);
        }
    }

    private async Task EstadoDiaFilterChangedAsync(EstadoDia? value)
    {
        estadoDia = value;

        if (FilterContext?.FilterDefinition is not null)
        {
            if (estadoDia is not null)
            {
                FilterContext.FilterDefinition.Value = estadoDia;
                await ApplyFilterAsync(FilterContext.FilterDefinition);
            }
            else
            {
                await ClearFilterAsync();
            }
        }
    }

    private void EstadoDiaFilterChanged(EstadoDia? value)
    {
        estadoDia = value;

        if (FilterContext?.FilterDefinition is not null)
        {
            if (estadoDia is not null)
            {
                FilterContext.FilterDefinition.Value = estadoDia;
                CurrentDataGrid?.GroupItems();
            }
            else
            {
                FilterContext.FilterDefinition.Value = null;
            }
        }
    }

    private async Task ApplyFilterAsync()
    {
        if (CurrentDataGrid is not null)
        {
            if (CurrentDataGrid.FilterDefinitions.TrueForAll(x => x.Id != FilterContext!.FilterDefinition!.Id))
            {
                CurrentDataGrid.FilterDefinitions.Add(FilterContext!.FilterDefinition);
            }

            await CurrentDataGrid.ReloadServerData();

            CurrentDataGrid.GroupItems();
            ((IMudStateHasChanged)CurrentDataGrid).StateHasChanged();
        }
    }

    private async Task ApplyFilterAsync(IFilterDefinition<AperturaCajaViewModel> filterDefinition)
    {
        if (CurrentDataGrid is not null)
        {
            if (CurrentDataGrid.FilterDefinitions.TrueForAll(x => x.Id != filterDefinition.Id))
            {
                CurrentDataGrid.FilterDefinitions.Add(filterDefinition);
            }

            await CurrentDataGrid.ReloadServerData();

            CurrentDataGrid.GroupItems();
            ((IMudStateHasChanged)CurrentDataGrid).StateHasChanged();
        }
    }

    private async Task ClearFilterAsync()
    {
        if (FilterContext?.FilterDefinition is not null)
        {
            await FilterContext.Actions.ClearFilterAsync(FilterContext.FilterDefinition);
            ((IMudStateHasChanged)CurrentDataGrid!).StateHasChanged();
        }
    }
}
