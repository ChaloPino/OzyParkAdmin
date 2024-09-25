using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa una expressión de filtrado para el <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">El tipo del elemento a filtrar.</typeparam>
public interface IFilterExpression<T>
    where T : class
{
    /// <summary>
    /// El nombre del miembro que se está usando para el filtrado.
    /// </summary>
    public string MemberName { get; }

    /// <summary>
    /// Reemplaza el miembro de filtrado.
    /// </summary>
    /// <param name="replacement">La expresión lambda que reemplazará la actual.</param>
    void Replace(LambdaExpression replacement);

    /// <summary>
    /// Ejecuta el filtrado definido.
    /// </summary>
    /// <param name="query">La consulta que será filtrada.</param>
    /// <returns>Una nueva consulta filtrada.</returns>
    IQueryable<T> Where(IQueryable<T> query);
}