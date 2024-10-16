using MudBlazor;
using OzyParkAdmin.Domain.Reportes.DataSources;
using OzyParkAdmin.Domain.Reportes.Filters;
using System.Xml.Linq;

namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// El modelo de filtro.
/// </summary>
public sealed class FilterViewModel
{
    private readonly string _layout;

    internal FilterViewModel(string aka, IEnumerable<Filter> filters, string layout)
    {
        _layout = $"<div>{layout}</div>";
        ReportAka = aka;
        ProcessLayout(filters);
    }

    private void ProcessLayout(IEnumerable<Filter> filters)
    {
        Dictionary<Filter, (SizeLayout Size, int Row)> filtersLayout = [];
        XDocument document = XDocument.Parse(_layout);

        XElement root = document.Root!;
        ProcessLayout(root, filters, 0, filtersLayout);

        Filters = filters.Select(filter => CreateFilterModel(filter, filtersLayout)).Where(x => x is not null).Select(x => x!).ToList();
    }

    private static void ProcessLayout(XElement root, IEnumerable<Filter> filters, int row, Dictionary<Filter, (SizeLayout Size, int Row)> layouts)
    {
        foreach (var item in root.Elements("div"))
        {
            if (!item.HasElements)
            {
                Filter? filter = filters.FirstOrDefault(x => string.Equals(x.Name, item.Value.Trim().Trim('@')));

                if (filter is not null)
                {
                    layouts[filter] = (ProcessItemSize(item), row);
                }
            }
            else
            {
                ProcessLayout(item, filters, row++, layouts);
            }
        }
    }

    /// <summary>
    /// El aka del reporte.
    /// </summary>
    public string ReportAka { get; }

    /// <summary>
    /// Los filtros del view model.
    /// </summary>
    public List<IFilterModel> Filters { get; private set; } = [];


    private IFilterModel? CreateFilterModel(Filter filter, Dictionary<Filter, (SizeLayout Size, int Row)> layouts)
    {
        layouts.TryGetValue(filter, out var layout);

        return filter switch
        {
            CheckFilter checkFilter => new CheckFilterModel(this, checkFilter, layout.Row, layout.Size),
            DateFilter dateFilter => new DateFilterModel(this, dateFilter, layout.Row, layout.Size),
            HiddenFilter hiddenFilter => new HiddenFilterModel(this, hiddenFilter, layout.Row, layout.Size),
            ListFilter listFilter  => CreateListFilterModel(listFilter, layout.Row, layout.Size),
            MonthFilter monthFilter => new MonthFilterModel(this, monthFilter, layout.Row, layout.Size),
            TextFilter textFilter => CreateTextFilterModel(textFilter, layout.Row, layout.Size),
            TimeFilter timeFilter => new TimeFilterModel(this, timeFilter, layout.Row, layout.Size),
            _ => null,
        };
    }

    private IFilterModel CreateTextFilterModel(TextFilter textFilter, int row, SizeLayout size) =>
        textFilter.Parameter is null || IsText(textFilter.Parameter) ? new TextFilterModel(this, textFilter, row, size) : new NumberFilterModel(this, textFilter, row, size);

    private IFilterModel CreateListFilterModel(ListFilter listFilter, int row, SizeLayout size) =>
        listFilter is { IsMultiple: false } ? new ListFilterModel(this, listFilter, row, size) : new MultiListFilterModel(this, listFilter, row, size);

    private static bool IsText(Parameter parameter) =>
        parameter.Type is System.Data.DbType.AnsiString or System.Data.DbType.AnsiStringFixedLength or System.Data.DbType.String or System.Data.DbType.StringFixedLength;

    private static SizeLayout ProcessItemSize(XElement item)
    {
        string? classes = item.Attribute("class")?.Value;
        int xs = 0, sm = 0, md = 0, lg = 0, xl = 0, xxl = 0;

        if (classes?.Contains("col-") == true)
        {
            foreach (string sizeClass in classes.Split(' ').Where(s => s.StartsWith("col-")))
            {
                string[] sizeParts = sizeClass.Split('-');

                if (sizeParts.Length == 2)
                {
                    xs = int.Parse(sizeParts[1]);
                    continue;
                }

                if (sizeParts.Length == 3)
                {
                    switch (sizeParts[1])
                    {
                        case "xs":
                            xs = int.Parse(sizeParts[2]);
                            break;
                        case "sm":
                            sm = int.Parse(sizeParts[2]);
                            break;
                        case "md":
                            md = int.Parse(sizeParts[2]);
                            break;
                        case "lg":
                            lg = int.Parse(sizeParts[2]);
                            break;
                        case "xl":
                            xl = int.Parse(sizeParts[2]);
                            break;
                        case "xxl":
                            xxl = int.Parse(sizeParts[2]);
                            break;
                    }
                }
            }
        }

        return new(xs, sm, md, lg, xl, xxl);
    }

    /// <summary>
    /// Consigue el modelo del filtro dado el filtro.
    /// </summary>
    /// <param name="filter">El filtro a buscar.</param>
    /// <returns>El modelo del filtro si existe.</returns>
    public IFilterModel? FindFilter(Filter? filter) =>
        filter is null ? null : Filters.Find(x => x.Id == filter.Id);

    /// <summary>
    /// <summary>
    /// Consigue los valores de los filtros padres.
    /// </summary>
    /// <returns>Los valores de los filtros padres.</returns>
    public string?[] GetParentValuesForFilter(IFilterModel filterModel)
    {
        return Filters.Where(x => filterModel.ParentFilters.Contains(x.Id)).Select(x => x.GetValue()?.ToString()).ToArray();
    }

    /// <summary>
    /// Consigue todos los filtros que son dependientes del <paramref name="parentFilter"/>.
    /// </summary>
    /// <param name="parentFilter">El filtro padre.</param>
    /// <returns>Todos los filtros que son dependientes del <paramref name="parentFilter"/>.</returns>
    public IEnumerable<IFilterModel> FindDependantFilters(IFilterModel parentFilter)
    {
        return Filters.Where(x => x.ParentFilters.Contains(parentFilter.Id));
    }
}
