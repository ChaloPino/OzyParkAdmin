using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Domain.Reportes.Filters;

/// <summary>
/// El filtro oculto, usado para registrar valores constantes o computados en el filtrado.
/// </summary>
public sealed class HiddenFilter : Filter
{
    private HiddenFilter()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="HiddenFilter"/>.
    /// </summary>
    /// <param name="report">El reporte al que pertenece el filtro.</param>
    /// <param name="id">El identificador del filtro.</param>
    public HiddenFilter(Report report, int id)
        : base(report, id)
    {
    }

    /// <summary>
    /// El valor predeterminado del filtro.
    /// </summary>
    public string? DefaultValue { get; private set; }

    /// <summary>
    /// El valor condicional del filtro.
    /// </summary>
    public string? MappedTo { get; private set; }

    /// <inheritdoc/>
    public override object? GetDefaultValue() =>
        DefaultValue;
}
