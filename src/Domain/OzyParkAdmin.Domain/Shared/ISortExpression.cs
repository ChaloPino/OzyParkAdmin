using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa una expressión de ordenamiento para el <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">El tipo de elemento a ordenar.</typeparam>
public interface ISortExpression<T>
    where T : class
{
    /// <summary>
    /// El nombre del miembro que realizará el ordenamiento.
    /// </summary>
    string MemberName { get; }

    /// <summary>
    /// Reemplaza el miembro de ordenamiento.
    /// </summary>
    /// <param name="replacement">La expresión lambda que reemplazará el actual.</param>
    void Replace(LambdaExpression replacement);

    /// <summary>
    /// Ejecuta el ordenamiento definido.
    /// </summary>
    /// <param name="query">La consulta que será ordenada.</param>
    /// <returns>Una nueva consulta ordenada.</returns>
    IOrderedQueryable<T> Sort(IQueryable<T> query);

    /// <summary>
    /// Ejecuta el ordenamiento definido.
    /// </summary>
    /// <param name="source">La fuente que será ordenada.</param>
    /// <returns>Una nueva consulta ordenada.</returns>
    IOrderedEnumerable<T> Sort(IEnumerable<T> source);


    /// <summary>
    /// Ejecuta el ordenamiento definido.
    /// </summary>
    /// <param name="query">La consulta que será ordenada.</param>
    /// <returns>Una nueva consulta ordenada.</returns>
    IOrderedQueryable<T> ThenSort(IOrderedQueryable<T> query);

    /// <summary>
    /// Ejecuta el ordenamiento definido.
    /// </summary>
    /// <param name="source">La fuente que será ordenada.</param>
    /// <returns>Una nueva consulta ordenada.</returns>
    IOrderedEnumerable<T> ThenSort(IOrderedEnumerable<T> source);
}