using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Reportes.Models;
using OzyParkAdmin.Domain.Reportes.Filters;
namespace OzyParkAdmin.Components.Admin.Reportes.Controls.Filters;

/// <summary>
/// El editor de filtro base.
/// </summary>
public abstract class FilterEdit : ComponentBase, IDisposable
{
    /// <summary>
    /// El <see cref="ISnackbar"/>.
    /// </summary>
    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    /// <summary>
    /// El layout del filtro.
    /// </summary>
    [CascadingParameter]
    public FilterLayout? FilterLayout { get; set; }

    /// <summary>
    /// Indicador de cuando se está cargando o guardando.
    /// </summary>
    [Parameter]
    public bool Loading { get; set; }

    /// <summary>
    /// Callback que se ejecuta cuando el <see cref="Loading"/> cambió.
    /// </summary>
    [Parameter]
    public EventCallback<bool> LoadingChanged { get; set; }

    internal abstract IFilterModel? FilterModel { get; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        FilterLayout?.AddFilterEdit(this);
    }

    /// <summary>
    /// Ejecuta el filtro usando el filtro padre.
    /// </summary>
    /// <returns>Una tarea asíncrona que representa la ejecución de la operación.</returns>
    public virtual Task ExecuteFilerAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Carga información de forma remote.
    /// </summary>
    /// <returns>La información que se carga de forma remota.</returns>
    protected async Task<List<ItemOption>> LoadDataAsync()
    {
        if (FilterModel is not null && FilterLayout is not null)
        {
            try
            {
                await ChangeLoading(true);
                return await FilterLayout.LoadFilterData(FilterModel);
            }
            finally
            {
                await ChangeLoading(false);
            }
        }

        return [];
    }

    private Task ChangeLoading(bool loading)
    {
        Loading = loading;
        return LoadingChanged.InvokeAsync(Loading);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Libera el filtro de la colección.
    /// </summary>
    /// <param name="disposing">Si se libera el elemento o no.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            FilterLayout?.RemoveFilterEdit(this);
        }
    }
}
