namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa el resultado de algún elemento de <typeparamref name="T"/> o nada.
/// </summary>
/// <typeparam name="T">El tipo del elemento.</typeparam>
public sealed class SomeOrNone<T>
{
    private readonly T? _some;

    private SomeOrNone(T? some) =>
        _some = some;

    /// <summary>
    /// Si existe algún elemento de <typeparamref name="T"/>.
    /// </summary>
    public bool IsSome => _some is not null;

    /// <summary>
    /// Si no existe algún elemento de <typeparamref name="T"/>.
    /// </summary>
    public bool IsNone => _some is null;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SomeOrNone{T}"/> con algún elemento de <typeparamref name="T"/>.
    /// </summary>
    /// <param name="some">El valor de <typeparamref name="T"/>.</param>
    /// <returns>El <see cref="SomeOrNone{T}"/> con algún elemento de <typeparamref name="T"/>.</returns>
    public static SomeOrNone<T> Some(T some) =>
        new(some);

    /// <summary>
    /// Crea una nueva instancia de <see cref="SomeOrNone{T}"/> sin ningún elemento de <typeparamref name="T"/>.
    /// </summary>
    /// <returns>El <see cref="SomeOrNone{T}"/> sin ningún elemento de <typeparamref name="T"/>.</returns>
    public static SomeOrNone<T> None() =>
        new(default);

    /// <summary>
    /// Ejecuta una acción cuando hay algún elemento y una acción con no hay ningún elemento.
    /// </summary>
    /// <param name="onSome">La acción a ejecutar cuando hay algún elemento.</param>
    /// <param name="onNone">La acción a ejecutar cuando no hay ningún elemento.</param>
    public void Switch(Action<T> onSome, Action onNone)
    {
        if (_some is not null)
        {
            onSome(_some);
            return;
        }

        onNone();
    }

    /// <summary>
    /// Ejecuta una acción cuando hay algún elemento y una acción con no hay ningún elemento.
    /// </summary>
    /// <param name="onSome">La acción a ejecutar cuando hay algún elemento.</param>
    /// <param name="onNone">La acción a ejecutar cuando no hay ningún elemento.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La tarea que representa una operación asíncrona.</returns>
    public async Task SwitchAsync(Func<T, CancellationToken, Task> onSome, Func<CancellationToken, Task> onNone, CancellationToken cancellationToken)
    {
        if (_some is not null)
        {
            await onSome(_some, cancellationToken);
            return;
        }

        await onNone(cancellationToken);
    }

    /// <summary>
    /// Ejecuta una acción cuando hay algún elemento y una acción con no hay ningún elemento.
    /// </summary>
    /// <param name="onSome">La acción a ejecutar cuando hay algún elemento.</param>
    /// <param name="onNone">La acción a ejecutar cuando no hay ningún elemento.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La tarea que representa una operación asíncrona.</returns>
    public async Task SwitchAsync(Func<T, CancellationToken, Task> onSome, Action onNone, CancellationToken cancellationToken)
    {
        if (_some is not null)
        {
            await onSome(_some, cancellationToken);
            return;
        }

        onNone();
    }

    /// <summary>
    /// Ejecuta una acción cuando hay algún elemento y una acción con no hay ningún elemento.
    /// </summary>
    /// <param name="onSome">La acción a ejecutar cuando hay algún elemento.</param>
    /// <param name="onNone">La acción a ejecutar cuando no hay ningún elemento.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La tarea que representa una operación asíncrona.</returns>
    public async Task SwitchAsync(Action<T> onSome, Func<CancellationToken, Task> onNone, CancellationToken cancellationToken)
    {
        if (_some is not null)
        {
            onSome(_some);
            return;
        }

        await onNone(cancellationToken);
    }

    /// <summary>
    /// Ejecuta una acción cuando hay algún elemento y una acción con no hay ningún elemento que devuelve como resultado <typeparamref name="TResult"/>.
    /// </summary>
    /// <param name="onSome">La acción a ejecutar cuando hay algún elemento.</param>
    /// <param name="onNone">La acción a ejecutar cuando no hay ningún elemento.</param>
    /// <returns>El resultado de ejecutar <paramref name="onSome"/> o <paramref name="onNone"/>.</returns>
    public TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone)
    {
        if (_some is not null)
        {
            return onSome(_some);
        }

        return onNone();
    }

    /// <summary>
    /// Ejecuta una acción cuando hay algún elemento y una acción con no hay ningún elemento que devuelve como resultado <typeparamref name="TResult"/>.
    /// </summary>
    /// <param name="onSome">La acción a ejecutar cuando hay algún elemento.</param>
    /// <param name="onNone">La acción a ejecutar cuando no hay ningún elemento.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>
    /// La tarea que representa una operación asíncrona.
    /// El resultado de ejecutar <paramref name="onSome"/> o <paramref name="onNone"/>.
    /// </returns>
    public Task<TResult> MatchAsync<TResult>(Func<T, CancellationToken, Task<TResult>> onSome, Func<CancellationToken, Task<TResult>> onNone, CancellationToken cancellationToken)
    {
        if (_some is not null)
        {
            return onSome(_some, cancellationToken);
        }

        return onNone(cancellationToken);
    }

    /// <summary>
    /// Ejecuta una acción cuando hay algún elemento y una acción con no hay ningún elemento que devuelve como resultado <typeparamref name="TResult"/>.
    /// </summary>
    /// <param name="onSome">La acción a ejecutar cuando hay algún elemento.</param>
    /// <param name="onNone">La acción a ejecutar cuando no hay ningún elemento.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>
    /// La tarea que representa una operación asíncrona.
    /// El resultado de ejecutar <paramref name="onSome"/> o <paramref name="onNone"/>.
    /// </returns>
    public async Task<TResult> MatchAsync<TResult>(Func<T, CancellationToken, Task<TResult>> onSome, Func<TResult> onNone, CancellationToken cancellationToken)
    {
        if (_some is not null)
        {
            return await onSome(_some, cancellationToken);
        }

        return onNone();
    }

    /// <summary>
    /// Ejecuta una acción cuando hay algún elemento y una acción con no hay ningún elemento que devuelve como resultado <typeparamref name="TResult"/>.
    /// </summary>
    /// <param name="onSome">La acción a ejecutar cuando hay algún elemento.</param>
    /// <param name="onNone">La acción a ejecutar cuando no hay ningún elemento.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>
    /// La tarea que representa una operación asíncrona.
    /// El resultado de ejecutar <paramref name="onSome"/> o <paramref name="onNone"/>.
    /// </returns>
    public async Task<TResult> MatchAsync<TResult>(Func<T, TResult> onSome, Func<CancellationToken, Task<TResult>> onNone, CancellationToken cancellationToken)
    {
        if (_some is not null)
        {
            return onSome(_some);
        }

        return await onNone(cancellationToken);
    }

    /// <summary>
    /// Convierte el tipo <paramref name="some"/> en <see cref="SomeOrNone{T}"/>.
    /// </summary>
    /// <param name="some">El valor de <typeparamref name="T"/>.</param>
    public static implicit operator SomeOrNone<T>(T some) => new(some);

    /// <summary>
    /// Permite crear un <see cref="SomeOrNone{T}"/> que no contiene algún elemento de <typeparamref name="T"/>.
    /// </summary>
    /// <param name="_">El <see cref="None"/>.</param>
    public static implicit operator SomeOrNone<T>(None _) => new(default);
}
