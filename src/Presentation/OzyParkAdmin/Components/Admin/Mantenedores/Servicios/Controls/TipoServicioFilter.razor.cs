using Microsoft.AspNetCore.Components;
using MudBlazor.Interfaces;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Controls;

/// <summary>
/// Filtro para el tipo de servicio.
/// </summary>
public partial class TipoServicioFilter
{
    private TipoServicio? tipoServicio;

    [CascadingParameter]
    private MudDataGrid<ServicioViewModel>? DataGrid { get; set; }

    private MudDataGrid<ServicioViewModel>? CurrentDataGrid => DataGrid ?? FilterContext?.FilterDefinition?.Column?.DataGrid;

    /// <summary>
    /// El <see cref="FilterContext{T}"/>.
    /// </summary>
    [Parameter]
    public FilterContext<ServicioViewModel>? FilterContext { get; set; }

    private static string[] Operators => [FilterOperator.Enum.Is, FilterOperator.Enum.IsNot];

    private string? Operator => FilterContext?.FilterDefinition?.Operator ?? Operators.FirstOrDefault();

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (FilterContext?.FilterDefinition is not null)
        {
            tipoServicio = (TipoServicio?)FilterContext.FilterDefinition.Value;
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

    private async Task TipoServicioFilterChangedAsync(TipoServicio? value)
    {
        tipoServicio = value;

        if (FilterContext?.FilterDefinition is not null)
        {
            if (tipoServicio is not null)
            {
                FilterContext.FilterDefinition.Value = tipoServicio;
                await ApplyFilterAsync(FilterContext.FilterDefinition);
            }
            else
            {
                await ClearFilterAsync();
            }
        }
    }

    private void TipoServicioFilterChanged(TipoServicio? value)
    {
        tipoServicio = value;

        if (FilterContext?.FilterDefinition is not null)
        {
            if (tipoServicio is not null)
            {
                FilterContext.FilterDefinition.Value = tipoServicio;
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

    private async Task ApplyFilterAsync(IFilterDefinition<ServicioViewModel> filterDefinition)
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
