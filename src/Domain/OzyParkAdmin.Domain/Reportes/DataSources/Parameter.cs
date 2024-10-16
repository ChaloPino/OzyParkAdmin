using System.Data;

namespace OzyParkAdmin.Domain.Reportes.DataSources;

/// <summary>
/// El parámetro de una fuente de datos.
/// </summary>
public sealed class Parameter
{
    private Parameter()
    {
    }

    internal Parameter(DataSource dataSource, IParameterInfo parameterInfo, int order)
    {
        DataSourceId = dataSource.Id;
        Name = parameterInfo.Name.Trim();
        Type = parameterInfo.DbType;
        AlternativeType = parameterInfo.AlternativeType;
        AlternativeTypeName = parameterInfo.AlternativeTypeName;
        Order = order;
    }

    /// <summary>
    /// El id de la fuente de datos.
    /// </summary>
    public Guid DataSourceId { get; private set; }

    /// <summary>
    /// El nombre del parámetro.
    /// </summary>
    public string Name { get; private set; } = default!;

    /// <summary>
    /// El tipo de dato del parámetro.
    /// </summary>
    public DbType Type { get; private set; }

    /// <summary>
    /// El id del tipo de dato alternativo.
    /// </summary>
    /// <remarks>
    /// Usado para otros tipos de datos como tipos tabla en SQL Server.
    /// </remarks>
    public int? AlternativeType { get; private set; }

    /// <summary>
    /// El nombre del tipo de dato alternativo.
    /// </summary>
    /// <remarks>
    /// Usado para otros tipos de datos como tipos tabla en SQL Server.
    /// </remarks>
    public string? AlternativeTypeName { get; private set; }

    /// <summary>
    /// El orden de despliegue.
    /// </summary>
    public int Order { get; private set; }

    internal void ChangeDbType(DbType type)
    {
        Type = type;
    }

    internal void ChangeOrder(int order)
    {
        Order = order;
    }
}
