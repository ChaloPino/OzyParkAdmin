using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace OzyParkAdmin.Domain.Shared;
/// <summary>
/// Contiene funcionalidades que extienden el <see cref="FilterExpressionCollection{T}"/>.
/// </summary>
public static class FilterExpressionCollectionExtensions
{
    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, bool?>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new BooleanFilterExpression<T>(memberExpression, @operator, (bool?)value));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, bool>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new BooleanFilterExpression<T>(memberExpression, @operator, (bool?)value ?? default));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, byte?>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new NumberFilterExpression<T, byte>(memberExpression, @operator, (double?)value));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, byte>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new NumberFilterExpression<T, byte>(memberExpression, @operator, (double?)value ?? default));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, short?>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new NumberFilterExpression<T, short>(memberExpression, @operator, (double?)value));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, short>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new NumberFilterExpression<T, short>(memberExpression, @operator, (double?)value ?? default));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, int?>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new NumberFilterExpression<T, int>(memberExpression, @operator, (double?)value));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, int>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new NumberFilterExpression<T, int>(memberExpression, @operator, (double?)value ?? default));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, long?>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new NumberFilterExpression<T, long>(memberExpression, @operator, (double?)value));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, long>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new NumberFilterExpression<T, long>(memberExpression, @operator, (double?)value ?? default));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, decimal?>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new NumberFilterExpression<T, decimal>(memberExpression, @operator, (double?)value));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, decimal>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new NumberFilterExpression<T, decimal>(memberExpression, @operator, (double?)value ?? default));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, float?>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new NumberFilterExpression<T, float>(memberExpression, @operator, (double?)value));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, float>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new NumberFilterExpression<T, float>(memberExpression, @operator, (float?)value ?? default));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, double?>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new NumberFilterExpression<T, double>(memberExpression, @operator, (double?)value));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, double>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new NumberFilterExpression<T, double>(memberExpression, @operator, (double?)value ?? default));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, DateOnly?>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new DateAndTimeFilterExpression<T, DateOnly>(memberExpression, @operator, (DateOnly?)value));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, DateOnly>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new DateAndTimeFilterExpression<T, DateOnly>(memberExpression, @operator, (DateOnly?)value ?? default));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, DateTime?>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new DateAndTimeFilterExpression<T, DateTime>(memberExpression, @operator, (DateTime?)value));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, DateTime>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new DateAndTimeFilterExpression<T, DateTime>(memberExpression, @operator, (DateTime?)value ?? default));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, DateTimeOffset?>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new DateAndTimeFilterExpression<T, DateTimeOffset>(memberExpression, @operator, (DateTimeOffset?)value));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, DateTimeOffset>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new DateAndTimeFilterExpression<T, DateTimeOffset>(memberExpression, @operator, (DateTimeOffset?)value ?? default));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, TimeOnly?>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new DateAndTimeFilterExpression<T, TimeOnly>(memberExpression, @operator, (TimeOnly?)value));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, TimeOnly>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new DateAndTimeFilterExpression<T, TimeOnly>(memberExpression, @operator, (TimeOnly?)value ?? default));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, TimeSpan?>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new DateAndTimeFilterExpression<T, TimeSpan>(memberExpression, @operator, (TimeSpan?)value));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, TimeSpan>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new DateAndTimeFilterExpression<T, TimeSpan>(memberExpression, @operator, (TimeSpan?)value ?? default));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, Guid?>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new GuidFilterExpression<T>(memberExpression, @operator, (Guid?)value));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, Guid>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new GuidFilterExpression<T>(memberExpression, @operator, (Guid?)value ?? Guid.Empty));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, string?>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new StringFilterExpression<T>(memberExpression, @operator, (string?)value));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T, TEnum>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, TEnum?>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
        where TEnum : struct, Enum
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new EnumFilterExpression<T, TEnum>(memberExpression, @operator, (TEnum?)value));
        return filterExpressions;
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpressions">La colección de expresiones de filtrado.</param>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    /// <param name="operator">La operación.</param>
    /// <param name="value">El valor a comparar.</param>
    /// <param name="createIfNull">Una marca para definir si se crea una expresión cuando el <paramref name="value"/> es <c>null</c>.</param>
    public static FilterExpressionCollection<T> Add<T, TEnum>(this FilterExpressionCollection<T> filterExpressions, Expression<Func<T, TEnum>> memberExpression, string @operator, object? value = null, bool createIfNull = false)
        where T : class
        where TEnum : struct, Enum
    {
        if (value is null && !createIfNull)
        {
            return filterExpressions;
        }

        filterExpressions.Add(new EnumFilterExpression<T, TEnum>(memberExpression, @operator, (TEnum?)value ?? default));
        return filterExpressions;
    }
}
