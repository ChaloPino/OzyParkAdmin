using System.Data;

namespace OzyParkAdmin.Domain.Reportes.DataSources;

/// <summary>
/// Información de un parámetro.
/// </summary>
public interface IParameterInfo
{
    /// <summary>
    /// El nombre del parámetro.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// El tipo de dato del parámetro.
    /// </summary>
    DbType DbType { get; }

    /// <summary>
    /// El identificador del tipo de dato alternativo.
    /// </summary>
    /// <remarks>
    /// Usado por ejemplo, para tipo tablas de SQL Server.
    /// </remarks>
    int? AlternativeType { get; }

    /// <summary>
    /// El nombre del tipo alternativo.
    /// </summary>
    /// <remarks>
    /// Usado por ejemplo, para tipo tablas de SQL Server.
    /// </remarks>
    string? AlternativeTypeName { get; }
}