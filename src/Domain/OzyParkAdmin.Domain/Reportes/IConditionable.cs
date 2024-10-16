namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// Contiene la configuración de elementos condicionales.
/// </summary>
public interface IConditionable
{
    /// <summary>
    /// Si tiene estilo condicional.
    /// </summary>
    bool HasConditionalStyle { get; }

    /// <summary>
    /// El estilo condicional para que se pinte el valor como exitoso.
    /// </summary>
    ConditionalStyle? SuccessStyle { get; }

    /// <summary>
    /// El operador condicional para evaluar el valor como exitoso.
    /// </summary>
    OperatorStyleType? SuccessOperator { get; }

    /// <summary>
    /// El valor condicional para evaluar el valor de la columna como exitoso.
    /// </summary>
    string? SuccessConditionalValue { get; }

    /// <summary>
    /// El valor alternativo condicional para evaluar el valor de la columna como exitoso.
    /// Se usa para evaluaciones de tipo rango como el operador <see cref="OperatorStyleType.Between"/>.
    /// </summary>
    string? SuccessAlternateConditionalValue { get; }

    /// <summary>
    /// El estilo condicional para que se pinte el valor como advertencia.
    /// </summary>
    ConditionalStyle? WarningStyle { get; }

    /// <summary>
    /// El operador condicional para evaluar el valor como advertencia.
    /// </summary>
    OperatorStyleType? WarningOperator { get; }

    /// <summary>
    /// El valor condicional para evaluar el valor de la columna como advertencia.
    /// </summary>
    string? WarningConditionalValue { get; }

    /// <summary>
    /// El valor alternativo condicional para evaluar el valor de la columna como advertencia.
    /// Se usa para evaluaciones de tipo rango como el operador <see cref="OperatorStyleType.Between"/>.
    /// </summary>
    string? WarningAlternateConditionalValue { get; }

    /// <summary>
    /// El estilo condicional para que se pinte el valor como error.
    /// </summary>
    ConditionalStyle? ErrorStyle { get; }
}
